/*// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DIHA.Areas.Identity.Models.AccountViewModels;
using DIHA.Areas.Identity.Models.ManageViewModels;
using DIHA.Areas.Identity.Models.RoleViewModels;
using DIHA.Areas.Identity.Models.UserViewModels;
using DIHA.Data;
using DIHA.ExtendMethods;
using DIHA.Models;
using DIHA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DIHA.Areas.Custom.Models.Data;
using Microsoft.Extensions.Logging;
using DIHA.Areas.Custom.Models.CustomViewModels;

namespace DIHA.Areas.Custom.Controllers
{

    [Area("Custom")]
    [Route("/ManageCustom/[action]")]
    public class CustomController : Controller
    {
        private readonly AppDbContext _context;

        private readonly UserManager<AppCustom> _customManager;

        public CustomController(AppDbContext context, UserManager<AppCustom> customManager)
        {
            _context = context;
            _customManager = customManager;
        }



        [TempData]
        public string StatusMessage { get; set; }

        //
        // GET: /ManageUser/Index
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage)
        {
            var model = new CustomListModel();
            model.currentPage = currentPage;

            var qr = _customManager.Users.OrderBy(u => u.CustomName);

            model.totalCustoms = await qr.CountAsync();
            model.countPages = (int)Math.Ceiling((double)model.totalCustoms / model.ITEMS_PER_PAGE);

            if (model.currentPage < 1)
                model.currentPage = 1;
            if (model.currentPage > model.countPages)
                model.currentPage = model.countPages;

            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SetPasswordAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            var custom = await _customManager.FindByIdAsync(id);
            ViewBag.custom = ViewBag;

            if (custom == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            return View();
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPasswordAsync(string id, SetCustomPasswordModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            var custom = await _customManager.FindByIdAsync(id);
            ViewBag.custom = ViewBag;

            if (custom == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _customManager.RemovePasswordAsync(custom);

            var addPasswordResult = await _customManager.AddPasswordAsync(custom, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            StatusMessage = $"Vừa cập nhật mật khẩu cho user: {custom.CustomName}";

            return RedirectToAction("Index");
        }
    }
}
*/