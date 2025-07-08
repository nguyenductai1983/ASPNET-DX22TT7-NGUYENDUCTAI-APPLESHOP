using AppleShop.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AppleShop.Controllers
{
    [Authorize] // Chỉ người dùng đã đăng nhập mới được truy cập
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Orders/Index hoặc /Orders
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var userOrders = db.Orders
                                .Where(o => o.UserId == currentUserId)
                                .OrderByDescending(o => o.OrderDate)
                                .ToList();
            return View(userOrders);
        }

        // GET: /Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .SingleOrDefault(o => o.Id == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            // KIỂM TRA BẢO MẬT: Đảm bảo người dùng chỉ xem được đơn hàng của chính mình
            var currentUserId = User.Identity.GetUserId();
            if (order.UserId != currentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(order);
        }
    }
}