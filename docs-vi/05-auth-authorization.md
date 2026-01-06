# Xác thực & Phân quyền

## 1. Chiến lược Xác thực

Hệ thống sử dụng **JWT (JSON Web Tokens)** để xác thực phi trạng thái (stateless).

### 1.1 Vòng đời Token

1. **Đăng nhập**: Người dùng gửi thông tin đăng nhập đến `/api/v1/auth/login`.
2. **Cấp phát**: Máy chủ xác thực thông tin và cấp phát:
   - `AccessToken`: Ngắn hạn (ví dụ: 15 phút). Dùng để truy cập API.
   - `RefreshToken`: Dài hạn (ví dụ: 7 ngày). Dùng để lấy access token mới.
3. **Sử dụng**: Client gửi `AccessToken` trong header Authorization:
   ```
   Authorization: Bearer <token>
   ```
4. **Hết hạn**: Khi `AccessToken` hết hạn, client gửi `RefreshToken` đến `/auth/refresh` để lấy cặp token mới.

### 1.2 Cấu trúc JWT Payload

Access Token chứa các claims sau:

```json
{
  "sub": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // ID Người dùng
  "email": "user@example.com",
  "name": "John Doe",
  "roles": ["SystemAdmin"], // Các vai trò cấp hệ thống
  "iat": 1698765432,
  "exp": 1698766332
}
```

> Lưu ý: Quyền cấp nhóm (Team-level permissions) **không** được lưu trong JWT để tránh token bị phình to và cũ. Chúng được kiểm tra dựa trên cơ sở dữ liệu hoặc cache mỗi khi có yêu cầu truy cập tài nguyên nhóm.

## 2. Chính sách Phân quyền

### 2.1 Ma trận Kiểm soát Truy cập dựa trên Vai trò (RBAC)

Quyền hạn được thực thi ở cấp độ **Nhóm (Team)**.

| Hành động                | Chủ sở hữu (Team Owner) | Quản trị viên (Team Admin) | Thành viên (Team Member) |
| :----------------------- | :---------------------- | :------------------------- | :----------------------- |
| **Quản lý Nhóm**         |                         |                            |                          |
| Cập nhật Cài đặt Nhóm    | ✅                      | ❌                         | ❌                       |
| Mời Thành viên           | ✅                      | ✅                         | ❌                       |
| Xóa Thành viên           | ✅                      | ✅                         | ❌                       |
| **Quản lý Dự án**        |                         |                            |                          |
| Tạo Dự án                | ✅                      | ✅                         | ❌                       |
| Xóa Dự án                | ✅                      | ✅                         | ❌                       |
| **Quản lý Công việc**    |                         |                            |                          |
| Tạo Công việc            | ✅                      | ✅                         | ✅                       |
| Giao Công việc           | ✅                      | ✅                         | ✅                       |
| Xóa Công việc            | ✅                      | ✅                         | ❌                       |
| **Bình luận**            |                         |                            |                          |
| Đăng Bình luận           | ✅                      | ✅                         | ✅                       |
| Xóa Bất kỳ Bình luận nào | ✅                      | ✅                         | ❌                       |
| Xóa Bình luận của mình   | ✅                      | ✅                         | ✅                       |

### 2.2 Các Thực tiễn Bảo mật Tốt nhất

- **Băm Mật khẩu**: BCrypt được sử dụng để băm mật khẩu.
- **HTTPS**: Tất cả lưu lượng truy cập phải được mã hóa qua TLS.
- **Giảm thiểu Phạm vi**: Token chỉ chứa các claims thiết yếu (`sub`, `email`) để giữ payload nhỏ gọn.
- **Xác thực Đầu vào**: Xác thực nghiêm ngặt tất cả các đầu vào để ngăn chặn các cuộc tấn công injection.
