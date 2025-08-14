# 🌐 EchopenAI 服务简介

> **EchopenAI** 是基于学术大模型打造的智能文本优化服务，专注于帮助研究者有效降低论文中的人工智能生成特征（AIGC）和文字重复率，提升学术合规性与原创性。**本服务非提示词工程**。

---

## 🚀 快速开始

### ✅ 在线服务（适合个人用户）
无需编程，注册即用，操作简单。

🔗 访问官网：[https://echopen.cn?source=github](https://echopen.cn?source=github)

- 支持网页端直接处理
- 适合小批量、非自动化场景使用

---

### 🔌 接口服务（适合开发者/机构）
集成至您的系统或写作工具，实现自动化批处理。

🔗 接口平台：[https://llmapi.fit?source=github](https://llmapi.fit?source=github)

- 提供标准 RESTful API
- 支持论文降重、降AIGC、复合优化三大功能
- 响应迅速，支持高并发调用

---

# 📚 学术大模型 API 接口文档（V 1.30）

> 本接口提供论文**降重**、**降AIGC检测**与**复合优化**服务，助力学术写作合规化。支持 HTTPS 协议，返回标准 JSON 格式数据。

---

## 🔗 接口地址（仅支持 HTTPS）

```
https://api.llmapi.fit/completion/reduce
```

> ⚠️ **注意**：当前接口仅支持文本处理。如需上传并保留 `.docx` 或 `.doc` 文档格式，请联系工作人员定制服务。

---

## 🚀 新版本特性预告

- ✅ 进一步提升文本改写自然度与语义一致性  
- ✅ 兼容 **格子达平台** 的 AIGC 检测标准，增强通过率

---

## 📎 联系方式

如有以下需求，欢迎联系：

- 文档格式保持（支持 `.docx` / `.doc`）
- 流式响应支持

> 微信：`GDDMDD`（请备注：定制接口咨询）

---

## 🔐 认证方式

所有请求必须在 HTTP Header 中携带认证信息：

```http
Authorization: Bearer <你的接口密钥>
```

🔑 **获取密钥**：登录后前往「个人中心」设置 API 密钥。

---

## 🛠️ 接口说明

通过 `service_type` 参数选择服务类型：

| 类型 | 值 | 说明 |
|------|-----|------|
| `aigc` | 降AIGC | 降低人工智能生成内容特征 |
| `jc` | 降重 | 内容去重、语义重构 |
| `complex` | 复合服务 | 同时执行降重 + 降AIGC |

---

## 💡 cURL 示例

```bash
curl -X POST \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <接口密钥>" \
  -d '{
    "text": "高村乡农村集体经济发展的重要因素是带头人的能力水平...",
    "service_type": "jc"
  }' \
  "https://api.llmapi.fit/completion/reduce"
```

---

## 📥 请求参数

| 参数名 | 类型 | 必填 | 说明 |
|--------|------|------|------|
| `text` | string | ✅ | 需要处理的原始文本内容 |
| `service_type` | string | ✅ | 服务类型：`aigc`, `jc`, `complex` |

---

## 📤 响应示例

```json
{
  "code": "success",
  "message": "",
  "request_id": "0009837a1ee1408d9550864e8c357fc4",
  "output_text": "高村乡农村集体经济发展重要的是带头人能力强...",
  "input_text_count": 169,
  "service_type": "jc"
}
```

---

## 📊 响应字段说明

| 字段名 | 类型 | 说明 |
|--------|------|------|
| `code` | string | 业务状态码，成功为 `success`，错误详见下方表格 |
| `message` | string | 错误信息（成功时为空） |
| `request_id` | string | 请求唯一标识，便于排查问题 |
| `output_text` | string | 处理后的文本结果 |
| `input_text_count` | number | 输入文本字数（按此扣除额度） |
| `service_type` | string | 实际调用的服务类型 |

> 📌 **复合模式字数规则**：`input_text_count * 1.5`，不足 100 字按 150 字扣除。

---

## ⚠️ 重要说明

### 📝 文本内容要求
- `text` 参数中**仅包含需改写的正文**，不得包含指令、提示或任务描述，多余的指令会被拒绝，甚至会导致无法达到改写效果。
- ❌ 不建议改写代码、数学公式等结构化内容 —— 效果有限且浪费字数。

### 📏 输入长度建议
- 单次请求建议 **不超过 1000 字**。
- 内容越长，改写复杂度越高，效果可能下降。

### 💰 字数扣除规则

| 模式 | 扣除规则 |
|------|----------|
| 降重 / 降AIGC | 按实际字数扣除，不足 100 字按 100 字计 |
| 复合模式 | 按 `1.5 × 实际字数` 扣除，不足 100 字按 150 字计 |

> 📏 字数统计尽量贴近 MS Office / WPS 规则。若发现差异，请联系工作人员。

---

### 🎯 改写效果说明

- ✅ 无法保证每次请求都`完全消除AIGC 特征`或实现理想降重效果。
- 示例：原文 AIGC 检测为 10%，表明部分段落仍可能存在明显 AI 痕迹。
- 建议对关键段落多次尝试或人工润色。

> ⚠️ 若对此敏感，请谨慎购买服务。

---

### 🧩 格式与输出说明

| 项目 | 说明 |
|------|------|
| **段落数量** | 尽量保留输入结构，但不绝对保证 |
| **输出字数** | 不保证与输入一致（由大模型生成特性决定） |
| **生成参数** | `temperature`, `top_p` 等参数**不开放**，以确保稳定性 |
| **流式响应** | 当前**不支持**流式输出。如有需求，请联系工作人员评估 |

---

### 🔄 错误与退款机制

- 若请求失败且发生字数扣除，系统将**自动返还字数至账户**，无需人工干预。
- 不另行通知，请关注账户余额变动。

---

## 🛑 HTTP 响应码说明

| 状态码 | 错误码 | 说明 | 示例响应 |
|--------|--------|------|-----------|
| `400` | `useageLimitedFailed` | 可用字数不足 | `{ "code": "useageLimitedFailed", "message": "可用字数不足" }` |
| `400` | `dataInspectionFailed` | 文本触发敏感词（绿网规则） | `{ "code": "dataInspectionFailed", "message": "文本触发了绿网敏感词规则" }` |
| `401` | `unauthorized` | 无效的接口密钥 | `{ "code": "unauthorized", "message": "无效的接口密钥" }` |
| `403` | - | 缺少有效 Authorization 结构 | `{ "detail": "缺少有效的Authorization结构. 请使用'Bearer API_KEY'" }` |
| `429` | `llmLimited` | 触发频率限制，请稍后重试 | `{ "code": "llmLimited", "message": "触发限制，稍后重试" }` |
| `500` | `internalFailed` | 服务器内部错误 | `{ "code": "internalFailed", "message": "internal error" }` |
| `500` | `llmInternalFailed` | 学术模型内部异常 | `{ "code": "llmInternalFailed", "message": "学术模型内部异常" }` |

---

## 🙋 常见问题（FAQ）

❓ **Q：能否处理整篇论文？**  
A：建议分段处理（如每段 ≤1000 字），避免长文本导致效果下降。

❓ **Q：如何提高降AIGC效果？**  
A：使用 `aigc` 模式处理即可达到理想的状态。

❓ **Q：是否支持 PDF/DOCX 文件上传？**  
A：目前接口仅支持文本。如需文件级处理，请联系微信 `GDDMDD` 定制。

---

> © 2025 回声笔AI · 学术大模型服务平台 | 技术支持：[https://llmapi.fit?source=github](https://llmapi.fit?source=github) | 微信联系：GDDMDD
