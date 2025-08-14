import json

import requests

def call_echopenai_api(api_key, text, service_type="jc"):
    """
    调用EchopenAI API进行文本处理
    
    Args:
        api_key (str): API密钥
        text (str): 需要处理的文本
        service_type (str): 服务类型 jc(降重)/aigc(降AIGC)/complex(复合)
    
    Returns:
        dict: API响应结果
    """
    
    url = "https://api.llmapi.fit/completion/reduce"
    
    headers = {
        "Content-Type": "application/json",
        "Authorization": f"Bearer {api_key}"
    }
    
    payload = {
        "text": text,
        "service_type": service_type
    }
    
    try:
        response = requests.post(url, headers=headers, json=payload, timeout=30)
        
        if response.status_code == 200:
            result = response.json()
            print("✅ 请求成功!")
            print(f"请求ID: {result['request_id']}")
            print(f"输入字数: {result['input_text_count']}")
            print(f"服务类型: {result['service_type']}")
            print(f"处理结果:\n{result['output_text']}")
            return result
            
        else:
            error_data = response.json()
            print(f"❌ 请求失败 (状态码: {response.status_code})")
            print(f"错误代码: {error_data.get('code')}")
            print(f"错误信息: {error_data.get('message', '未知错误')}")
            return None
            
    except requests.exceptions.RequestException as e:
        print(f"❌ 网络请求异常: {e}")
        return None
    except json.JSONDecodeError as e:
        print(f"❌ JSON解析错误: {e}")
        return None

# 使用示例
if __name__ == "__main__":
    # 请替换为您的实际API密钥
    API_KEY = "your_api_key_here"
    
    # 要处理的文本
    text_to_process = "高村乡农村集体经济发展的重要因素是带头人的能力水平..."
    
    # 调用API (可以修改service_type参数)
    result = call_echopenai_api(
        api_key=API_KEY,
        text=text_to_process,
        service_type="jc"  # jc(降重), aigc(降AIGC), complex(复合)
    )