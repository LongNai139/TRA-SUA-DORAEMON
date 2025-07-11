using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DIHA.Areas.Category.Models.Data;

namespace DIHA.Areas.Product.Models.Data
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public int IDSanPham { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string? TenSanpham { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(1000, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 1000")]
        [Column(TypeName = "money")]
        public decimal? DonGia { get; set; }

        public bool? DaXoa { get; set; }
        public string? Anh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int IDDanhMuc { get; set; }

        [ForeignKey("IDDanhMuc")]
        public DanhMuc? DanhMuc { get; set; }
    }

}
