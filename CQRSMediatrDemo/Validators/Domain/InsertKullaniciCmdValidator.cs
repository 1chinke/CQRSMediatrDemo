using Demo.Mediatr.Commands.KullaniciCommands;
using Demo.Repository;
using FluentValidation;

namespace Demo.Validators;

public class InsertKullaniciCmdValidator : AbstractValidator<InsertKullanici>
{
    private readonly IKullaniciRepo _repo;

    public InsertKullaniciCmdValidator(IKullaniciRepo repo)
    {
        _repo = repo;

        RuleFor(p => p.Model.Username).MustAsync(BeUnique).WithMessage("Bu kayıt zaten tanımlı.");
    }

    protected async Task<bool> BeUnique(string username, CancellationToken cancellation)
    {
        var result = await _repo.GetByUsername(username);
        return result == null;
    }


}
