using AppleShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AppleShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Action Index sẽ được xây dựng ở bước 2
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.User).OrderByDescending(o => o.OrderDate);
            return View(orders.ToList());
        }
        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,OrderDate,Total,ShipName,ShipAddress,ShipPhoneNumber,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }
        // Action Details sẽ được xây dựng ở bước 3
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy đơn hàng và tất cả các thông tin liên quan
            Order order = db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails.Select(od => od.Product)) // Lấy cả Chi tiết đơn hàng và Sản phẩm tương ứng
                .SingleOrDefault(o => o.Id == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }
}