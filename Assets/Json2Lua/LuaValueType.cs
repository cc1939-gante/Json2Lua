using System;
using System.Collections.Generic;
using System.Text;
 
namespace _3rd.Json2Lua
{
    /// <summary>
    /// Lua值类型枚举
    /// 定义了Lua中支持的各种数据类型
    /// </summary>
    public enum LuaValueType
    {
        /// <summary>
        /// 表类型 - 对应Lua中的table
        /// </summary>
        Table,
        
        /// <summary>
        /// 字符串类型 - 对应Lua中的string
        /// </summary>
        String,
        
        /// <summary>
        /// 数字类型 - 对应Lua中的number（包括整数和浮点数）
        /// </summary>
        Num,
        
        /// <summary>
        /// 布尔类型 - 对应Lua中的boolean
        /// </summary>
        Boolean,
        
        /// <summary>
        /// 空值类型 - 对应Lua中的nil
        /// </summary>
        nil
    }
}