using Framework.Attributes;
using Framework.Model;
using IDAL;
using Microsoft.Data.SqlClient;
using Utilities;
namespace DAL;

public class BaseDAL : IBaseDAL
{
    /// <summary>
    /// The generic constraint means this DAL only can use the class 
    ///  that Inherit from BaseModel which has Id attribute with type 
    /// Guid
    /// </summary>
    public T Find<T>(Guid id) where T : BaseModel
    {
        // Type type = typeof(T);
        // // Obtain the Model column name
        // // []是因為避免SQL Server的關鍵字問題：這屬於SQL Server的語法
        // string selectedColumns = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]")); // GetColumnName是自己寫的Extension
        // string sql = $"SELECT {0} FROM [{type.Name}] WHERE Id={id}";
        string sql = SqlHelper<T>.FindSql(id);
        T result;

        // using var conn = new SqlConnection(Configuration.AppSettings.ConnectionString);
        // using var command = new SqlCommand(sql, conn);
        // conn.Open();
        // SqlDataReader reader = command.ExecuteReader();
        // if (reader.Read()) // 表示有數據
        // {
        //     result = MapReaderToModelList<T>(reader).FirstOrDefault();
        // }
        result = ExcuteSql(sql, (command) =>
        {
            SqlDataReader reader = command.ExecuteReader();
            List<T> resultList = MapReaderToModelList<T>(reader);
            result = resultList.FirstOrDefault();
            return result;
        });
        return result;
    }

    public List<T> FindAll<T>() where T : BaseModel
    {
        // Type type = typeof(T);
        // string selectedColumns = string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"));
        // string sql = $"SELECT {0} FROM [{type.Name}]";
        string sql = SqlHelper<T>.FindAllSql;
        List<T> result;
        // using var conn = new SqlConnection(Configuration.AppSettings.ConnectionString);
        // using var command = new SqlCommand(sql, conn);
        // conn.Open();
        // SqlDataReader reader = command.ExecuteReader();
        // List<T> result = MapReaderToModelList<T>(reader);
        // return result;

        result = ExcuteSql(sql, (command) =>
        {
            SqlDataReader reader = command.ExecuteReader();
            List<T> result = MapReaderToModelList<T>(reader);
            return result;
        });
        return result;
    }

    private static List<T> MapReaderToModelList<T>(SqlDataReader reader) where T : BaseModel
    {
        Type type = typeof(T);
        List<T> modelList = new List<T>();
        while (reader.Read())
        {
            T result = Activator.CreateInstance(type) as T;
            foreach (var prop in type.GetProperties())
            {
                object val = reader[prop.GetColumnName()];
                // 要特別注意是否是DBNull
                prop.SetValue(result, val is DBNull ? null : val);
            }
        }
        return modelList;
    }

    public void Update<T>(T t) where T : BaseModel
    {
        t.Validate(); // validate the model data correctness.

        Type type = typeof(T);
        var props = type.GetProperties().Where(p => !p.GetColumnName().Equals("Id"));
        // // 當使用@符號定義一個參數時, 作為佔為符，我們可以在查詢語句中使用該參數的名稱，而不是直接插入值 => 避免sql注入
        // string setString = string.Join(",", props.Select(p => $"{p.GetColumnName()}=@{p.GetColumnName()}"));
        // string sql = $"UPDATE {type.Name} SET {setString} WHERE Id={t.Id}";
        string sql = SqlHelper<T>.UpdateSql(t.Id);
        // using var conn = new SqlConnection(Configuration.AppSettings.ConnectionString);
        // using var command = new SqlCommand(sql, conn);
        // foreach (var prop in props)
        // {
        //     if (prop.Name == "Id") { continue; }
        //     command.Parameters.AddWithValue($"@{prop.GetColumnName()}", prop.GetValue(t) ?? DBNull.Value);
        // }
        // conn.Open();
        // int affetedRow = command.ExecuteNonQuery();
        // if (affetedRow == 0) { throw new Exception("Update data don't exit."); }
        int affectedRow = ExcuteSql(sql, (command) =>
        {
            foreach (var prop in props)
            {
                command.Parameters.AddWithValue($@"{prop.GetColumnName()}", prop.GetValue(t) ?? DBNull.Value);
            }
            int affectedRow = command.ExecuteNonQuery();
            return affectedRow;
        });
        if (affectedRow == 0) { throw new Exception("Update data don't exit."); }
    }

    public void Insert<T>(T t) where T : BaseModel
    {
        throw new NotImplementedException();
    }

    public void Delete<T>(T t) where T : BaseModel
    {
        throw new NotImplementedException();
    }

    // 為了要解決重複代碼，我們可以將空同的東西封裝成一個方法，然後可變得使用事件暴露
    private T ExcuteSql<T>(string sql, Func<SqlCommand, T> func)
    {
        using var conn = new SqlConnection(Configuration.AppSettings.ConnectionString);
        conn.Open(); // 開啟連接
        var sqlTranscation = conn.BeginTransaction(); // 開啟事物
        var command = conn.CreateCommand();
        try
        {
            command.CommandText = sql;
            T result = func.Invoke(command);
            sqlTranscation.Commit(); // 提交事物
            return result;
        }
        catch (Exception ex)
        {
            sqlTranscation.Rollback();
            throw;
        }
    }
}
