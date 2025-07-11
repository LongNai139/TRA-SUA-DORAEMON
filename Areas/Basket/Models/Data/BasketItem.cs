using DIHA.Areas.Product.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;
namespace DIHA.Areas.Basket.Models.Data
{
    [NotMapped]

    public class BasketItem : SanPham
    {
       public int SoLuong { get; set; } = 1;
    }
}
