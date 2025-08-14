<?php
/**
 * EchopenAI API PHP 示例
 */

class EchopenAIClient {
    private $apiKey;
    private $baseUrl = 'https://api.llmapi.fit';
    
    public function __construct($apiKey) {
        $this->apiKey = $apiKey;
    }
    
    /**
     * 调用API进行文本处理
     * 
     * @param string $text 要处理的文本
     * @param string $serviceType 服务类型: jc(降重)/aigc(降AIGC)/complex(复合)
     * @return array|false 成功返回结果数组，失败返回false
     */
    public function callAPI($text, $serviceType = 'jc') {
        $url = $this->baseUrl . '/completion/reduce';
        
        $headers = [
            'Content-Type: application/json',
            'Authorization: Bearer ' . $this->apiKey
        ];
        
        $data = [
            'text' => $text,
            'service_type' => $serviceType
        ];
        
        $options = [
            'http' => [
                'header'  => $headers,
                'method'  => 'POST',
                'content' => json_encode($data, JSON_UNESCAPED_UNICODE),
                'timeout' => 30
            ]
        ];
        
        $context = stream_context_create($options);
        
        try {
            $result = file_get_contents($url, false, $context);
            
            if ($result === false) {
                throw new Exception('网络请求失败');
            }
            
            $response = json_decode($result, true);
            
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw new Exception('JSON解析失败: ' . json_last_error_msg());
            }
            
            if (!isset($response['code']) || $response['code'] !== 'success') {
                $message = $response['message'] ?? '未知错误';
                throw new Exception("API请求失败: {$message}");
            }
            
            return $response;
            
        } catch (Exception $e) {
            echo "❌ 请求失败: " . $e->getMessage() . "\n";
            return false;
        }
    }
}

// 使用示例
// 请替换为您的实际API密钥
$apiKey = 'your_api_key_here';

// 创建客户端实例
$client = new EchopenAIClient($apiKey);

// 要处理的文本
$text = "高村乡农村集体经济发展的重要因素是带头人的能力水平...";

// 调用API (可以修改serviceType参数)
$result = $client->callAPI($text, 'jc'); // jc(降重), aigc(降AIGC), complex(复合)

if ($result) {
    echo "✅ 请求成功!\n";
    echo "请求ID: " . $result['request_id'] . "\n";
    echo "输入字数: " . $result['input_text_count'] . "\n";
    echo "服务类型: " . $result['service_type'] . "\n";
    echo "处理结果:\n" . $result['output_text'] . "\n";
}
?>