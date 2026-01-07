using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TaskFlow.Application.Commons;
using TaskFlow.API.Middleware;
using TaskFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Tùy chỉnh phản hồi lỗi mặc định của [ApiController]
        options.InvalidModelStateResponseFactory = context =>
        {
            // Lấy danh sách lỗi từ ModelState
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var response = TaskFlow.Application.Commons.ApiResponse<object>.CreateFailure(
                message: "Dữ liệu đầu vào không hợp lệ",
                statusCode: 400,
                errors: errors
            );

            return new BadRequestObjectResult(response);
        };
    });

// Đăng ký AutoMapper (kiểu mới)
// Đăng ký MappingProfile của Application layer
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<TaskFlow.Application.Mappings.MappingProfile>();
});

// Đăng ký FluentValidation
// Quét toàn bộ Assembly của Application layer để tìm các Validator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TaskFlow.Application.Mappings.MappingProfile>();

// Đăng ký Infrastructure Services qua extension method
builder.Services.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TaskFlow API",
        Version = "v1",
        Description = "TaskFlow API documentation"
    });
});

var app = builder.Build();

// Enable Swagger UI only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskFlow API V1");
    });
}

// Global Exception Handler - Phải đặt sớm trong pipeline
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
