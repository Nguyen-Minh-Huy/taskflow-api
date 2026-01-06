# Thiết kế Cơ sở Dữ liệu

## 1. Tổng quan

Cơ sở dữ liệu sử dụng **Microsoft SQL Server**. Lược đồ được chuẩn hóa (3NF) để đảm bảo tính toàn vẹn dữ liệu.

## 2. Sơ đồ ER (Conceptual)

```mermaid
erDiagram
    Users ||--o{ TeamMembers : "belongs to"
    Teams ||--o{ TeamMembers : "has"
    Teams ||--o{ Projects : "owns"
    Projects ||--o{ Tasks : "contains"
    Users ||--o{ Tasks : "assigned to"
    Users ||--o{ Comments : "writes"
    Tasks ||--o{ Comments : "has"
    Tasks ||--o{ FileAttachments : "has"
    Users ||--o{ ActivitiesLog : "performs"

    Users {
        Guid Id
        string Email
        string PasswordHash
        string DisplayName
        string PhoneNumber
        string SystemRole
        datetime CreatedAt
        Guid? CreatedBy
        datetime UpdatedAt
        Guid? UpdatedBy
    }

    Teams {
        Guid Id
        string Name
        string Description
        Guid OwnerId
        datetime CreatedAt
        Guid? CreatedBy
        datetime UpdatedAt
        Guid? UpdatedBy
    }

    Projects {
        Guid Id
        string Name
        string Description
        datetime StartDate
        datetime DueDate
        Guid TeamId
        datetime CreatedAt
        Guid? CreatedBy
        datetime UpdatedAt
        Guid? UpdatedBy
    }

    Tasks {
        Guid Id
        string Title
        string Description
        int Status
        int Priority
        datetime StartDate
        datetime DueDate
        Guid ProjectId
        Guid? AssignedUserId
        datetime CreatedAt
        Guid? CreatedBy
        datetime UpdatedAt
        Guid? UpdatedBy
    }

    Comments {
        Guid Id
        string Content
        Guid TaskId
        Guid UserId
        datetime CreatedAt
        Guid? CreatedBy
        datetime UpdatedAt
        Guid? UpdatedBy
    }

    FileAttachments {
        Guid Id
        string FileName
        string FilePath
        long FileSize
        string ContentType
        Guid TaskId
        Guid UploadedByUserId
        datetime CreatedAt
        Guid? CreatedBy
        datetime UpdatedAt
        Guid? UpdatedBy
    }

    ActivitiesLog {
        Guid Id
        string Action
        string EntityType
        Guid EntityId
        string Details
        Guid UserId
        datetime CreatedAt
    }
```

## 3. Định nghĩa Bảng

### Các Trường Audit Chung

Tất cả các bảng (ngoại trừ các bảng liên kết chặt chẽ nếu không được ghi nhật ký) bao gồm:

- `CreatedAt` (DATETIME2)
- `CreatedBy` (GUID, Nullable)
- `UpdatedAt` (DATETIME2, Nullable)
- `UpdatedBy` (GUID, Nullable)

### 3.1 `Users`

Lưu trữ thông tin xác thực và hồ sơ.

- `Id` (PK, GUID)
- `Email` (NVARCHAR(255), Duy nhất)
- `PhoneNumber` (NVARCHAR(20), Nullable)
- `PasswordHash` (NVARCHAR(MAX))
- `DisplayName` (NVARCHAR(100))
- `SystemRole` (NVARCHAR(50)) - ví dụ: 'SystemAdmin', 'User'

### 3.2 `Teams`

Đại diện cho một nhóm người dùng làm việc cùng nhau.

- `Id` (PK, GUID)
- `Name` (NVARCHAR(100))
- `Description` (NVARCHAR(500))
- `OwnerId` (FK -> Users.Id)

### 3.3 `TeamMembers`

Bảng liên kết Nhiều-Nhiều giữa Users và Teams.

- `TeamId` (FK)
- `UserId` (FK)
- `Role` (Enum: Admin, Member)

### 3.4 `Projects`

Chứa các công việc, thuộc về một nhóm.

- `Id` (PK, GUID)
- `Name` (NVARCHAR(150))
- `Description` (NVARCHAR(MAX))
- `StartDate` (DATETIME2, Nullable)
- `DueDate` (DATETIME2, Nullable)
- `TeamId` (FK -> Teams.Id)

### 3.5 `Tasks`

Các đơn vị công việc riêng lẻ.

- `Id` (PK, GUID)
- `Title` (NVARCHAR(200))
- `Description` (NVARCHAR(MAX))
- `Status` (Enum: Todo, InProgress, Review, Done)
- `Priority` (Enum: Low, Medium, High, Urgent)
- `StartDate` (DATETIME2, Nullable)
- `DueDate` (DATETIME2, Nullable)
- `ProjectId` (FK -> Projects.Id)
- `AssignedUserId` (FK -> Users.Id, Nullable)

### 3.6 `Comments`

Các luồng thảo luận về công việc.

- `Id` (PK, GUID)
- `Content` (NVARCHAR(MAX))
- `TaskId` (FK -> Tasks.Id)
- `UserId` (FK -> Users.Id)

### 3.7 `FileAttachments`

Các tệp đính kèm vào công việc.

- `Id` (PK, GUID)
- `FileName` (NVARCHAR(255))
- `FilePath` (NVARCHAR(MAX)) - URL hoặc đường dẫn lưu trữ
- `FileSize` (BIGINT)
- `ContentType` (NVARCHAR(100))
- `TaskId` (FK -> Tasks.Id)
- `UploadedByUserId` (FK -> Users.Id)

### 3.8 `ActivitiesLog`

Nhật ký kiểm tra (audit log) cho các hành động hệ thống.

- `Id` (PK, GUID)
- `Action` (NVARCHAR(50)) - ví dụ: 'Create', 'Update', 'Delete'
- `EntityType` (NVARCHAR(50)) - ví dụ: 'Task', 'Project'
- `EntityId` (GUID)
- `Details` (NVARCHAR(MAX)) - JSON hoặc mô tả văn bản về thay đổi
- `UserId` (FK -> Users.Id)

## 4. Chiến lược Đánh chỉ mục (Indexing)

- **Users**: Index trên `Email` và `PhoneNumber`.
- **Tasks**: Index trên `ProjectId`, `AssignedUserId`.
- **ActivitiesLog**: Index trên `EntityId`, `UserId`, `CreatedAt`.
- **Projects**: Index trên `TeamId`.
