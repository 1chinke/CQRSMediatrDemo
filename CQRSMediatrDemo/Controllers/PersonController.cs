using Demo.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Demo.Validators.Api;
using FluentValidation.Results;
using Demo.Mediatr.Queries;
using Demo.Mediatr.Commands;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        throw new NotImplementedException();
        /*

        var result = await _mediator.Send(new GetPeopleQuery());

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };
        */
    }

    // GET api/<PersonController>/5
    [HttpGet("{id}")]
    //public async Task<PersonModel> Get(int id)
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetPersonByIdQuery(id));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };
    }

    // POST api/<PersonController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PersonModel model)
    {
        /*var errors = Validate(model);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }*/

        var result = await _mediator.Send(new InsertPersonCommand(model.Id, model.FirstName, model.LastName));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };

    }

    // PUT api/<PersonController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PersonModel model)
    {
        /*var errors = Validate(model);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }*/

        var result = await _mediator.Send(new UpdatePersonCommand(id, model));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };        
    }

    // DELETE api/<PersonController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeletePersonCommand(id));


        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };
    }

    private List<ValidationFailure> Validate(PersonModel model)
    {
        PersonValidator validator = new();

        var validatorResult = validator.Validate(model);

        return validatorResult.Errors;

    }
}
