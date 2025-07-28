using System;
using System.Collections.Generic;
using UnityEngine;
using JsonUtility = _3rd.Json2Lua.JsonUtility;

/// <summary>
/// Json2Lua 转换测试组件
/// 挂载到GameObject上运行测试用例
/// </summary>
public class Json2LuaTest : MonoBehaviour
{
    [Header("测试配置")]
    [SerializeField] private bool runOnStart = true;
    [SerializeField] private bool enablePerformanceTest = true;
    [SerializeField] private int performanceTestCount = 1000;
    
    [Header("测试结果")]
    [SerializeField] private string lastTestResult = "";
    [SerializeField] private float lastTestTime = 0f;
    
    private readonly List<TestCase> testCases = new List<TestCase>();
    
    [System.Serializable]
    public class TestCase
    {
        public string name;
        public string jsonInput;
        public string expectedOutput;
        public bool shouldPass = true;
    }
    
    void Start()
    {
        if (runOnStart)
        {
            RunAllTests();
        }
    }
    
    /// <summary>
    /// 运行所有测试用例
    /// </summary>
    [ContextMenu("运行所有测试")]
    public void RunAllTests()
    {
        Debug.Log("=== Json2Lua 测试开始 ===");
        
        InitializeTestCases();
        
        int passedTests = 0;
        int totalTests = testCases.Count;
        float startTime = Time.realtimeSinceStartup;
        
        foreach (var testCase in testCases)
        {
            if (RunSingleTest(testCase))
            {
                passedTests++;
            }
        }
        
        lastTestTime = Time.realtimeSinceStartup - startTime;
        
        Debug.Log($"=== 测试完成 ===");
        Debug.Log($"总测试数: {totalTests}");
        Debug.Log($"通过测试: {passedTests}");
        Debug.Log($"失败测试: {totalTests - passedTests}");
        Debug.Log($"测试耗时: {lastTestTime:F3}秒");
        
        if (enablePerformanceTest)
        {
            RunPerformanceTest();
        }
    }
    
    /// <summary>
    /// 运行单个测试用例
    /// </summary>
    private bool RunSingleTest(TestCase testCase)
    {
        try
        {
            Debug.Log($"\n--- 测试: {testCase.name} ---");
            Debug.Log($"输入JSON: {testCase.jsonInput}");
            
            string result = JsonUtility.Json2Lua(testCase.jsonInput);
            
            Debug.Log($"输出Lua: {result}");
            
            if (!string.IsNullOrEmpty(testCase.expectedOutput))
            {
                // 标准化字符串再比较
                string normalizedResult = NormalizeString(result);
                string normalizedExpected = NormalizeString(testCase.expectedOutput);
                bool isCorrect = normalizedResult == normalizedExpected;
                if (isCorrect)
                {
                    Debug.Log($"[PASS] 测试通过: {testCase.name}");
                }
                else
                {
                    Debug.LogError($"[FAIL] 测试失败: {testCase.name}");
                    Debug.LogError($"期望输出: {testCase.expectedOutput}");
                    Debug.LogError($"实际输出: {result}");
                    Debug.LogError($"标准化期望: {normalizedExpected}");
                    Debug.LogError($"标准化实际: {normalizedResult}");
                }
                return isCorrect;
            }
            else
            {
                Debug.Log($"[PASS] 测试完成: {testCase.name}");
                return true;
            }
        }
        catch (Exception ex)
        {
            if (testCase.shouldPass)
            {
                Debug.LogError($"[FAIL] 测试异常: {testCase.name} - {ex.Message}");
                return false;
            }
            else
            {
                Debug.Log($"[PASS] 预期异常: {testCase.name} - {ex.Message}");
                return true;
            }
        }
    }

    // 新增：字符串标准化方法
    private string NormalizeString(string str)
    {
        return str.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
    }
    
