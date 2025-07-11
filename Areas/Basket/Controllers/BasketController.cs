using DIHA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;
using DIHA.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using DIHA.Models.Sell;
using DIHA.Areas.Category.Models.Data;
using DIHA.Areas.Product.Models.Data;
using DIHA.Areas.Basket.Models.Data;
using System.Text.Json;

namespace DIHA.Areas.Basket.Controllers
{
    [Area("Basket")]
    [Route("basket_Controller/[action]")]
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public BasketController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult AddToCart(int id, string ten, decimal gia, string anh, int idDanhMuc)
        {
            var cart = GetCartFromSession();

            var existing = cart.FirstOrDefault(c => c.IDSanPham == id);
            if (existing != null)
            {
                existing.SoLuong++;
            }
            else
            {
                cart.Add(new BasketItem
                {
                    IDSanPham = id,
                    TenSanpham = ten,
                    DonGia = gia,
                    Anh = anh,
                    IDDanhMuc = idDanhMuc,
                    SoLuong = 1
                });
            }

            SaveCartToSession(cart);

            return Json(new
            {
                success = true,
                count = cart.Sum(x => x.SoLuong),
                items = cart,
                message = "Đã thêm vào giỏ hàng!"
            });
        }
        private List<BasketItem> GetCartFromSession()
        {
            var data = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(data)
                ? new List<BasketItem>()
                : JsonSerializer.Deserialize<List<BasketItem>>(data)!;
        }

        private void SaveCartToSession(List<BasketItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int id, string type)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(p => p.IDSanPham == id);
            if (item != null)
            {
                if (type == "increase")
                    item.SoLuong++;
                else if (type == "decrease")
                {
                    item.SoLuong--;
                    if (item.SoLuong <= 0)
                        cart.Remove(item);
                }
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteItem(int id)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(p => p.IDSanPham == id);
            if (item != null)
                cart.Remove(item);
            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

    }
}
