namespace _3rd.Json2Lua
{
    /// <summary>
    /// JSON到Lua转换工具类
    /// 提供JSON字符串到Lua表格式字符串的转换功能
    /// </summary>
    public class JsonUtility
    {
        /// <summary>
        /// 将JSON格式字符串转换为Lua表格式字符串
        /// </summary>
        /// <param name="json">输入的JSON字符串</param>
        /// <param name="indented">是否使用缩进格式化，默认为true</param>
        /// <returns>Lua表格式的字符串，以"return"开头</returns>
        /// <exception cref="System.ArgumentException">当JSON格式无效时抛出</exception>
        public static string Json2Lua(string json, bool indented = true)
        {
            if (string.IsNullOrEmpty(json))
            {
                return "return {}";
            }

            try
            {
                var table = LuaTable.CreateFromJson(json);
                return $"return {table.GetString(indented)}";
            }
            catch (System.Exception ex)
            {
                throw new System.ArgumentException($"JSON格式无效: {ex.Message}", nameof(json), ex);
            }
        }

        /// <summary>
        /// 将JSON格式字符串转换为Lua表格式字符串（紧凑格式）
        /// </summary>
        /// <param name="json">输入的JSON字符串</param>
        /// <returns>紧凑格式的Lua表字符串</returns>
        public static string Json2LuaCompact(string json)
        {
            return Json2Lua(json, false);
        }
    }
}