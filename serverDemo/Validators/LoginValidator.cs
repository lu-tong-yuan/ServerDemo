using FluentValidation;
using serverDemo.Dtos;

namespace serverDemo.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator() 
        {
            RuleFor(request => request.Email)
               .Length(6, 50).WithMessage("電子信箱長度必須介於 6 到 50 字元之間")
               .NotEmpty().WithMessage("電子信箱 不能為空");

            RuleFor(request => request.Password)
                .Length(3, 255).WithMessage("密碼長度必須介於 0 到 255 字元之間")
                .NotEmpty().WithMessage("密碼 不能為空");
        }
    }
}
