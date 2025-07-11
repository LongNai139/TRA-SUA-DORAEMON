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

namespace DIHA.Areas.Category.Controllers
{
    [Area("Category")]
    [Route("category_Controller/[action]")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var danhMucList = await _context.DanhMucs
                .Where(d => d.DaXoa == false || d.DaXoa == null)
                .ToListAsync();

            ViewBag.DanhmucSelectList = new SelectList(danhMucList, "IDDanhMuc", "TenDanhMuc");
            return View(danhMucList);
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int IDDanhMuc)
        {
            if (IDDanhMuc == 0)
            {
                return BadRequest("ID danh mục không hợp lệ!");
            }

            var category = await _context.DanhMucs.FindAsync(IDDanhMuc);
            return category != null ? Ok(category) : NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> PostEditCategory(int IDDanhMuc, string SuaTenDanhMuc)
        {
            if (IDDanhMuc == 0)
            {
                return BadRequest("ID danh mục không hợp lệ!");
            }

            var category = await _context.DanhMucs.FindAsync(IDDanhMuc);
            if (category == null)
            {
                return NotFound();
            }
            category.TenDanhMuc = SuaTenDanhMuc;
            category.DaXoa = false;

            _context.DanhMucs.Update(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Category");
        }

        private bool CategoryExists(string TenDanhMuc)
        {
            return _context.DanhMucs.Any(e => (e.DaXoa == false || e.DaXoa == null) && e.TenDanhMuc.ToLower() == TenDanhMuc.ToLower()); // Kiểm tra danh mục có tồn tại không
        }

        public IActionResult CreateCategory(int DanhmucID) => View(new DanhMuc());

        [HttpPost]
        public async Task<IActionResult> CreateCategory(DanhMuc category)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin danh mục";
                return RedirectToAction("Index");
            }
            string tenDanhMucLower = category.TenDanhMuc.Trim().ToLower();
            var existingCategory = _context.DanhMucs
                .FirstOrDefault(e => e.TenDanhMuc.Trim().ToLower() == tenDanhMucLower);

            if (existingCategory != null)
            {
                if (existingCategory.DaXoa == false)
                {
                    TempData["Error"] = "Tên danh mục đã tồn tại";
                    return RedirectToAction("Index");
                }
                else
                {
                    existingCategory.DaXoa = false;
                    _context.DanhMucs.Update(existingCategory);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Danh mục đã được khôi phục";
                    return RedirectToAction("Index");
                }
            }
            category.DaXoa = false;
            _context.DanhMucs.Add(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã thêm một danh mục mới";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.DanhMucs.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category); // Trả về sản phẩm dưới dạng JSON
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategoryConfirmed(int IDDanhMuc)
        {
            var category = await _context.DanhMucs.FindAsync(IDDanhMuc);
            if (category == null)
            {
                return NotFound();
            }
            category.DaXoa = true;
            _context.DanhMucs.Update(category); // Xóa sản phẩm
            await _context.SaveChangesAsync(); // Lưu thay đổi
            TempData["Success"] = "Đã xóa một danh mục.";
            return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách sản phẩm
        }
        public async Task<IActionResult> GetbyCategoryID(int id)
        {
            var category = await _context.DanhMucs.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
    }
}
