using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
 
namespace _3rd.Json2Lua
{
    public class LuaTable
    {
        //存放table中的有序部分
        private List<LuaObject> list = new List<LuaObject>();
 
        //存放table中的无序部分
        private Dictionary<string, LuaObject> map = new Dictionary<string, LuaObject>();
 
        //所在层级
        private int layer = 0;
 
        public LuaTable(int _layer)
        {
            layer = _layer + 1;
        }
 
        #region 有序数组添加
 
        /// <summary>
        /// [有序]添加string value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(string value)
        {
            list.Add(new LuaObject(value));
        }
 
        /// <summary>
        /// [有序]num value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(int value)
        {
            list.Add(new LuaObject(value));
        }
 
        /// <summary>
        /// [有序]num value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(float value)
        {
            list.Add(new LuaObject(value));
        }
 
        /// <summary>
        /// [有序]bool value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(bool value)
        {
            list.Add(new LuaObject(value));
        }
 
        public void AddItem(LuaTable value)
        {
            list.Add(new LuaObject(value));
        }
 
        /// <summary>
        /// 加入nil
        /// </summary>
        public void AddItemNil()
        {
            list.Add(new LuaObject());
        }
 
        #endregion
 
 
        #region Key_Value添加
 
        public void AddItem(string key, string value)
        {
            //查重
            if (!map.ContainsKey(key))
            {
                map.Add(key, new LuaObject(value));
            }
        }
 
        public void AddItem(string key, int value)
        {
            //查重
            if (!map.ContainsKey(key))
            {
                map.Add(key, new LuaObject(value));
            }
        }
 
        public void AddItem(string key, float value)
        {
            //查重
            if (!map.ContainsKey(key))
            {
                map.Add(key, new LuaObject(value));
            }
        }
 
        public void AddItem(string key, bool value)
        {
            //查重
            if (!map.ContainsKey(key))
            {
                map.Add(key, new LuaObject(value));
            }
        }
 
        public void AddItem(string key, LuaTable value)
        {
            //查重
            if (!map.ContainsKey(key))
            {
                map.Add(key, new LuaObject(value));
            }
        }
 
        public void AddItemNil(string key)
        {
            //查重
            if (!map.ContainsKey(key))
            {
                map.Add(key, new LuaObject());
            }
        }
 
        #endregion
 
        /// <summary>
        /// 检查字符串是否为数字
        /// </summary>
        private bool IsNumeric(string str)
        {
            return int.TryParse(str, out _);
        }
 
        /// <summary>
        /// 添加缩进到StringBuilder
        /// </summary>
        private void AppendIndent(StringBuilder sb, int change = 0)
        {
            for (int i = 0; i < layer + change; i++)
            {
                sb.Append('\t'); // 使用制表符
            }
        }
 
        /// <summary>
        /// 将LuaTable转换为字符串
        /// </summary>
        public string GetString(bool indented = true)
        {
            var sb = new StringBuilder();
            sb.Append('{');
            
            bool hasList = list.Count > 0;
            bool hasMap = map.Count > 0;
            bool isNeedIndented = ShouldUseIndent(indented, hasList, hasMap);
            
            if (indented && (hasList || hasMap) && isNeedIndented)
            {
                sb.AppendLine();
            }
            
            if (hasList)
            {
                AppendListItems(sb, indented, isNeedIndented);
            }
 
            if (hasMap)
            {
                AppendMapItems(sb, indented);
            }
 
            if (!isNeedIndented)
            {
                sb.Append('}');
            }
            else
            {
                AppendIndent(sb, -1);
                sb.Append('}');
            }
 
            return sb.ToString();
        }

        /// <summary>
        /// 判断是否需要使用缩进格式化
        /// </summary>
        private bool ShouldUseIndent(bool indented, bool hasList, bool hasMap)
        {
            if (!indented || (!hasList && !hasMap))
                return false;
                
            // 总是使用缩进格式化
            return true;
        }

        /// <summary>
        /// 添加列表项到输出
        /// </summary>
        private void AppendListItems(StringBuilder sb, bool indented, bool isNeedIndented)
        {
            int count = 0;
            foreach (var item in list)
            {
                count++;
                if (!isNeedIndented)
                {
                    if (indented && count == 1)
                        sb.Append(' ');
                    sb.Append(item.GetString(indented));
                    if (count < list.Count)
                        sb.Append(',');
                    if (indented)
                        sb.Append(' ');
                }
                else
                {
                    AppendIndent(sb);
                    sb.Append(item.GetString(indented));
                    if (count < list.Count)
                        sb.Append(',');
                    sb.AppendLine();
                }
            }
        }

