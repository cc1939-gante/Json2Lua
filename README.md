# Json2Lua 项目

## 项目简介

Json2Lua 是一个用于将 JSON 格式字符串转换为 Lua 表格式字符串的工具库。该库提供了高性能、易用的 JSON 到 Lua 转换功能，支持格式化输出和错误处理。

## 主要功能

- **JSON 到 Lua 转换**：将 JSON 字符串转换为 Lua 表格式
- **格式化支持**：支持缩进格式化和紧凑格式输出
- **类型安全**：完整的 Lua 数据类型支持（table、string、number、boolean、nil）
- **错误处理**：提供详细的错误信息和异常处理
- **高性能**：使用 StringBuilder 优化字符串操作

## 核心组件

### 1. JsonUtility 类
主要的转换工具类，提供简单的 API 接口。

```csharp
// 格式化转换
string luaCode = JsonUtility.Json2Lua(jsonString);

// 紧凑格式转换
string compactLuaCode = JsonUtility.Json2LuaCompact(jsonString);
```

### 2. LuaTable 类
表示 Lua 表的内部数据结构，支持有序数组和无序键值对。

### 3. LuaObject 类
表示 Lua 中的各种数据类型，包括字符串、数字、布尔值、表和空值。

### 4. LuaValueType 枚举
定义 Lua 中支持的数据类型。

## 使用示例

```csharp
// 基本使用
string json = "{\"name\":\"张三\",\"age\":25,\"skills\":[\"C#\",\"Unity\"]}";
string luaCode = JsonUtility.Json2Lua(json);

// 输出结果：
// return {
//     name = "张三",
//     age = 25,
//     skills = {
//         "C#",
//         "Unity"
//     }
// }
```

## 性能优化

### 1. StringBuilder 优化
- 使用 StringBuilder 替代字符串拼接，大幅提升性能
- 减少内存分配和垃圾回收压力

### 2. 方法拆分
- 将复杂方法拆分为多个小方法，提高可读性和可维护性
- 每个方法职责单一，便于测试和调试

### 3. 代码现代化
- 使用 C# 8.0+ 的 switch expression
- 使用 readonly 字段确保不可变性
- 使用字符串插值提升代码可读性

## 错误处理

库提供了完善的错误处理机制：

```csharp
try
{
    string luaCode = JsonUtility.Json2Lua(invalidJson);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"转换失败: {ex.Message}");
}
```

## 项目结构

```
Json2Lua/
├── JsonUtility.cs      # 主要转换工具类
├── LuaTable.cs         # Lua表数据结构
├── LuaObject.cs        # Lua对象数据类型
├── LuaValueType.cs     # Lua值类型枚举
└── README.md          # 项目文档
```

## 版本历史

### v2.0 (当前版本)
- ✅ 使用 StringBuilder 优化字符串操作
- ✅ 添加完整的 XML 文档注释
- ✅ 优化方法结构，提高可维护性
- ✅ 改进错误处理机制
- ✅ 使用现代 C# 语法

### v1.0 (原始版本)
- 基础 JSON 到 Lua 转换功能
- 支持基本数据类型
- 简单的格式化输出

## 贡献指南

欢迎提交 Issue 和 Pull Request 来改进这个项目。

## 许可证

本项目采用 MIT 许可证。 

## 感谢
来源： https://blog.csdn.net/qq_39574690/article/details/144568391
