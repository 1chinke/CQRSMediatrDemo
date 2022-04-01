using Demo.Mediatr.Commands;
using Demo.Repository;
using FluentValidation;

namespace Demo.Validators;

public class InsertPersonCommandValidator : AbstractValidator<InsertPersonCommand>
{
    private readonly IPersonRepo _repo;

    public InsertPersonCommandValidator(IPersonRepo repo)
    {
        _repo = repo;

        /*RuleFor(p => p)
            .MustAsync(async BeUnique)
            .WithMessage("Bu kayıt zaten tanımlı")
            .WithErrorCode("1");*/


        /*RuleFor(p => p.Id).MustAsync(async (id, cancellation) =>
        {
            var old = await _repo.GetPersonById(id);
            return old == null;
        }).WithMessage("Bu kayıt zaten tanımlı");*/

        RuleFor(p => p.Id).MustAsync(BeUnique).WithMessage("Bu kayıt zaten tanımlı");

    }

    protected async Task<bool> BeUnique(int id, CancellationToken cancellation)
    {
        var result = await _repo.GetPersonById(id);
        return result == null;
    }

    
}
