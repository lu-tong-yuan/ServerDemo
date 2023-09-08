using FluentValidation;
using serverDemo.Dtos;

namespace serverDemo.Validators
{
    public class RegisterValidator : AbstractValidator<UserDto>
    {
        public RegisterValidator() 
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("用戶名稱 不能為空")
                .Length(3, 50).WithMessage("用戶名稱長度必須介於 3 到 50 字元之間");

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("電子信箱 不能為空")
                .Length(6, 50).WithMessage("電子信箱長度必須介於 6 到 50 字元之間")
                .EmailAddress().WithMessage("請輸入有效的電子郵件地址");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("密碼 不能為空")
                .Length(3, 255).WithMessage("密碼長度必須介於 0 到 255 字元之間");

            RuleFor(request => request.Role)
                .NotEmpty().WithMessage("身份 不能為空")
                .Must(value => value == "student" || value == "instructor").WithMessage("身份只能填入student或是instructor");

        }
    }
}
