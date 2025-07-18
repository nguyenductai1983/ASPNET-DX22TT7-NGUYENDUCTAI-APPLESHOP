namespace AppleShop.Models
{
    public enum OrderStatus
    {
        Pending,      // Đang chờ xử lý
        Processing,   // Đang xử lý
        Shipped,      // Đã giao hàng
        Delivered,    // Đã nhận hàng
        Cancelled     // Đã hủy
    }
}