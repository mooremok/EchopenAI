package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"net/http"
)

// RequestBody API请求体结构
type RequestBody struct {
	Text        string `json:"text"`
	ServiceType string `json:"service_type"`
}

// ResponseBody API响应体结构
type ResponseBody struct {
	Code             string `json:"code"`
	Message          string `json:"message"`
	RequestID        string `json:"request_id"`
	OutputText       string `json:"output_text"`
	InputTextCount   int    `json:"input_text_count"`
	ServiceType      string `json:"service_type"`
}

// APIClient EchopenAI API客户端
type APIClient struct {
	BaseURL    string
	APIKey     string
	HTTPClient *http.Client
}

// NewAPIClient 创建新的API客户端
func NewAPIClient(apiKey string) *APIClient {
	return &APIClient{
		BaseURL:    "https://api.llmapi.fit",
		APIKey:     apiKey,
		HTTPClient: &http.Client{},
	}
}

// CallAPI 调用API进行文本处理
func (c *APIClient) CallAPI(text, serviceType string) (*ResponseBody, error) {
	// 构建请求体
	requestBody := RequestBody{
		Text:        text,
		ServiceType: serviceType,
	}
	
	jsonData, err := json.Marshal(requestBody)
	if err != nil {
		return nil, fmt.Errorf("序列化请求体失败: %v", err)
	}
	
	// 创建请求
	req, err := http.NewRequest("POST", c.BaseURL+"/completion/reduce", bytes.NewBuffer(jsonData))
	if err != nil {
		return nil, fmt.Errorf("创建请求失败: %v", err)
	}
	
	// 设置请求头
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", "Bearer "+c.APIKey)
	
	// 发送请求
	resp, err := c.HTTPClient.Do(req)
	if err != nil {
		return nil, fmt.Errorf("发送请求失败: %v", err)
	}
	defer resp.Body.Close()
	
	// 读取响应体
	body, err := io.ReadAll(resp.Body)
	if err != nil {
		return nil, fmt.Errorf("读取响应体失败: %v", err)
	}
	
	// 解析响应
	var response ResponseBody
	if err := json.Unmarshal(body, &response); err != nil {
		return nil, fmt.Errorf("解析响应JSON失败: %v", err)
	}
	
	// 检查响应状态
	if resp.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("API请求失败 (状态码: %d): %s", resp.StatusCode, response.Message)
	}
	
	return &response, nil
}

func main() {
	// 请替换为您的实际API密钥
	apiKey := "your_api_key_here"
	
	// 创建客户端
	client := NewAPIClient(apiKey)
	
	// 要处理的文本
	text := "高村乡农村集体经济发展的重要因素是带头人的能力水平..."
	
	// 调用API (可以修改serviceType参数)
	response, err := client.CallAPI(text, "jc") // jc(降重), aigc(降AIGC), complex(复合)
	if err != nil {
		fmt.Printf("❌ 请求失败: %v\n", err)
		return
	}
	
	// 输出结果
	fmt.Println("✅ 请求成功!")
	fmt.Printf("请求ID: %s\n", response.RequestID)
	fmt.Printf("输入字数: %d\n", response.InputTextCount)
	fmt.Printf("服务类型: %s\n", response.ServiceType)
	fmt.Printf("处理结果:\n%s\n", response.OutputText)
}