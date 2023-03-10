using System.Reflection;
using IDAL;
using Utilities;

namespace Factory;

public class DALFactory
{
    private static Type DALType;
    static DALFactory()
    {
        try
        {
            Assembly assembly = Assembly.Load(Configuration.AppSettings.DALDllName);
            DALType = assembly.GetType(Configuration.AppSettings.DALTypeName);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }

    }
    public static IBaseDAL CreateInstance()
    {
        return Activator.CreateInstance(DALType) as IBaseDAL;
    }
}