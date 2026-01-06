# Yêu cầu Nghiệp vụ

## 1. Giới thiệu

Tài liệu này phác thảo các yêu cầu chức năng và phi chức năng cho hệ thống TaskFlow. Hệ thống phục vụ như một backend API cho một ứng dụng khách quản lý công việc.

## 2. Yêu cầu Chức năng

### 2.1 Quản lý Người dùng

- **Đăng ký**: Người dùng phải có thể tạo tài khoản với email và mật khẩu.
- **Xác thực**: Người dùng phải đăng nhập để nhận access token (JWT).
- **Quản lý Hồ sơ**: Người dùng có thể cập nhật tên hiển thị và mật khẩu của họ.

### 2.2 Quản lý Không gian làm việc / Nhóm

- **Tạo Nhóm**: Người dùng có thể tạo các nhóm để nhóm các dự án và thành viên lại.
- **Mời Thành viên**: Quản trị viên nhóm có thể thêm người dùng khác vào nhóm.
- **Vai trò**: Hỗ trợ các vai trò riêng biệt (ví dụ: Chủ sở hữu, Quản trị viên, Thành viên) trong một nhóm.

### 2.3 Quản lý Dự án

- **Tạo Dự án**: Các nhóm có thể sở hữu nhiều dự án.
- **Khả năng hiển thị Dự án**: Các dự án có thể là riêng tư (chỉ nhóm) hoặc công khai (toàn tổ chức, phạm vi tương lai).
- **Chỉnh sửa/Xóa**: Chỉ chủ sở hữu dự án hoặc quản trị viên mới có thể sửa đổi siêu dữ liệu của dự án.

### 2.4 Quản lý Công việc

- **Tạo Công việc**: Người dùng có thể tạo công việc trong một dự án.
- **Phân công**: Công việc có thể được phân công cho các thành viên cụ thể trong nhóm.
- **Thuộc tính**: Công việc phải có:
  - Tiêu đề & Mô tả
  - Mức độ ưu tiên (Thấp, Trung bình, Cao, Khẩn cấp)
  - Trạng thái (Cần làm, Đang làm, Đang xem xét, Xong)
  - Ngày hết hạn
- **Bình luận**: Người dùng có thể thêm bình luận vào công việc để cộng tác.

### 2.5 Bảo mật & Quyền hạn

- **RBAC**: Truy cập vào tài nguyên phải được kiểm soát dựa trên vai trò của nhóm/dự án.
- **Cô lập Dữ liệu**: Người dùng không thể truy cập dữ liệu từ các dự án/nhóm mà họ không thuộc về.

## 3. Yêu cầu Phi chức năng

- **Hiệu suất**: Thời gian phản hồi API phải dưới 200ms cho 95% các yêu cầu đọc.
- **Khả năng mở rộng**: Thiết kế cơ sở dữ liệu nên hỗ trợ các chiến lược mở rộng theo chiều ngang trong tương lai.
- **Độ tin cậy**: Hệ thống nên xử lý các ngoại lệ một cách khéo léo và trả về các phản hồi lỗi được chuẩn hóa.
- **Tài liệu**: Tất cả các điểm cuối API phải được ghi lại thông qua Swagger.
