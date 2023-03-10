using Framework.Attributes;
using Framework.Model;

namespace Model;

public class User : BaseModel
{
    [Leng(2, 8)]
    public string Name { get; set; }
    public string Account { get; set; }
    public string Password { get; set; }
    [Email]
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string CompanyName { get; set; }
    [Column("State")]
    public int Status { get; set; }
    public int UserType { get; set; }
    public DateTime LastLoginTime { get; set; }
    public int CreateId { get; set; }

    public DateTime CreateTime { get; set; }

    public int LastModifierId { get; set; }

    public DateTime LastModifyTime { get; set; }
}