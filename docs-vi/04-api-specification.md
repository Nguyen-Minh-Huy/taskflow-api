# Đặc tả API

## 1. Tổng quan

API được tổ chức xung quanh các tài nguyên REST. Tất cả các yêu cầu và phản hồi đều ở định dạng JSON.
**URL Cơ sở**: `/api/v1`

### 1.1 Định dạng Phản hồi

Phản hồi thành công tiêu chuẩn:

```json
{
  "success": true,
  "message": "Operation successful",
  "data": { ... }
}
```

Phản hồi lỗi:

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": ["Email is required", "Password is too short"]
}
```

## 2. Các Điểm cuối (Endpoints)

### 2.1 Xác thực (Authentication)

| Phương thức | Điểm cuối        | Mô tả                  | Yêu cầu Auth       |
| :---------- | :--------------- | :--------------------- | :----------------- |
| `POST`      | `/auth/register` | Đăng ký người dùng mới | Không              |
| `POST`      | `/auth/login`    | Xác thực và nhận JWT   | Không              |
| `POST`      | `/auth/refresh`  | Làm mới access token   | Có (Refresh Token) |

#### POST /auth/register

**Body Yêu cầu:**

```json
{
  "email": "user@example.com",
  "password": "Password123@",
  "phoneNumber": "+1234567890",
  "displayName": "John Doe"
}
```

#### POST /auth/login

**Body Yêu cầu:**

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Phản hồi:**

```json
{
  "accessToken": "ey...",
  "refreshToken": "7a...",
  "expiresAt": "2023-10-01T12:00:00Z"
}
```

### 2.2 Người dùng (Users)

| Phương thức | Điểm cuối                | Mô tả                         | Yêu cầu Auth |
| :---------- | :----------------------- | :---------------------------- | :----------- |
| `GET`       | `/users/me`              | Lấy hồ sơ người dùng hiện tại | Có           |
| `PUT`       | `/users/me`              | Cập nhật chi tiết hồ sơ       | Có           |
| `PUT`       | `/users/change-password` | Đổi mật khẩu                  | Có           |

#### PUT /users/me

**Body Yêu cầu:**

```json
{
  "displayName": "Jane Doe",
  "phoneNumber": "+1234567890"
}
```

### 2.3 Nhóm (Teams)

Rules:

- Chỉ Admin mới được mời / xóa / đổi role thành viên
- Không cho Admin cuối cùng rời nhóm
- Team phải có ít nhất 1 Admin

| Phương thức | Điểm cuối                          | Mô tả                       | Yêu cầu Auth |
| :---------- | :--------------------------------- | :-------------------------- | :----------- |
| `GET`       | `/teams`                           | Lấy danh sách nhóm của user | Có           |
| `POST`      | `/teams`                           | Tạo một nhóm mới            | Có           |
| `GET`       | `/teams/{id}`                      | Lấy thông tin nhóm          | Có           |
| `PUT`       | `/teams/{id}`                      | Cập nhật thông tin nhóm     | Có           |
| `DELETE`    | `/teams/{id}`                      | Xóa nhóm                    | Có           |
| `POST`      | `/teams/{id}/invitations`          | Mời thành viên vào nhóm     | Có           |
| `POST`      | `/team-invitations/{token}/accept` | Chấp nhận lời mời vào nhóm  | Có           |
| `POST`      | `/team-invitations/{token}/reject` | Từ chối lời mời vào nhóm    | Có           |
| `GET`       | `/teams/{id}/members`              | Lấy danh sách thành viên    | Có           |
| `PATCH`     | `/teams/{id}/members/{userId}`     | Cập nhật role thành viên    | Có           |
| `DELETE`    | `/teams/{id}/members/{userId}`     | Xóa thành viên khỏi nhóm    | Có           |
| `POST`      | `/teams/{id}/leave`                | Thoát khỏi nhóm             | Có           |

#### POST /teams (Tạo một nhóm mới)

**Body Yêu cầu:**

```json
{
  "name": "Team Name",
  "description": "Team Description",
  "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

#### PUT /teams/{id} (Cập nhật thông tin nhóm)

**Body Yêu cầu:**

```json
{
  "name": "Update Team Name",
  "description": "Update Team Description"
}
```

#### POST /teams/{id}/invitations – Mời thành viên vào nhóm

( Ghi chú
role: Admin | Member,....
Nếu email chưa tồn tại → gửi invite, user tham gia sau khi đăng ký )

**Body Yêu cầu:**

```json
{
  "email": "user@example.com",
  "role": "Member"
}
```

#### PATCH /teams/{id}/members/{userId} – Cập nhật role thành viên

( Chỉ Admin mới được đổi role
Không cho hạ quyền Admin cuối cùng )

**Body Yêu cầu:**

```json
{
  "role": "Admin"
}
```

### 2.4 Dự án (Projects)

| Phương thức | Điểm cuối        | Mô tả                               | Yêu cầu Auth |
| :---------- | :--------------- | :---------------------------------- | :----------- |
| `GET`       | `/projects`      | Liệt kê các dự án hiển thị với user | Có           |
| `GET`       | `/projects/{id}` | Lấy chi tiết dự án                  | Có           |
| `POST`      | `/projects`      | Tạo một dự án mới                   | Có           |
| `PUT`       | `/projects/{id}` | Cập nhật dự án                      | Có           |
| `DELETE`    | `/projects/{id}` | Xóa dự án                           | Có           |

#### POST /projects

**Body Yêu cầu:**

```json
{
  "name": "Website Redesign",
  "description": "Overhaul the company website",
  "teamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "startDate": "2023-11-01T00:00:00Z",
  "dueDate": "2023-12-31T00:00:00Z"
}
```

### 2.5 Công việc (Tasks)

| Phương thức | Điểm cuối                     | Mô tả                             | Yêu cầu Auth |
| :---------- | :---------------------------- | :-------------------------------- | :----------- |
| `GET`       | `/projects/{projectId}/tasks` | Lấy danh sách công việc của dự án | Có           |
| `POST`      | `/tasks`                      | Tạo công việc mới                 | Có           |
| `GET`       | `/tasks/{id}`                 | Lấy chi tiết công việc            | Có           |
| `PUT`       | `/tasks/{id}`                 | Cập nhật trạng thái hoặc chi tiết | Có           |
| `DELETE`    | `/tasks/{id}`                 | Xóa công việc                     | Có           |
| `POST`      | `/tasks/{id}/assign`          | Giao công việc cho người dùng     | Có           |

#### POST /tasks

**Body Yêu cầu:**

```json
{
  "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Design Homepage",
  "description": "Create Figma mockups",
  "status": "Todo",
  "priority": "High",
  "startDate": "2023-11-05T00:00:00Z",
  "dueDate": "2023-11-10T00:00:00Z",
  "assignedUserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### 2.6 Bình luận (Comments)

| Phương thức | Điểm cuối                  | Mô tả                       | Yêu cầu Auth |
| :---------- | :------------------------- | :-------------------------- | :----------- |
| `GET`       | `/tasks/{taskId}/comments` | Lấy bình luận của công việc | Có           |
| `POST`      | `/tasks/{taskId}/comments` | Thêm bình luận              | Có           |
| `DELETE`    | `/comments/{id}`           | Xóa bình luận               | Có           |

#### POST /tasks/{taskId}/comments

**Body Yêu cầu:**

```json
{
  "content": "Updated the logo assets."
}
```
