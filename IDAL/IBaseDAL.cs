using Framework.Model;

namespace IDAL;

public interface IBaseDAL
{
    T Find<T>(Guid id) where T : BaseModel;
    List<T> FindAll<T>() where T : BaseModel;
    void Update<T>(T t) where T : BaseModel;
    void Insert<T>(T t) where T : BaseModel;
    void Delete<T>(T t) where T : BaseModel;
}