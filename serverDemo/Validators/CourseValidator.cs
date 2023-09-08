using FluentValidation;
using serverDemo.Dtos;

namespace serverDemo.Validators
{
    public class CourseValidator : AbstractValidator<CourseDto>
    {
        public CourseValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage("課程標題 不能為空")
                .Length(6, 50).WithMessage("課程標題長度必須介於 6 到 50 字元之間");

            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("內容 不能為空")
                .Length(6, 50).WithMessage("內容長度必須介於 6 到 50 字元之間");

            RuleFor(request => int.Parse(request.Price))
                .NotEmpty().WithMessage("金額 不能為空")
                .InclusiveBetween(10, 9999).WithMessage("金額必須介於 10 到 9999 之間");
        }
    }
}
