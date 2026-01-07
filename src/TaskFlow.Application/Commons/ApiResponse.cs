using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.Commons
{
    /// <summary>
    /// Standard API response wrapper for all API endpoints
    /// </summary>
    /// <typeparam name="T">The type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Errors = new List<string>();
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a successful response
        /// </summary>
        public static ApiResponse<T> CreateSuccess(T data, string message = "Success", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a failed response with multiple errors
        /// </summary>
        public static ApiResponse<T> CreateFailure(string message, int statusCode = 400, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        /// <summary>
        /// Creates a failure response with a single error
        /// </summary>
        public static ApiResponse<T> CreateFailure(string message, string error, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = new List<string> { error }
            };
        }
    }

    /// <summary>
    /// Non-generic version for responses without data payload
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Errors = new List<string>();
            Timestamp = DateTime.UtcNow;
        }

        public static ApiResponse CreateSuccess(string message = "Success", int statusCode = 200)
        {
            return new ApiResponse
            {
                Success = true,
                StatusCode = statusCode,
                Message = message
            };
        }

        public static ApiResponse CreateFailure(string message, int statusCode = 400, List<string>? errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
