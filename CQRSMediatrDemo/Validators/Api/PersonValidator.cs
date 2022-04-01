using Demo.Models;
using FluentValidation;

namespace Demo.Validators.Api;

public class PersonValidator : AbstractValidator<PersonModel>
{
    public PersonValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("{PropertyName} 0 ya da null olamaz");

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
