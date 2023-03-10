using Framework.Attributes;
using Framework.Model;

namespace DAL;

// 使用泛型緩存的方法，提前生成一個sql語句，對應每一個model
public class SqlHelper<T> where T : BaseModel
{
    private static string _findSql;
    public static string FindSql(Guid id)
    {
        return _findSql + id.ToString();
    }
    public static string FindAllSql;
    private static string _updateSql;
    public static string UpdateSql(Guid id)
    {
        return _updateSql + id.ToString();
    }

    static SqlHelper()
    {
        Type type = typeof(T);
        string selectedColumns = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]")); // GetColumnName是自己寫的Extension
        _findSql = $"SELECT {selectedColumns} FROM [{type.Name}] WHERE Id=";
        FindAllSql = $"SELECT {selectedColumns} FROM [{type.Name}]";

        var props = type.GetProperties().Where(p => !p.GetColumnName().Equals("Id"));
        // 當使用@符號定義一個參數時, 作為佔為符，我們可以在查詢語句中使用該參數的名稱，而不是直接插入值 => 避免sql注入
        string setString = string.Join(",", props.Select(p => $"{p.GetColumnName()}=@{p.GetColumnName()}"));
        string _updateSql = $"UPDATE {type.Name} SET {setString} WHERE Id=";
    }
}