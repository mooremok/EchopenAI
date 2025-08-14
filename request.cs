using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // 需要安装Newtonsoft.Json NuGet包

namespace EchopenAIExample
{
    /// <summary>
    /// EchopenAI API 响应模型
    /// </summary>
    public class ApiResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        
        [JsonProperty("output_text")]
        public string OutputText { get; set; }
        
        [JsonProperty("input_text_count")]
        public int InputTextCount { get; set; }
        
        [JsonProperty("service_type")]
        public string ServiceType { get; set; }
    }
    
    /// <summary>
    /// EchopenAI API 客户端
    /// </summary>
    public class EchopenAIClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.llmapi.fit/completion/reduce";
        
        public EchopenAIClient(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }
        
        /// <summary>
        /// 调用API进行文本处理
        /// </summary>
        /// <param name="text">要处理的文本</param>
        /// <param name="serviceType">服务类型: jc(降重)/aigc(降AIGC)/complex(复合)</param>
        /// <returns>API响应对象，失败返回null</returns>
        public async Task<ApiResponse> CallAPIAsync(string text, string serviceType = "jc")
        {
            try
            {
                // 设置请求头
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                
                // 构建请求体
                var requestBody = new
                {
                    text = text,
                    service_type = serviceType
                };
                
                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                // 发送POST请求
                Console.WriteLine("正在发送请求...");
                var response = await _httpClient.PostAsync(BaseUrl, content);
                
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    // 解析成功响应
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    Console.WriteLine("✅ 请求成功!");
                    return apiResponse;
                }
                else
                {
                    // 处理错误响应
                    Console.WriteLine($"❌ 请求失败 (状态码: {(int)response.StatusCode})");
                    Console.WriteLine($"响应内容: {responseContent}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"❌ 网络请求异常: {e.Message}");
                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine($"❌ 请求超时: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ 未知错误: {e.Message}");
                return null;
            }
        }
        
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
    
    class Program
    {
        static async Task Main(string[] args)
        {
            // 请替换为您的实际API密钥
            string apiKey = "your_api_key_here";
            
            // 创建客户端实例
            using var client = new EchopenAIClient(apiKey);
            
            // 要处理的文本
            string text = "高村乡农村集体经济发展的重要因素是带头人的能力水平...";
            
            // 调用API (可以修改serviceType参数)
            var result = await client.CallAPIAsync(text, "jc"); // jc(降重), aigc(降AIGC), complex(复合)
            
            if (result != null)
            {
                Console.WriteLine($"请求ID: {result.RequestId}");
                Console.WriteLine($"输入字数: {result.InputTextCount}");
                Console.WriteLine($"服务类型: {result.ServiceType}");
                Console.WriteLine("处理结果:");
                Console.WriteLine(result.OutputText);
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}