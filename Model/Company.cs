using Framework.Attributes;
using Framework.Model;

namespace Model;

public class Company : BaseModel
{
    [Column("CmpName")] // 數據庫中是CmpName
    public string Name { get; set; }
    public int CreatorId { get; set; }
    // 可空類型要特別注意DataTime, int 這種值類型(Struct)
    public DateTime? CreateTime { get; set; }
    public int? LastModifierId { get; set; }
    public DateTime? LastModifyTime { get; set; }
}