using DIHA.Areas.Product.Models.Data;
using DIHA.Areas.Custom.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIHA.Areas.Invoice.Models.Data
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public int IDHoaDon { get; set; }

        public string IdCustom { get; set; }

        [ForeignKey("IdCustom")]
        public DIHA.Areas.Custom.Models.Data.Custom? Custom { get; set; }
        [StringLength(256)]
        public string? TenKhach { get; set; }
        [StringLength(256)]
        public string? NguoiDuyet { get; set; }

        public int DaMua { get; set; }

        public ICollection<ChiTietHoaDon>? ChiTietHoaDons { get; set; }

        public string? PhuongThucThanhToan { get; set; }

        public DateTime NgayMua { get; set; }

        public string? DiaChiGiaoHang { get; set; }

        public string? SoDienThoaiNhanHang { get; set; }

        public decimal? TongTien { get; set; }

        public string TrangThai { get; set; } = "Chờ xử lý";
    }

}