        /// <summary>
        /// 添加键值对到输出
        /// </summary>
        private void AppendMapItems(StringBuilder sb, bool indented)
        {
            int count = 0;
            foreach (var item in map)
            {
                count++;
                var key = FormatKey(item.Key);
                
                if (indented)
                {
                    AppendIndent(sb);
                    sb.Append($"{key} = {item.Value.GetString(indented)}");
                }
                else
                {
                    sb.Append($"{key}={item.Value.GetString(indented)}");
                }
                
                if (count < map.Count)
                    sb.Append(',');
                if (indented)
                    sb.AppendLine();
            }
        }

        /// <summary>
        /// 格式化键名（数字键加方括号）
        /// </summary>
        private string FormatKey(string key)
        {
            key = key.Replace('$', '_');
            return IsNumeric(key) ? $"[{key}]" : key;
        }
 
        /// <summary>
        /// 从JSON字符串创建LuaTable
        /// </summary>
        public static LuaTable CreateFromJson(string json)
        {
            var token = JToken.Parse(json);
            if (token.Type == JTokenType.Array)
            {
                return JsonArray2LuaTable(token.ToObject<JArray>(), 0);
            }
            else if (token.Type == JTokenType.Object)
            {
                return JsonObject2LuaTable(token.ToObject<JObject>(), 0);
            }
            else
            {
                throw new System.ArgumentException("JSON格式无效: 根元素必须是对象或数组", nameof(json));
            }
        }
 
        /// <summary>
        /// 将JSON对象转换为LuaTable
        /// </summary>
        static LuaTable JsonObject2LuaTable(JObject jsonObj, int layer)
        {
            var curLuaTable = new LuaTable(layer);
 
            //构建无序Table(lua表 key-value形式)
            foreach (var item in jsonObj)
            {
                ProcessJsonValue(curLuaTable, item.Key, item.Value, layer);
            }
 
            return curLuaTable;
        }
 
        /// <summary>
        /// 将JSON数组转换为LuaTable
        /// </summary>
        static LuaTable JsonArray2LuaTable(JArray jsonArray, int layer)
        {
            var curLuaTable = new LuaTable(layer);
            //构建有序Table(lua表)
            foreach (var item in jsonArray)
            {
                ProcessJsonValue(curLuaTable, null, item, layer);
            }
 
            return curLuaTable;
        }

        /// <summary>
        /// 处理JSON值类型转换
        /// </summary>
        private static void ProcessJsonValue(LuaTable table, string key, JToken value, int layer)
        {
            switch (value.Type)
            {
                case JTokenType.Boolean:
                    AddValue(table, key, (bool)value);
                    break;
                case JTokenType.Array:
                    AddValue(table, key, JsonArray2LuaTable(value.ToObject<JArray>(), layer + 1));
                    break;
                case JTokenType.String:
                    AddValue(table, key, (string)value);
                    break;
                case JTokenType.Date: //转成string
                    AddValue(table, key, (string)value);
                    break;
                case JTokenType.Float:
                    AddValue(table, key, (float)value);
                    break;
                case JTokenType.Integer:
                    AddValue(table, key, (int)value);
                    break;
                case JTokenType.None:
                    AddNilValue(table, key);
                    break;
                case JTokenType.Null:
                    AddNilValue(table, key);
                    break;
                case JTokenType.Object:
                    AddValue(table, key, JsonObject2LuaTable(value.ToObject<JObject>(), layer + 1));
                    break;
                case JTokenType.TimeSpan:
                    AddValue(table, key, (float)value);
                    break;
            }
        }

        /// <summary>
        /// 根据是否有key添加值到table
        /// </summary>
        private static void AddValue(LuaTable table, string key, object value)
        {
            if (key == null)
            {
                // 有序数组
                switch (value)
                {
                    case string s: table.AddItem(s); break;
                    case int i: table.AddItem(i); break;
                    case float f: table.AddItem(f); break;
                    case bool b: table.AddItem(b); break;
                    case LuaTable lt: table.AddItem(lt); break;
                }
            }
            else
            {
                // 无序键值对
                switch (value)
                {
                    case string s: table.AddItem(key, s); break;
                    case int i: table.AddItem(key, i); break;
                    case float f: table.AddItem(key, f); break;
                    case bool b: table.AddItem(key, b); break;
                    case LuaTable lt: table.AddItem(key, lt); break;
                }
            }
        }

        /// <summary>
        /// 添加nil值到table
        /// </summary>
        private static void AddNilValue(LuaTable table, string key)
        {
            if (key == null)
                table.AddItemNil();
            else
                table.AddItemNil(key);
        }
    }
}