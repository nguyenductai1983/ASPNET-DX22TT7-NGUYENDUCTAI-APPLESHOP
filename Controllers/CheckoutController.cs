using AppleShop.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AppleShop.Controllers
{
    [Authorize] // Bắt buộc người dùng phải đăng nhập
    public class CheckoutController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        // POST: Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutViewModel viewModel)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                // Tạo đơn hàng
                var order = new Order
                {
                    UserId = User.Identity.GetUserId(),
                    OrderDate = DateTime.Now,
                    // Lấy thông tin từ ViewModel
                    ShipName = viewModel.ShipName,
                    ShipAddress = viewModel.ShipAddress,
                    ShipPhoneNumber = viewModel.ShipPhoneNumber,
                    Total = cart.Sum(i => i.Total)
                };

                // Thêm chi tiết đơn hàng
                order.OrderDetails = new List<OrderDetail>();
                foreach (var item in cart)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Xóa giỏ hàng
                Session["Cart"] = null;

                return RedirectToAction("Complete");
            }

            return View(viewModel);
        }

        public ActionResult Complete()
        {
            return View();
        }
    }
}