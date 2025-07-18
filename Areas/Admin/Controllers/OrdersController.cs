using AppleShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using System.Text;
using Rotativa;
using System;
using System.Collections.Generic; // Thêm using cho List

namespace AppleShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // CẬP NHẬT ACTION INDEX
        public ActionResult Index(string searchString, string statusFilter, int? page)
        {
            var orders = db.Orders.Include(o => o.User);

            if (!String.IsNullOrEmpty(statusFilter))
            {
                if (Enum.TryParse<OrderStatus>(statusFilter, out var status))
                {
                    orders = orders.Where(o => o.Status == status);
                }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Id.ToString().Contains(searchString) ||
                                           o.User.Email.Contains(searchString));
            }

            orders = orders.OrderByDescending(o => o.OrderDate);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatusFilter = statusFilter;

            // TẠO SELECTLIST CHO DROPDOWN LỌC TRẠNG THÁI
            var statusList = Enum.GetValues(typeof(OrderStatus))
                                 .Cast<OrderStatus>()
                                 .Select(e => new SelectListItem
                                 {
                                     Value = e.ToString(),
                                     Text = e.ToString() // Bạn có thể tùy chỉnh tên hiển thị ở đây
                                 });
            ViewBag.StatusFilterOptions = new SelectList(statusList, "Value", "Text", statusFilter);


            return View(orders.ToPagedList(pageNumber, pageSize));
        }


        // Các action khác (ExportToCsv, ExportToPdf, Details, Edit...) giữ nguyên như cũ
        public FileResult ExportToCsv()
        {
            var orders = db.Orders.Include(o => o.User).ToList();
            var sb = new StringBuilder();
            sb.AppendLine("MaDonHang,KhachHang,NgayDat,TenNguoiNhan,DiaChiGiao,SoDienThoai,TrangThai,TongTien");
            foreach (var item in orders)
            {
                string customerEmail = SanitizeCsvField(item.User?.Email);
                string shipName = SanitizeCsvField(item.ShipName);
                string shipAddress = SanitizeCsvField(item.ShipAddress);
                sb.AppendLine($"{item.Id},{customerEmail},{item.OrderDate:dd/MM/yyyy HH:mm},{shipName},{shipAddress},{item.ShipPhoneNumber},{item.Status},{item.Total}");
            }
            byte[] fileBytes = new UTF8Encoding(true).GetBytes(sb.ToString());
            return File(fileBytes, "text/csv", "DanhSachDonHang.csv");
        }

        public ActionResult ExportToPdf()
        {
            var orders = db.Orders.Include(o => o.User).ToList();
            return new ViewAsPdf("OrdersPdf", orders)
            {
                FileName = "DanhSachDonHang.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                CustomSwitches = "--encoding utf-8"
            };
        }

        private string SanitizeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            if (field.Contains(",")) return $"\"{field}\"";
            return field;
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Order order = db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .SingleOrDefault(o => o.Id == id);
            if (order == null) return HttpNotFound();
            return View(order);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Order order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();
            return View(order);
        }

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
    }
}
