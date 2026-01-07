using System;                                   
using System.Collections.Generic;             
using System.Net;                              
using System.Threading.Tasks;                  
using Microsoft.AspNetCore.Http;               
using Microsoft.Extensions.Logging;            
using TaskFlow.Application.Commons;             
using TaskFlow.Application.Commons.Exceptions;

namespace TaskFlow.API.Middleware
{
    /// <summary>
    /// Middleware để bắt tất cả exception và trả về ApiResponse chuẩn
    /// Middleware là một lớp trung gian xử lý HTTP request/response
    /// Nó sẽ bắt mọi exception xảy ra trong application
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        /// <summary>
        /// RequestDelegate là một function nhận HttpContext và trả về Task
        /// _next đại diện cho middleware tiếp theo trong pipeline
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// ILogger dùng để ghi log thông tin, warning, error
        /// </summary>
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        // ============ CONSTRUCTOR ============
        // Khởi tạo middleware (được gọi 1 lần khi application start)

        public GlobalExceptionMiddleware(
            RequestDelegate next,                              // Middleware tiếp theo
            ILogger<GlobalExceptionMiddleware> logger)         // Logger service
        {
            _next = next;
            _logger = logger;
        }

        // ============ INVOKE METHOD (Phương thức chính của Middleware) ============
        // Được gọi mỗi khi có request đến

        public async Task InvokeAsync(HttpContext context)
        {
            // HttpContext chứa toàn bộ thông tin của request/response

            try
            {
                // *** Gọi middleware tiếp theo trong pipeline ***
                // Nếu middleware tiếp theo không throw exception, code sẽ tiếp tục
                // Nếu throw exception, sẽ jump vào catch block
                await _next(context);

                // Nếu không có lỗi, response được trả về client bình thường
            }
            catch (Exception ex)
            {
                // Bất kỳ exception nào từ middleware sau đó cũng sẽ bị bắt ở đây
                // Ghi log lỗi
                _logger.LogError($"Exception: {ex}");

                // Xử lý exception và trả về response chuẩn
                await HandleExceptionAsync(context, ex);
            }
        }

        // ============ HANDLE EXCEPTION METHOD ============
        // Xử lý exception và tạo response chuẩn

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // HttpContext.Response là response object
            var response = context.Response;

            // Đặt loại dữ liệu trả về là JSON
            response.ContentType = "application/json";

            ApiResponse apiResponse;

            // *** Xử lý Exception dựa trên BaseException ***
            if (exception is BaseException baseEx)
            {
                response.StatusCode = baseEx.StatusCode;
                
                List<string> errors;

                if (baseEx is ValidationException validationEx)
                {
                    errors = validationEx.Errors;
                }
                else 
                {
                    errors = new List<string> { baseEx.Message };
                }

                apiResponse = ApiResponse.CreateFailure(
                    message: baseEx.Message,
                    statusCode: baseEx.StatusCode,
                    errors: errors
                );
            }
            else
            {
                // Các exception hệ thống khác (500)
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                apiResponse = ApiResponse.CreateFailure(
                    message: "Lỗi máy chủ nội bộ",
                    statusCode: 500,
                    errors: new List<string> { "Đã xảy ra lỗi không mong muốn" }
                );
            }

            // *** Viết ApiResponse dưới dạng JSON vào response body ***
            // Client sẽ nhận được JSON này
            return response.WriteAsJsonAsync(apiResponse);
        }
    }
}