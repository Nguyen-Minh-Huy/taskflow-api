# Tổng quan Dự án TaskFlow

| Thuộc tính     | Giá trị                                                       |
| :------------- | :------------------------------------------------------------ |
| **Tên**        | TaskFlow                                                      |
| **Phiên bản**  | 1.0.0                                                         |
| **Trạng thái** | Đang phát triển                                               |
| **Tech Stack** | .NET Core 8.0, EF Core, SQL Server, React (Dự kiến)           |
| **Kiến trúc**  | Clean Architecture (Domain, Application, Infrastructure, API) |

## 🎯 Mục tiêu Dự án

TaskFlow là một Web API quản lý công việc mạnh mẽ, hướng đến làm việc nhóm, được thiết kế để hợp lý hóa việc cộng tác dự án. Mục tiêu của nó là cung cấp cho các tổ chức một nền tảng tập trung để theo dõi tiến độ, quản lý khối lượng công việc và đảm bảo việc bàn giao dự án hiệu quả.

## 🔑 Tính năng Chính

- **Quản lý Dự án**: Tổ chức các công việc thành các dự án dễ quản lý.
- **Theo dõi Công việc**: Quản lý toàn bộ vòng đời của công việc (ToDo, InProgress, Done).
- **Cộng tác Nhóm**: Giao công việc cho các thành viên trong nhóm và theo dõi chức năng.
- **Bảo mật**: Truy cập an toàn với Xác thực JWT và Kiểm soát truy cập dựa trên vai trò (RBAC).
- **Khả năng Kiểm tra**: Ghi nhật ký hoạt động toàn diện cho các hành động trong hệ thống.

## 🛠 Tech Stack

- **Framework**: ASP.NET Core 8.0 Web API
- **Cơ sở dữ liệu**: Microsoft SQL Server
- **ORM**: Entity Framework Core (Code-First)
- **Xác thực**: JWT (JSON Web Token)
- **Tài liệu**: Swagger / OpenAPI
