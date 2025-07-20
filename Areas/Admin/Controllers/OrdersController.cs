using AppleShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using System.Text;
using Rotativa;
using System;
using System.IO;
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
        // CẬP NHẬT LẠI ACTION EXPORTTOCSV
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
                string orderIdForCsv = $"=\"{item.Id}\"";
                string phoneNumberForCsv = $"=\"{item.ShipPhoneNumber}\"";
                sb.AppendLine($"{orderIdForCsv},{customerEmail},{item.OrderDate:dd/MM/yyyy HH:mm},{shipName},{shipAddress},{phoneNumberForCsv},{item.Status},{item.Total}");

            }
            string fileName = $"DanhSachDonHang_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            // Phương pháp tường minh để đảm bảo có BOM
            using (var memoryStream = new MemoryStream())
            {
                // 1. Lấy BOM của UTF-8
                byte[] bom = Encoding.UTF8.GetPreamble();
                // 2. Ghi BOM vào đầu stream
                memoryStream.Write(bom, 0, bom.Length);

                // 3. Ghi nội dung file (đã được mã hóa UTF-8 không có BOM)
                byte[] contentBytes = Encoding.UTF8.GetBytes(sb.ToString());
                memoryStream.Write(contentBytes, 0, contentBytes.Length);

                // 4. Trả về file từ MemoryStream
                return File(memoryStream.ToArray(), "text/csv", fileName);
            }
        }

        public ActionResult ExportToPdf()
        {
            var orders = db.Orders.Include(o => o.User).ToList();
            string fileName = $"DanhSachDonHang_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            return new ViewAsPdf("OrdersPdf", orders)
            {
                FileName = fileName,
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
