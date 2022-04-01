using Demo.Mediatr.Commands.PersonCommands;
using Demo.Repository;
using FluentValidation;

namespace Demo.Validators;

public class InsertPersonCmdValidator : AbstractValidator<InsertPerson>
{
    private readonly IPersonRepo _repo;

    public InsertPersonCmdValidator(IPersonRepo repo)
    {
        _repo = repo;

        RuleFor(p => p.Id).MustAsync(BeUnique).WithMessage("Bu kayıt zaten tanımlı");

    }

    protected async Task<bool> BeUnique(int id, CancellationToken cancellation)
    {
        var result = await _repo.GetById(id);
        return result == null;
    }

    
}
