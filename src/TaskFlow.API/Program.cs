using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TaskFlow.Application.Commons;
using TaskFlow.API.Middleware;
using TaskFlow.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

// Đăng ký Auth Services
builder.Services.AddScoped<TaskFlow.Application.Interfaces.IAuthService, TaskFlow.Infrastructure.Services.AuthService>();
builder.Services.AddScoped<TaskFlow.Application.Interfaces.ITokenService, TaskFlow.Infrastructure.Services.TokenService>();

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
});

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

// register DbContext
builder.Services.AddDbContext<TaskFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
