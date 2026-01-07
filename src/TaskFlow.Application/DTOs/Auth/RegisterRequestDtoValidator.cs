using FluentValidation;
namespace TaskFlow.Application.DTOs.Auth;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(255).WithMessage("Email tối đa 255 ký tự");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu là bắt buộc")
            .MinimumLength(6).WithMessage("Mật khẩu tối thiểu 6 ký tự")
            .Matches(@"[A-Z]").WithMessage("Mật khẩu phải có ít nhất 1 chữ hoa")
            .Matches(@"[a-z]").WithMessage("Mật khẩu phải có ít nhất 1 chữ thường")
            .Matches(@"[0-9]").WithMessage("Mật khẩu phải có ít nhất 1 chữ số");
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Tên hiển thị là bắt buộc")
            .MaximumLength(100).WithMessage("Tên hiển thị tối đa 100 ký tự");
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Số điện thoại tối đa 20 ký tự")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}