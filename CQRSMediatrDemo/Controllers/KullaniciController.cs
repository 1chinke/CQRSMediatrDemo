﻿using Demo.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Demo.Validators.Api;
using FluentValidation.Results;
using Demo.Mediatr.Queries.KullaniciQueries;
using Demo.Mediatr.Commands.KullaniciCommands;


namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KullaniciController : ControllerBase
{
    private readonly IMediator _mediator;
    

    public KullaniciController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<KullaniciController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        //throw new NotImplementedException();        

        var result = await _mediator.Send(new GetAllKullanici());

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };
        
    }

    // GET api/<KullaniciController>/5
    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username)
    {
        var result = await _mediator.Send(new GetKullaniciByUsername(username));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };
    }

    // POST api/<KullaniciController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Kullanici model)
    {
        var errors = Validate(model);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        var result = await _mediator.Send(new InsertKullanici(model));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };

    }

    // PUT api/<KullaniciController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string username, [FromBody] Kullanici model)
    {
        var errors = Validate(model);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        var result = await _mediator.Send(new UpdateKullanici(username, model));

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };        
    }

    // DELETE api/<KullaniciController>/5
    [HttpDelete("{username}")]
    public async Task<IActionResult> Delete(string username)
    {
        var result = await _mediator.Send(new DeleteKullanici(username));


        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(),
            _ => BadRequest(result),
        };
    }

    private List<ValidationFailure> Validate(Kullanici model)
    {
        KullaniciValidator validator = new();

        var validatorResult = validator.Validate(model);

        return validatorResult.Errors;

    }
}
