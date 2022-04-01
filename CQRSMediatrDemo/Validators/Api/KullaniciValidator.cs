using Demo.Models;
using FluentValidation;

namespace Demo.Validators.Api;

public class KullaniciValidator : AbstractValidator<Kullanici>
{
    public KullaniciValidator()
    {
        RuleFor(p => p.Username).NotEmpty().WithMessage("{PropertyName} boş geçilemez.");

        RuleFor(p => p.Password).NotEmpty().WithMessage("{PropertyName} boş geçilemez.");

        RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("{PropertyName} boş geçilemez.")
            .EmailAddress().WithMessage("{PropertyValue} geçerli bir e-posta adresi değil.");

        RuleFor(p => p.Role).NotEmpty().WithMessage("{PropertyName} boş geçilemez.");

        RuleFor(p => p.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} boş geçilemez.")
            .Length(2, 20).WithMessage("{PropertyName} uzunluğu ({TotalLength}) hatalı. {MinLength} - {MaxLength} arasında olmalı.")
            .Must(CheckName).WithMessage("{PropertyName} içerisinde sadece harf olmalı.");

        RuleFor(p => p.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} boş geçilemez.")
            .Length(2, 20).WithMessage("{PropertyName} uzunluğu ({TotalLength}) hatalı. {MinLength} - {MaxLength} arasında olmalı.")
            .Must(CheckName).WithMessage("{PropertyName} içerisinde sadece harf olmalı.");
    }

    protected bool CheckName(string name)
    {
        name = name
            .Replace(" ", "")
            .Replace(".", "");

        return name.All(Char.IsLetter);
    }

}
