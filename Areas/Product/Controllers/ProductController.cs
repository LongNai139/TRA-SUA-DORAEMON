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
using DIHA.Areas.Invoice.Models.Data;
namespace DIHA.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("product_Controller/[action]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProductController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var danhMucList = await _context.DanhMucs
                .Where(d => d.DaXoa == false || d.DaXoa == null)
                .ToListAsync();

            ViewBag.DanhmucSelectList = new SelectList(danhMucList, "IDDanhMuc", "TenDanhMuc");

            var sanPhamList = await _context.SanPhams
                .Where(s => s.DaXoa == false || s.DaXoa == null)
                .Include(s => s.DanhMuc)
                .ToListAsync();
            return View(sanPhamList);
        }
        public async Task<IActionResult> CustomPage()
        {
            var danhMucList = await _context.DanhMucs
                .Where(d => d.DaXoa == false || d.DaXoa == null)
                .ToListAsync();

            ViewBag.DanhmucSelectList = new SelectList(danhMucList, "IDDanhMuc", "TenDanhMuc");

            var sanPhamList = await _context.SanPhams
                .Where(s => s.DaXoa == false || s.DaXoa == null)
                .Include(s => s.DanhMuc)
                .ToListAsync();
            return View(sanPhamList);
        }
        public async Task<IActionResult> StaffPage()
        {
            var danhMucList = await _context.DanhMucs
                .Where(d => d.DaXoa == false || d.DaXoa == null)
                .ToListAsync();

            ViewBag.DanhmucSelectList = new SelectList(danhMucList, "IDDanhMuc", "TenDanhMuc");

            var sanPhamList = await _context.SanPhams
                .Where(s => s.DaXoa == false || s.DaXoa == null)
                .Include(s => s.DanhMuc)
                .ToListAsync();
            return View(sanPhamList);
        }
        public async Task<IActionResult> OrderConfirmationPage()
        {
            var hoaDonList = await _context.HoaDons
                .Include(ct => ct.ChiTietHoaDons)
                .ThenInclude(sp => sp.SanPham)
                .Where(h => h.TrangThai == "Chờ xử lý")
                .ToListAsync();
            return View(hoaDonList);
        }
        public async Task<IActionResult> StatisticsPage()
        {
            var hoaDonList = await _context.HoaDons
                .Include(ct => ct.ChiTietHoaDons)
                    .ThenInclude(sp => sp.SanPham)
                .ToListAsync();
            return View(hoaDonList);
        }
        [HttpGet]
        public IActionResult CreateProductView(int IDSanPham) => View(new SanPham());

        [HttpPost]
        public async Task<IActionResult> CreateProduct(SanPham product)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin sản phẩm";
                return RedirectToAction("Index");
            }
            string tenSanphamLower = product.TenSanpham.Trim().ToLower();
            var existingProduct = _context.SanPhams
                .FirstOrDefault(e => e.TenSanpham.Trim().ToLower() == tenSanphamLower);

            if (existingProduct != null)
            {
                if (existingProduct.DaXoa == false)
                {
                    TempData["Error"] = "Tên sản phẩm đã tồn tại";
                    return RedirectToAction("Index");
                }
                else
                {
                    existingProduct.DaXoa = false;
                    _context.SanPhams.Update(existingProduct);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Sản phẩm đã được khôi phục";
                    return RedirectToAction("Index");
                }
            }
            product.DaXoa = false;
            _context.SanPhams.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã thêm một sản phẩm mới";
            return RedirectToAction("Index");
        }
        private bool ProductDetailExists(string TenSanpham)
        {
            return _context.SanPhams.Any(e => (e.DaXoa == false || e.DaXoa == null) && e.TenSanpham.ToLower() == TenSanpham.ToLower()); // Kiểm tra danh mục có tồn tại không
        }
        private async Task<string> SaveImage(IFormFile file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);
            return "/image/" + fileName;
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int IDSanPham)
        {
            if (IDSanPham == 0)
            {
                return BadRequest("ID sản phẩm không hợp lệ!");
            }

            var product = await _context.SanPhams.FindAsync(IDSanPham);
            return product != null ? Ok(product) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostEditProduct(int IDSanPham, string SuaTenSanPham, decimal DonGia, int IDDanhMuc, IFormFile file)
        {
            if (IDSanPham == 0)
            {
                return BadRequest("ID sản phẩm không hợp lệ!");
            }

            var product = await _context.SanPhams.FindAsync(IDSanPham);
            if (product == null)
            {
                return NotFound();
            }
            var danhMucTonTai = await _context.DanhMucs.AnyAsync(dm => dm.IDDanhMuc == IDDanhMuc);
            if (!danhMucTonTai)
            {
                return BadRequest("Danh mục không hợp lệ!");
            }

            // Cập nhật thông tin sản phẩm
            product.TenSanpham = SuaTenSanPham;
            product.DonGia = DonGia;
            product.IDDanhMuc = IDDanhMuc;

            if (file != null && file.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine("wwwroot/image", fileName);

                using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(400, 400),
                        Mode = ResizeMode.Max
                    }));

                    var encoder = new JpegEncoder
                    {
                        Quality = 80
                    };

                    await image.SaveAsJpegAsync(filePath, encoder);
                }
                product.Anh = "/image/" + fileName;
            }

            _context.SanPhams.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.SanPhams.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product); // Trả về sản phẩm dưới dạng JSON
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProductConfirmed(int IDSanPhamHidden)
        {
            var product = await _context.SanPhams.FindAsync(IDSanPhamHidden);
            if (product == null)
            {
                return NotFound();
            }
            product.DaXoa = true;
            _context.SanPhams.Update(product); // Xóa sản phẩm
            await _context.SaveChangesAsync(); // Lưu thay đổi
            TempData["Success"] = "Đã xóa một sản phẩm.";
            return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách sản phẩm
        }

        public async Task<IActionResult> LaySanPhamTheoID(int IDSanPham)
        {
            Console.WriteLine("IDSanPham nhận được: " + IDSanPham);
            var sanpham = await _context.SanPhams
                                       .Where(sp => sp.IDSanPham == IDSanPham)
                                       .Include(sp => sp.DanhMuc)
                                       .FirstOrDefaultAsync();
            if (sanpham == null || sanpham.DaXoa == true)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm!" });
            }
            var result = new
            {
                IDsanpham = sanpham.IDSanPham,
                tensanpham = sanpham.TenSanpham,
                dongia = sanpham.DonGia,
                IDdanhmuc = sanpham.IDDanhMuc,
                tendanhmuc = sanpham.DanhMuc?.TenDanhMuc, 
                anh = sanpham.Anh
            };
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductDetail(int IDDanhMuc)
        {
            if (IDDanhMuc == 0)
            {
                return NotFound();
            }


            var SanPham = await _context.SanPhams
                                          .Include(dm => dm.DanhMuc)
                                          .Where(dm => dm.IDDanhMuc == IDDanhMuc && dm.DaXoa == false)
                                          .ToListAsync();
            if (!SanPham.Any())
            {
                ViewBag.ErrorMessage = "Không có sản phẩm nào trong danh mục này.";
            }
            var danhMucList = await _context.DanhMucs
                .Where(dm => dm.DaXoa == false || dm.DaXoa == null)
                .Select(dm => new { dm.IDDanhMuc, dm.TenDanhMuc })
                .ToListAsync();

            ViewBag.DanhmucSelectList = new SelectList(danhMucList, "IDDanhMuc", "TenDanhMuc");
            ViewBag.IDDanhMuc = IDDanhMuc;
            foreach (var danhMuc in danhMucList)
            {
                Console.WriteLine($"ID: {danhMuc.IDDanhMuc}, Tên: {danhMuc.TenDanhMuc}");
            }
            return View("Index", SanPham);
        }
        public async Task<IActionResult> SeedDataAsync()
        {
            var rolenames = typeof(RoleName).GetFields().ToList();
            foreach (var role in rolenames)
            {
                var roleName = (string)role.GetRawConstantValue();
                var rfound = await _roleManager.FindByNameAsync(roleName);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var useradmin = await _userManager.FindByNameAsync("admin");
            if (useradmin == null)
            {
                var user = new AppUser
                {
                    UserName = "admin",
                    Email = "maibaolong@example.com",
                    EmailConfirmed = true,
                };
                await _userManager.CreateAsync(user, "admin123");
                await _userManager.AddToRoleAsync(user, RoleName.Administrator);
            }
            TempData["StatusMessage"] = "Đã tạo dữ liệu mẫu thành công!";
            return RedirectToAction("Index");
        }
        public IActionResult Doraemon()
        {
            var products = _context.SanPhams.Where(x => x.TenSanpham.Contains("Doraemon")).ToList();

            // Lấy giỏ hàng từ session
            var cartData = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartData) ? new List<BasketItem>() : JsonSerializer.Deserialize<List<BasketItem>>(cartData)!;

            ViewBag.Cart = cart;

            return View(products);
        }
        
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int IDHoaDon)
        {
            var order = await _context.HoaDons.FindAsync(IDHoaDon);
            if (order == null)
            {
                return NotFound();
            }

            order.TrangThai = "Hủy đơn";

            _context.HoaDons.Update(order);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã hủy đơn hàng thành công" });
        }
        public IActionResult DoanhThuTheoThang()
        {
            var today = DateTime.Now;
            int currentMonth = today.Month;
            int currentYear = today.Year;

            var hoaDons = _context.HoaDons
                .Where(hd => hd.NgayMua.Month == currentMonth &&
                             hd.NgayMua.Year == currentYear &&
                             hd.TrangThai == "Đã duyệt")
                .ToList();

            var doanhThuTheoNgay = Enumerable.Range(1, DateTime.DaysInMonth(currentYear, currentMonth))
                .Select(ngay =>
                {
                    var hoaDonTrongNgay = hoaDons
                        .Where(hd => hd.NgayMua.Day == ngay)
                        .ToList();

                    decimal tongTien = hoaDonTrongNgay.Sum(hd => hd.TongTien ?? 0);
                    int soDon = hoaDonTrongNgay.Count;
                    int soMon = hoaDonTrongNgay.Sum(hd => hd.DaMua);

                    return new
                    {
                        Ngay = ngay,
                        DoanhThu = tongTien,
                        SoDon = soDon,
                        SoMon = soMon
                    };
                }).ToList();

            return Json(doanhThuTheoNgay);
        }
        public IActionResult DoanhThuTheoNgay()
        {
            var today = DateTime.Today;
            var startHour = 7;
            var endHour = 22;

            var hoaDons = _context.HoaDons
                .Where(hd =>
                    hd.NgayMua.Date == today &&
                    hd.TrangThai == "Đã duyệt")
                .ToList();

            var doanhThuTheoKhungGio = Enumerable.Range(startHour, endHour - startHour)
                .Select(gio =>
                {
                    var start = today.AddHours(gio);
                    var end = today.AddHours(gio + 1);

                    var hoaDonTrongKhung = hoaDons
                        .Where(hd => hd.NgayMua >= start && hd.NgayMua < end)
                        .ToList();

                    decimal tongTien = hoaDonTrongKhung.Sum(hd => hd.TongTien ?? 0);
                    int soDon = hoaDonTrongKhung.Count;
                    int soMon = hoaDonTrongKhung.Sum(hd => hd.DaMua);

                    return new
                    {
                        KhungGio = $"{start:HH:mm} - {end:HH:mm}",
                        DoanhThu = tongTien,
                        SoDon = soDon,
                        SoMon = soMon
                    };
                }).ToList();

            return Json(doanhThuTheoKhungGio);
        }

        public IActionResult DoanhThuTheoNgayTuChon(DateTime batDau, DateTime ketThuc)
        {
            // Bao gồm cả ngày kết thúc
            ketThuc = ketThuc.Date.AddDays(1);

            var hoaDons = _context.HoaDons
                .Where(hd => hd.TrangThai == "Đã duyệt" &&
                             hd.NgayMua >= batDau.Date && hd.NgayMua < ketThuc)
                .ToList();

            var soNgay = (ketThuc - batDau).Days;

            var doanhThuTheoNgay = Enumerable.Range(0, soNgay)
                .Select(i =>
                {
                    var ngay = batDau.Date.AddDays(i);

                    var hoaDonTrongNgay = hoaDons
                        .Where(hd => hd.NgayMua.Date == ngay)
                        .ToList();

                    decimal tongTien = hoaDonTrongNgay.Sum(hd => hd.TongTien ?? 0);
                    int soDon = hoaDonTrongNgay.Count;
                    int soMon = hoaDonTrongNgay.Sum(hd => hd.DaMua); // hoặc dùng ChiTietHoaDon

                    return new
                    {
                        Ngay = ngay.ToString("dd/MM/yyyy"),
                        DoanhThu = tongTien,
                        SoDon = soDon,
                        SoMon = soMon
                    };
                }).ToList();

            return Json(doanhThuTheoNgay);
        }


        public IActionResult DoanhThuToanBo()
        {
            var batDau = new DateTime(2025, 6, 1);

            var ketThuc = DateTime.Now;

            var hoaDons = _context.HoaDons
                .Where(hd => hd.TrangThai == "Đã duyệt" &&
                             hd.NgayMua >= batDau && hd.NgayMua <= ketThuc)
                .ToList();

            var soNgay = (ketThuc.Date - batDau.Date).Days + 1;

            var doanhThuTheoNgay = Enumerable.Range(0, soNgay)
                .Select(i =>
                {
                    var ngay = batDau.Date.AddDays(i);

                    var hoaDonTrongNgay = hoaDons
                        .Where(hd => hd.NgayMua.Date == ngay)
                        .ToList();

                    decimal tongTien = hoaDonTrongNgay.Sum(hd => hd.TongTien ?? 0);
                    int soDon = hoaDonTrongNgay.Count;
                    int soMon = hoaDonTrongNgay.Sum(hd => hd.DaMua);

                    return new
                    {
                        Ngay = ngay.ToString("dd/MM/yyyy"),
                        DoanhThu = tongTien,
                        SoDon = soDon,
                        SoMon = soMon
                    };
                }).ToList();

            return Json(doanhThuTheoNgay);
        }
        public IActionResult TopSanPhamBanChay(int top = 5)
        {
            var data = _context.HoaDons
                .Where(hd => hd.TrangThai == "Đã duyệt")
                .SelectMany(hd => hd.ChiTietHoaDons)
                .GroupBy(ct => ct.IDSanPham)
                .Select(g => new
                {
                    IDSanPham = g.Key,
                    TenSanPham = g.First().SanPham.TenSanpham,
                    TongDaBan = g.Sum(x => x.SoLuongMua)
                })
                .OrderByDescending(x => x.TongDaBan)
                .Take(top)
                .ToList();

            return Json(data);
        }

        public IActionResult TopKhachHang(int top = 5)
        {
            var data = _context.HoaDons
                .Where(hd => hd.TrangThai == "Đã duyệt")
                .GroupBy(hd => hd.TenKhach)
                .Select(g => new
                {
                    tenKhach = g.Key,
                    tongDaMua = g.SelectMany(hd => hd.ChiTietHoaDons).Sum(ct => ct.SoLuongMua)
                })
                .OrderByDescending(x => x.tongDaMua)
                .Take(top)
                .ToList();

            return Json(data);
        }
    }
}
