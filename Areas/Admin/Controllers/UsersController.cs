﻿using AppleShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AppleShop.Areas.Admin.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
namespace AppleShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        // GET: Admin/Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user.Id);
            var allRoles = _roleManager.Roles.ToList();

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };

            return View(viewModel);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // --- BẮT ĐẦU LOGIC XỬ LÝ MẬT KHẨU MỚI ---
                // Chỉ thực hiện đổi mật khẩu nếu trường NewPassword được nhập
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    // Xóa mật khẩu cũ trước
                    if (await _userManager.HasPasswordAsync(user.Id))
                    {
                        await _userManager.RemovePasswordAsync(user.Id);
                    }
                    // Thêm mật khẩu mới
                    var result = await _userManager.AddPasswordAsync(user.Id, model.NewPassword);

                    if (!result.Succeeded)
                    {
                        // Nếu có lỗi, thêm lỗi vào ModelState và hiển thị lại form
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        model.AllRoles = _roleManager.Roles.ToList().Select(r => new SelectListItem { Text = r.Name, Value = r.Name });
                        return View(model);
                    }
                }
                // --- KẾT THÚC LOGIC XỬ LÝ MẬT KHẨU ---

                // Xử lý vai trò (giữ nguyên như cũ)
                var userRoles = await _userManager.GetRolesAsync(user.Id);
                model.UserRoles = model.UserRoles ?? new List<string>();
                var rolesToRemove = userRoles.Except(model.UserRoles).ToArray();
                await _userManager.RemoveFromRolesAsync(user.Id, rolesToRemove);
                var rolesToAdd = model.UserRoles.Except(userRoles).ToArray();
                await _userManager.AddToRolesAsync(user.Id, rolesToAdd);

                return RedirectToAction("Index");
            }

            // Nếu có lỗi, tải lại danh sách roles để hiển thị lại form
            model.AllRoles = _roleManager.Roles.ToList().Select(r => new SelectListItem { Text = r.Name, Value = r.Name });
            return View(model);
        }
        // GET: Admin/Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Thay vì xóa, chúng ta sẽ khóa tài khoản vĩnh viễn
                // Bật chức năng khóa
                await _userManager.SetLockoutEnabledAsync(user.Id, true);
                // Set ngày hết hạn khóa là một ngày rất xa trong tương lai
                await _userManager.SetLockoutEndDateAsync(user.Id, DateTime.MaxValue);
            }
            return RedirectToAction("Index");
        }
    }
}