using DIHA.Areas.Product.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIHA.Areas.Invoice.Models.Data
{
    [Table("ChiTietHoaDon")]
    public class ChiTietHoaDon
    {
        [Key]
        public int IDChiTietHoaDon { get; set; }
        public int IDHoaDon { get; set; }
        [ForeignKey("IDHoaDon")]
        public HoaDon? HoaDon { get; set; }
        public int IDSanPham { get; set; }
        [ForeignKey("IDSanPham")]
        public SanPham? SanPham { get; set; }
        public int SoLuongMua { set; get; }
    }
}
