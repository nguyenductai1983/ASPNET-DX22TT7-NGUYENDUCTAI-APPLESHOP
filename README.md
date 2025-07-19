# AppleShop
Xây dựng website bán sản phẩm công nghệ của Apple
## Mô tả
Website bán sản phẩm công nghệ của Apple, 
bao gồm các tính năng như quản lý sản phẩm, giỏ hàng, thanh toán, quản lý người dùng và quản lý đơn hàng. 
Dự án được xây dựng với ASP.NET Core MVC và Entity Framework Core.
### Tên SV thực hiện: Nguyễn Đức Tài
### Email: nguyenductai1983@gmail.com
### SĐT: 0906 585 600

## Công nghệ sử dụng
C#, ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap, jQuery, Font Awesome
## Dữ liệu mẫu nằm trong thư mục gôc
Schema and data.sql
## Thông tin Admin
user: admin@appleshop.com
password: Admin@123

## Lỗi 
###Could not find file '..\AppleShop\bin\roslyn\csc.exe'.
Restore NuGet Packages (Thử cách này trước)
Trong Solution Explorer của Visual Studio, chuột phải vào Solution 'AppleShop'.
Chọn Restore NuGet Packages.
Sau khi hoàn tất, hãy Build lại Solution (nhấn Ctrl + Shift + B). Lỗi sẽ được khắc phục.
Update-Package -reinstall Microsoft.CodeDom.Providers.DotNetCompilerPlatform
### Lỗi xuất file PDF cài lại 
Install-Package Rotativa
### Chưa có user admin để đăng nhập
Update-Database