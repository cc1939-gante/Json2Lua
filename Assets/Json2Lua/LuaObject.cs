using System;
using System.Collections.Generic;
using System.Text;
 
namespace _3rd.Json2Lua
{
    /// <summary>
    /// Lua对象，表示Lua中的各种数据类型
    /// </summary>
    public class LuaObject
    {
        private readonly LuaValueType type;
        private readonly float valueNumber;
        private readonly string valueString;
        private readonly bool valueBoolean;
        private readonly LuaTable valueTable;
 
        /// <summary>
        /// 创建字符串类型的Lua对象
        /// </summary>
        public LuaObject(string value)
        {
            type = LuaValueType.String;
            valueString = value;
        }
 
        /// <summary>
        /// 创建数字类型的Lua对象
        /// </summary>
        public LuaObject(float value)
        {
            type = LuaValueType.Num;
            valueNumber = value;
        }
 
        /// <summary>
        /// 创建整数类型的Lua对象
        /// </summary>
        public LuaObject(int value)
        {
            type = LuaValueType.Num;
            valueNumber = value;
        }
 
        /// <summary>
        /// 创建布尔类型的Lua对象
        /// </summary>
        public LuaObject(bool value)
        {
            type = LuaValueType.Boolean;
            valueBoolean = value;
        }
 
        /// <summary>
        /// 创建表类型的Lua对象
        /// </summary>
        public LuaObject(LuaTable value)
        {
            type = LuaValueType.Table;
            valueTable = value;
        }
 
        /// <summary>
        /// 创建nil类型的Lua对象
        /// </summary>
        public LuaObject()
        {
            type = LuaValueType.nil;
        }
 
        /// <summary>
        /// 将Lua对象转换为字符串表示
        /// </summary>
        public string GetString(bool indented = true)
        {
            return type switch
            {
                LuaValueType.Table => valueTable.GetString(indented),
                LuaValueType.Boolean => valueBoolean ? "true" : "false",
                LuaValueType.Num => valueNumber.ToString(),
                LuaValueType.String => $"\"{valueString}\"",
                LuaValueType.nil => "nil",
                _ => string.Empty
            };
        }
 
        /// <summary>
        /// 获取Lua对象的类型
        /// </summary>
        public LuaValueType GetLuaValueType()
        {
            return type;
        }
    }
}