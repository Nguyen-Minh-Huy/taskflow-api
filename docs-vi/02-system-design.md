# Thiết kế Hệ thống & Kiến trúc

## 1. Kiến trúc Cấp cao

TaskFlow API tuân theo các nguyên tắc **Clean Architecture** (còn được gọi là Kiến trúc Củ hành) để đảm bảo sự phân tách các mối quan tâm, khả năng kiểm tra và sự độc lập với các framework.

## 2. Phân tích Các lớp

### 2.1 Lớp Domain (`TaskFlow.Domain`)

**Trách nhiệm**: Cốt lõi của hệ thống. Chứa logic nghiệp vụ và các thực thể.
**Phụ thuộc**: Không.
**Nội dung**:

- **Thực thể**: Các đối tượng nghiệp vụ cốt lõi (ví dụ: `User`, `Project`, `Task`).
- **Value Objects**: Các đối tượng bất biến (ví dụ: `Email`, `Priority`).
- **Enums**: Các hằng số như `TaskStatus`, `UserRole`.
- **Giao diện**: Các giao diện Repository (`IUnitOfWork`, `IGenericRepository`) và các giao diện dịch vụ domain.

### 2.2 Lớp Application (`TaskFlow.Application`)

**Trách nhiệm**: Điều phối logic ứng dụng và các trường hợp sử dụng (use cases).
**Phụ thuộc**: Lớp Domain.
**Nội dung**:

- **DTOs (Data Transfer Objects)**: Các mô hình cho đầu vào/đầu ra tới API.
- **Giao diện**: Các trừu tượng hóa dịch vụ (`ITaskService`, `IAuthService`).
- **Dịch vụ/Handlers**: Triển khai các quy tắc nghiệp vụ (ví dụ: "Giao Công việc", "Tạo Dự án").
- **Validators**: Các quy tắc FluentValidation cho các yêu cầu đến.
- **AutoMapper Profiles**: Cấu hình ánh xạ giữa các Thực thể và DTO.

### 2.3 Lớp Infrastructure (`TaskFlow.Infrastructure`)

**Trách nhiệm**: Triển khai các giao diện được định nghĩa trong các lớp cốt lõi. Tương tác với các hệ thống bên ngoài.
**Phụ thuộc**: Lớp Application, Lớp Domain.
**Nội dung**:

- **Truy cập Dữ liệu**: Triển khai Entity Framework Core `DbContext` và Repository.
- **Migrations**: Các định nghĩa lược đồ cơ sở dữ liệu.
- **Dịch vụ**: Triển khai các dịch vụ bên ngoài (ví dụ: EmailService, FileStorage).

### 2.4 Lớp API (`TaskFlow.API`)

**Trách nhiệm**: Điểm vào cho ứng dụng. Xử lý các yêu cầu HTTP.
**Phụ thuộc**: Lớp Application, Lớp Infrastructure.
**Nội dung**:

- **Controllers**: Các điểm cuối RESTful.
- **Middleware**: Xử lý lỗi toàn cục, ghi nhật ký, xác thực.
- **Dependency Injection**: Cấu hình gốc thành phần (`Program.cs`).
- **Swagger**: Cấu hình tài liệu API.

## 3. Các Quyết định Thiết kế Chính

- **Mẫu CQRS**: (Tùy chọn/Dự kiến) Tách biệt trách nhiệm Lệnh (Command) và Truy vấn (Query) để có khả năng mở rộng.
- **Dependency Injection**: Sử dụng rộng rãi .NET Core DI container.
- **Mẫu Repository**: Trừu tượng hóa trên EF Core để tách biệt logic truy cập dữ liệu.
