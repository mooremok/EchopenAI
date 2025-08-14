import java.io.*;
import java.net.*;
import java.nio.charset.StandardCharsets;

/**
 * EchopenAI API Java 示例
 */
public class EchopenAIClient {
    private final String apiKey;
    private final String baseUrl = "https://api.llmapi.fit";
    
    public EchopenAIClient(String apiKey) {
        this.apiKey = apiKey;
    }
    
    /**
     * 调用API进行文本处理
     * 
     * @param text 要处理的文本
     * @param serviceType 服务类型: jc(降重)/aigc(降AIGC)/complex(复合)
     * @return API响应JSON字符串，失败返回null
     */
    public String callAPI(String text, String serviceType) {
        try {
            URL url = new URL(baseUrl + "/completion/reduce");
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            
            // 设置请求方法和超时
            connection.setRequestMethod("POST");
            connection.setConnectTimeout(30000);
            connection.setReadTimeout(30000);
            
            // 设置请求头
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Authorization", "Bearer " + apiKey);
            connection.setDoOutput(true);
            
            // 构建请求体
            String jsonInputString = String.format(
                "{\"text\": \"%s\", \"service_type\": \"%s\"}",
                text.replace("\"", "\\\""),
                serviceType
            );
            
            // 发送请求
            try (OutputStream os = connection.getOutputStream()) {
                byte[] input = jsonInputString.getBytes(StandardCharsets.UTF_8);
                os.write(input, 0, input.length);
            }
            
            // 读取响应
            int responseCode = connection.getResponseCode();
            InputStream inputStream;
            
            if (responseCode >= 200 && responseCode < 300) {
                inputStream = connection.getInputStream();
            } else {
                inputStream = connection.getErrorStream();
            }
            
            StringBuilder response = new StringBuilder();
            try (BufferedReader br = new BufferedReader(
                    new InputStreamReader(inputStream, StandardCharsets.UTF_8))) {
                String responseLine;
                while ((responseLine = br.readLine()) != null) {
                    response.append(responseLine.trim());
                }
            }
            
            connection.disconnect();
            
            // 检查响应状态
            if (responseCode >= 200 && responseCode < 300) {
                System.out.println("✅ 请求成功!");
                return response.toString();
            } else {
                System.out.println("❌ 请求失败 (状态码: " + responseCode + ")");
                System.out.println("响应: " + response.toString());
                return null;
            }
            
        } catch (IOException e) {
            System.out.println("❌ 网络请求异常: " + e.getMessage());
            return null;
        }
    }
    
    public static void main(String[] args) {
        // 请替换为您的实际API密钥
        String apiKey = "your_api_key_here";
        
        // 创建客户端实例
        EchopenAIClient client = new EchopenAIClient(apiKey);
        
        // 要处理的文本
        String text = "高村乡农村集体经济发展的重要因素是带头人的能力水平...";
        
        // 调用API (可以修改serviceType参数)
        String jsonResponse = client.callAPI(text, "jc"); // jc(降重), aigc(降AIGC), complex(复合)
        
        if (jsonResponse != null) {
            // 这里可以使用JSON库（如Jackson或Gson）来解析JSON
            // 为简单起见，这里直接打印原始JSON
            System.out.println("原始响应JSON:");
            System.out.println(jsonResponse);
        }
    }
}