    /// <summary>
    /// 初始化测试用例
    /// </summary>
    private void InitializeTestCases()
    {
        testCases.Clear();
        
        // 测试用例1: 简单对象
        testCases.Add(new TestCase
        {
            name = "简单对象",
            jsonInput = "{\"name\":\"张三\",\"age\":25}",
            expectedOutput = "return {\n\tname = \"张三\",\n\tage = 25\n}"
        });
        
        // 测试用例2: 数组
        testCases.Add(new TestCase
        {
            name = "简单数组",
            jsonInput = "[\"apple\",\"banana\",\"orange\"]",
            expectedOutput = "return {\n\t\"apple\",\n\t\"banana\",\n\t\"orange\"\n}"
        });
        
        // 测试用例3: 嵌套对象
        testCases.Add(new TestCase
        {
            name = "嵌套对象",
            jsonInput = "{\"person\":{\"name\":\"李四\",\"skills\":[\"C#\",\"Unity\"]}}",
            expectedOutput = "return {\n\tperson = {\n\t\tname = \"李四\",\n\t\tskills = {\n\t\t\t\"C#\",\n\t\t\t\"Unity\"\n\t\t}\n\t}\n}"
        });
        
        // 测试用例4: 混合类型
        testCases.Add(new TestCase
        {
            name = "混合类型",
            jsonInput = "{\"string\":\"test\",\"number\":123.45,\"boolean\":true,\"null\":null,\"array\":[1,2,3]}",
            expectedOutput = "return {\n\tstring = \"test\",\n\tnumber = 123.45,\n\tboolean = true,\n\tnull = nil,\n\tarray = {\n\t\t1,\n\t\t2,\n\t\t3\n\t}\n}"
        });
        
        // 测试用例5: 数字键
        testCases.Add(new TestCase
        {
            name = "数字键",
            jsonInput = "{\"1\":\"one\",\"2\":\"two\",\"10\":\"ten\"}",
            expectedOutput = "return {\n\t[1] = \"one\",\n\t[2] = \"two\",\n\t[10] = \"ten\"\n}"
        });
        
        // 测试用例6: 空对象
        testCases.Add(new TestCase
        {
            name = "空对象",
            jsonInput = "{}",
            expectedOutput = "return {}"
        });
        
        // 测试用例7: 空数组
        testCases.Add(new TestCase
        {
            name = "空数组",
            jsonInput = "[]",
            expectedOutput = "return {}"
        });
        
        // 测试用例8: 特殊字符
        testCases.Add(new TestCase
        {
            name = "特殊字符",
            jsonInput = "{\"$special\":\"value\",\"normal\":\"test\"}",
            expectedOutput = "return {\n\t_special = \"value\",\n\tnormal = \"test\"\n}"
        });
        
        // 测试用例9: 错误JSON
        testCases.Add(new TestCase
        {
            name = "错误JSON格式",
            jsonInput = "{invalid json}",
            shouldPass = false
        });
        
        // 测试用例10: 空字符串
        testCases.Add(new TestCase
        {
            name = "空字符串",
            jsonInput = "",
            expectedOutput = "return {}"
        });
    }
    
    /// <summary>
    /// 运行性能测试
    /// </summary>
    [ContextMenu("运行性能测试")]
    public void RunPerformanceTest()
    {
        Debug.Log("\n=== 性能测试开始 ===");
        
        string testJson = "{\"name\":\"性能测试\",\"data\":[1,2,3,4,5],\"nested\":{\"key\":\"value\"}}";
        
        // 预热
        for (int i = 0; i < 10; i++)
        {
            JsonUtility.Json2Lua(testJson);
        }
        
        // 性能测试
        float startTime = Time.realtimeSinceStartup;
        
        for (int i = 0; i < performanceTestCount; i++)
        {
            JsonUtility.Json2Lua(testJson);
        }
        
        float endTime = Time.realtimeSinceStartup;
        float totalTime = endTime - startTime;
        float avgTime = totalTime / performanceTestCount * 1000; // 转换为毫秒
        
        Debug.Log($"性能测试结果:");
        Debug.Log($"总转换次数: {performanceTestCount}");
        Debug.Log($"总耗时: {totalTime:F3}秒");
        Debug.Log($"平均耗时: {avgTime:F3}毫秒");
        Debug.Log($"每秒转换: {performanceTestCount / totalTime:F0}次");
        
        lastTestResult = $"性能测试: {performanceTestCount}次转换, 平均{avgTime:F3}ms";
    }
    
    /// <summary>
    /// 测试紧凑格式
    /// </summary>
    [ContextMenu("测试紧凑格式")]
    public void TestCompactFormat()
    {
        Debug.Log("\n=== 紧凑格式测试 ===");
        
        string json = "{\"name\":\"紧凑测试\",\"items\":[1,2,3]}";
        
        string formatted = JsonUtility.Json2Lua(json, true);
        string compact = JsonUtility.Json2LuaCompact(json);
        
        Debug.Log("格式化输出:");
        Debug.Log(formatted);
        
        Debug.Log("紧凑格式输出:");
        Debug.Log(compact);
    }
    
    /// <summary>
    /// 测试复杂JSON
    /// </summary>
    [ContextMenu("测试复杂JSON")]
    public void TestComplexJson()
    {
        Debug.Log("\n=== 复杂JSON测试 ===");
        
        string complexJson = @"{
            ""gameConfig"": {
                ""version"": ""1.0.0"",
                ""players"": [
                    {""id"": 1, ""name"": ""玩家1"", ""level"": 10},
                    {""id"": 2, ""name"": ""玩家2"", ""level"": 15}
                ],
                ""settings"": {
                    ""sound"": true,
                    ""music"": false,
                    ""volume"": 0.8
                }
            }
        }";
        
        try
        {
            string result = JsonUtility.Json2Lua(complexJson);
            Debug.Log("复杂JSON转换结果:");
            Debug.Log(result);
        }
        catch (Exception ex)
        {
            Debug.LogError($"复杂JSON转换失败: {ex.Message}");
        }
    }
    
    /// <summary>
    /// 清空测试结果
    /// </summary>
    [ContextMenu("清空测试结果")]
    public void ClearTestResults()
    {
        lastTestResult = "";
        lastTestTime = 0f;
        Debug.Log("测试结果已清空");
    }
} 