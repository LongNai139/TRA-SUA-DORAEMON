using DIHA.Areas.Basket.Models.Data;
using DIHA.Areas.Invoice.Models.Data;
using DIHA.Areas.Product.Models.Data;
using DIHA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DIHA.Areas.Basket.Controllers
{
    [Area("Invoice")]
    [Route("invoice_Controller/[action]")]
    public class InvoiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public InvoiceController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateInvoice(string paymentMethod, string shippingAddress, string shippingPhoneNumber, string tenKhach)
        {
            var user = await _userManager.GetUserAsync(User);

            var cartJson = HttpContext.Session.GetString("Cart");

            var cart = GetCartFromSession();

            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống.";
                return RedirectToAction("Index", "Basket", new { area = "Basket" });
            }
            var sanPhamIds = cart.Select(i => i.IDSanPham).ToList();
            var sanPhams = _context.SanPhams
                .Where(sp => sanPhamIds.Contains(sp.IDSanPham))
                .ToDictionary(sp => sp.IDSanPham, sp => sp.DonGia);

            var invoice = new HoaDon
            {
                IdCustom = user.IdCustom,
                DaMua = cart.Sum(i => i.SoLuong),
                TenKhach = tenKhach,
                PhuongThucThanhToan = paymentMethod,
                NgayMua = DateTime.Now,
                DiaChiGiaoHang = shippingAddress,
                SoDienThoaiNhanHang = shippingPhoneNumber,
                TongTien = cart.Sum(i => i.SoLuong * (sanPhams.ContainsKey(i.IDSanPham) ? sanPhams[i.IDSanPham] : 0)),
                ChiTietHoaDons = new List<ChiTietHoaDon>()
            };

            foreach (var item in cart)
            {
                invoice.ChiTietHoaDons.Add(new ChiTietHoaDon
                {
                    IDSanPham = item.IDSanPham,
                    SoLuongMua = item.SoLuong,
                });
            }

            try
            {
                _context.HoaDons.Add(invoice);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Cart");

                TempData["Success"] = "Đơn hàng đã được tạo thành công.";
                return RedirectToAction("CustomPage", "Product", new { area = "Product" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tạo hóa đơn: " + ex.Message;
                return RedirectToAction("Index", "Basket", new { area = "Basket" });
            }
        }
        private List<BasketItem> GetCartFromSession()
        {
            var data = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(data)
                ? new List<BasketItem>()
                : JsonSerializer.Deserialize<List<BasketItem>>(data)!;
        }
        [HttpPost]
        public async Task<IActionResult> ApproveOrder(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            if (hoaDon.TrangThai == "Đã duyệt")
            {
                return BadRequest("Đơn hàng đã được duyệt trước đó.");
            }

            hoaDon.TrangThai = "Đã duyệt";
            hoaDon.NguoiDuyet = User.Identity?.Name;

            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
