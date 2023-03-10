using Microsoft.Extensions.Configuration; // 引用Microsoft.Extensions.Configuration.Json
namespace Utilities;

public class AppSettings
{
    public string ConnectionString { get; set; }
    public string DALDllName { get; set; }
    public string DALTypeName { get; set; }
}

public class Configuration
{
    public static AppSettings AppSettings { get; private set; }

    static Configuration()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath("../API").AddJsonFile("appsettings.json");

        var configuration = configurationBuilder.Build();
        AppSettings.ConnectionString = configuration["ConnectionString"];
        AppSettings.DALDllName = configuration.GetSection("DALSetting")["DALNameDll"];
        AppSettings.DALTypeName = configuration.GetSection("DALSetting")["DALTypeName"];
    }
}