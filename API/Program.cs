using Factory;
using IDAL;
using Model;

try
{
    // 我們並不希望直接依賴BaseDAL, 所以我們建立工廠，因為我們應該依賴接口
    IBaseDAL baseDAL = DALFactory.CreateInstance();
    Company company = baseDAL.Find<Company>(new Guid());
    List<Company> companies = baseDAL.FindAll<Company>();

    company.Name = "Google";
    baseDAL.Update(company);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}