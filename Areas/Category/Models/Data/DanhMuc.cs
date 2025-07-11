using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIHA.Areas.Category.Models.Data
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        public int IDDanhMuc { get; set; }
        public string? TenDanhMuc {  set; get; }
        public bool? DaXoa { get; set; }
    }
}
