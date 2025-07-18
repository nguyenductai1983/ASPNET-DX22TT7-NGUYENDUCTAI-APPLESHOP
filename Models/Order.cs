using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppleShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal Total { get; set; }

        // Thêm các thông tin giao hàng
        [Required]
        public string ShipName { get; set; }
        [Required]
        public string ShipAddress { get; set; }
        [Required]
        public string ShipPhoneNumber { get; set; }
        public OrderStatus Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}