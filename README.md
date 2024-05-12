# Quản lý hàng tồn kho sản phẩm
### Quản lý sản phẩm bằng thao tác CRUD với ASP.NET Core WebAPI

**Chạy API**
1. Điều kiện tiên quyết:
     - Visual Studio 2022
     - Cài đặt .NET 6 SDK nếu chưa có.
     - Đảm bảo bạn có phiên bản SQL Server.
2. Sao chép kho lưu trữ:
     - Sao chép kho lưu trữ dự án của bạn vào máy cục bộ.
4. Cấu hình kết nối cơ sở dữ liệu:
     - Mở tệp appsinstall.json trong dự án của bạn.
     - Cập nhật chuỗi DefaultConnection với chi tiết kết nối cơ sở dữ liệu của bạn (nếu sử dụng SQL Server).
5. Khởi tạo cơ sở dữ liệu:
     - Xóa tất cả các di chuyển hiện có khỏi thư mục Di chuyển.
     - Mở Package Manager Console (PMC) và chạy lệnh sau:
       - Thêm di chuyển "v1"
       - Cập nhật cơ sở dữ liệu
     - Lệnh **Add-Migration v1** sẽ tạo tập lệnh di chuyển thiết lập lược đồ cơ sở dữ liệu dựa trên mô hình hiện tại của bạn.
     - Lệnh **Update-Database** sẽ áp dụng quá trình di chuyển và tạo cơ sở dữ liệu.
6. Xây dựng và chạy:
     - Mở dấu nhắc lệnh/thiết bị đầu cuối trong thư mục dự án.
     - Chạy các lệnh sau:
       - khôi phục dotnet
       - xây dựng dotnet
       - chạy dotnet
7. Điểm cuối API:
     - Sử dụng Swagger cho tài liệu API tại https://localhost:7117/swagger/index.html
      
         ![ảnh chụp màn hình](Swagger.png)
     - Bạn có thể kiểm tra chúng bằng các công cụ như Postman hoặc trình duyệt của bạn.
8. Yêu cầu mẫu:
     - Lấy toàn bộ sản phẩm trong kho:
       - https://localhost:7117/api/ProductInventoryQuản lý/GetAllProductsFromInventory
     - Lấy sản phẩm từ kho bằng bộ lọc và phân trang:
       - https://localhost:7117/api/ProductInventoryQuản lý/GetProductsFromInventoryUsingFilter?Name=Mouse&Brand=HP&MinPrice=50&MaxPrice=600&PageNumber=1&PageSize=10
9. Ghi nhật ký bằng Serilog:
     - Chạy ứng dụng và kiểm tra bảng điều khiển hoặc tệp ProductInventoryQuản lýLogs.txt để tìm các thông báo được ghi trong thư mục dự án.