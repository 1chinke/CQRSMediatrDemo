using Demo.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Demo.Validators.Api;
using FluentValidation.Results;
using Demo.Mediatr.Queries.PersonQueries;
using Demo.Mediatr.Commands.PersonCommands;


namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IMediator _mediator;
    

    public PersonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<PersonController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        //throw new NotImplementedException();        

        var result = await _mediator.Send(new GetPeople());

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound("Kişi bulunamadı."),
            _ => BadRequest(result),
        };
        
    }

    // GET api/<PersonController>/5
    [HttpGet("{id}")]
    //public async Task<PersonModel> Get(int id)
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetPersonById(id));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound("Kişi bulunamadı."),
            _ => BadRequest(result),
        };
    }

    // POST api/<PersonController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Person model)
    {
        var errors = Validate(model);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        var result = await _mediator.Send(new InsertPerson(model.Id, model.FirstName, model.LastName));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound("Kişi bulunamadı."),
            _ => BadRequest(result),
        };

    }

    // PUT api/<PersonController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Person model)
    {
        var errors = Validate(model);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        var result = await _mediator.Send(new UpdatePerson(id, model));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound("Kişi bulunamadı."),
            _ => BadRequest(result),
        };        
    }

    // DELETE api/<PersonController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeletePerson(id));


        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound("Kişi bulunamadı."),
            _ => BadRequest(result),
        };
    }

    private List<ValidationFailure> Validate(Person model)
    {
        PersonValidator validator = new();

        var validatorResult = validator.Validate(model);

        return validatorResult.Errors;

    }
}
