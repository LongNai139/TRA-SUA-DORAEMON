using DIHA.Models.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIHA.Models.Product
{
    [Table("ProductCategoryProduct")]
    public class ProductCategoryProduct
    {
        public int ProductID { set; get; }

        public int CategoryID { set; get; }


        [ForeignKey("ProductID")]
        public ProductModel Product { set; get; }


        [ForeignKey("CategoryID")]
        public CategoryProduct Category { set; get; }
    }
}