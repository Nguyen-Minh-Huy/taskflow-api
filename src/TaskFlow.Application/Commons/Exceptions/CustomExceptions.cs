using System;
using System.Collections.Generic;

namespace TaskFlow.Application.Commons.Exceptions
{
    /// <summary>
    /// ValidationException - Lỗi validation dữ liệu (400)
    /// </summary>
    public class ValidationException : BaseException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ValidationException(string message, List<string>? errors = null)
            : base(message, 400)
        {
            Errors = errors ?? new List<string> { message };
        }
    }

    /// <summary>
    /// NotFoundException - Resource không tìm thấy (404)
    /// </summary>
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, 404) { }
    }

    /// <summary>
    /// UnauthorizedException - User chưa đăng nhập (401)
    /// </summary>
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(message, 401) { }
    }

    /// <summary>
    /// ForbiddenException - User không có quyền (403)
    /// </summary>
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message) : base(message, 403) { }
    }

    /// <summary>
    /// BusinessException - Vi phạm business logic (400)
    /// </summary>
    public class BusinessException : BaseException
    {
        public BusinessException(string message) : base(message, 400) { }
    }
}
