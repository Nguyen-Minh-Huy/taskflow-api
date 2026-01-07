# Implementation Plan / Kế hoạch Triển khai

This document contains the implementation plan in both English and Vietnamese.
Tài liệu này chứa kế hoạch triển khai bằng cả tiếng Anh và tiếng Việt.

---

## English Version

### Goal Description

Establish a comprehensive foundational architecture including Error Handling, Logging, Response Wrappers, Validation, Mapping, Pagination, and Auditing.

### User Review Required

> [!IMPORTANT]
>
> - **Validation**: Using **FluentValidation** with automatic pipeline integration.
> - **Mapping**: Using **AutoMapper** for Entity-DTO conversion.
> - **Pagination**: Standard `PagedResult<T>` for all list endpoints.
> - **Auditing**: Automatic handling of `CreatedAt`/`UpdatedAt` via `DbContext` interceptor or override.

### Proposed Changes

#### Application Layer (`src/TaskFlow.Application`)

- **[NEW] Commons**:
  - `Commons/ApiResponse.cs`: Standard wrapper `{ success, message, data, errors }`.
  - `Commons/PagedResult.cs`: Wrapper for paginated data `{ items, pageIndex, totalPages, ... }`.
  - `Commons/BaseException.cs`: Custom exceptions.
- **[NEW] Interfaces**:
  - `Interfaces/ICurrentUserService.cs`: Abstraction to access logged-in user data.
  - `Interfaces/IDateTimeProvider.cs`: Abstraction for `DateTime.UtcNow`.
- **[NEW] Behaviors/Filters**:
  - `ValidationBehavior`: (If using MediatR) or Middleware to validate DTOs automatically.
- **[NEW] Mappings**:
  - `Mappings/MappingProfile.cs`: AutoMapper configuration.

#### Infrastructure Layer (`src/TaskFlow.Infrastructure`)

- **[NEW] Services**:
  - `Services/CurrentUserService.cs`: Implementation interacting with `IHttpContextAccessor`.
  - `Services/DateTimeProvider.cs`.
- **[MODIFY] [TaskFlowDbContext.cs](file:///b:/project/taskflow-api/src/TaskFlow.Infrastructure/TaskFlowDbContext.cs)**:
  - Override `SaveChangesAsync` to automatically set `CreatedAt`/`UpdatedAt` based on `IAuditableEntity`.

#### API Layer (`src/TaskFlow.API`)

- **[NEW] Middlewares**:
  - `GlobalExceptionMiddleware.cs`.
- **[MODIFY] [Program.cs](file:///b:/project/taskflow-api/src/TaskFlow.API/Program.cs)**:
  - Register FluentValidation, AutoMapper.
  - Register `GlobalExceptionMiddleware`.
  - Register `CurrentUserService`, `DateTimeProvider`.

### Verification Plan

1.  **Automated Tests**:
    - Test Validation: Send invalid data to `auth/register` and expect 400 with validation errors.
    - Test Auditing: Create an entity and verify `CreatedAt` is set.
2.  **Manual Verification**:
    - Swagger check for Pagination response structure.

---

## Phiên bản Tiếng Việt

### Mô tả Mục tiêu

Thiết lập kiến trúc nền tảng toàn diện bao gồm Xử lý lỗi, Logging, Phản hồi chuẩn, Validate dữ liệu, Mapping, Phân trang và Auditing.

### Yêu cầu Người dùng Xem xét

> [!IMPORTANT]
>
> - **Validation**: Sử dụng **FluentValidation**.
> - **Mapping**: Sử dụng **AutoMapper** để chuyển đổi Entity-DTO.
> - **Phân trang**: Cấu trúc chuẩn `PagedResult<T>` cho các API danh sách.
> - **Auditing**: Tự động cập nhật `CreatedAt`/`UpdatedAt` thông qua `DbContext`.

### Các Thay đổi Đề xuất

#### Lớp Ứng dụng (`src/TaskFlow.Application`)

- **[MỚI] Commons**:
  - `Commons/ApiResponse.cs`: Wrapper chuẩn `{ success, message, data, errors }`.
  - `Commons/PagedResult.cs`: Wrapper cho dữ liệu phân trang.
  - `Commons/BaseException.cs`: Lớp base cho exception.
- **[MỚI] Interfaces**:
  - `Interfaces/ICurrentUserService.cs`: Lấy thông tin user đang đăng nhập.
  - `Interfaces/IDateTimeProvider.cs`: Abstraction cho thời gian.
- **[MỚI] Behaviors/Filters**:
  - Tích hợp FluentValidation để tự động validate DTO.
- **[MỚI] Mappings**:
  - `Mappings/MappingProfile.cs`: Cấu hình AutoMapper.

#### Lớp Cơ sở Hạ tầng (`src/TaskFlow.Infrastructure`)

- **[MỚI] Services**:
  - `Services/CurrentUserService.cs`: Lấy user từ `IHttpContextAccessor`.
  - `Services/DateTimeProvider.cs`.
- **[SỬA] [TaskFlowDbContext.cs](file:///b:/project/taskflow-api/src/TaskFlow.Infrastructure/TaskFlowDbContext.cs)**:
  - Override `SaveChangesAsync` để tự động gán `CreatedAt`/`UpdatedAt`.

#### Lớp API (`src/TaskFlow.API`)

- **[MỚI] Middlewares**:
  - `GlobalExceptionMiddleware.cs`.
- **[SỬA] [Program.cs](file:///b:/project/taskflow-api/src/TaskFlow.API/Program.cs)**:
  - Đăng ký FluentValidation, AutoMapper.
  - Đăng ký Global Exception Middleware.
  - Đăng ký `CurrentUserService`, `DateTimeProvider`.

### Kế hoạch Xác minh

1.  **Kiểm thử Tự động**:
    - Test Validation: Gửi dữ liệu lỗi lên API đăng ký và mong đợi lỗi 400.
    - Test Auditing: Tạo entity và kiểm tra `CreatedAt` tự động được gán.
2.  **Xác minh Thủ công**:
    - Kiểm tra cấu trúc phân trang trên Swagger.
