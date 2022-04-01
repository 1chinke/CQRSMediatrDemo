using Demo.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Demo.Validators.Api;
using Demo.Mediatr.Queries.KullaniciQueries;
using Demo.Mediatr.Commands.KullaniciCommands;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KullaniciController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
       

    public KullaniciController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
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
            404 => NotFound("Kullanıcı bulunamadı."),
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
            404 => NotFound("Kullanıcı bulunamadı."),
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
            404 => NotFound("Kullanıcı bulunamadı."),
            _ => BadRequest(result),
        };

    }

    // PUT api/<KullaniciController>/5
    [HttpPut("{username}")]
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
            404 => NotFound("Kullanıcı bulunamadı."),
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
            404 => NotFound("Kullanıcı bulunamadı."),
            _ => BadRequest(result),
        };
    }

    // GET api/<KullaniciController>/5
    [HttpGet("{username},{password}")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var result = await _mediator.Send(new GetKullaniciByUsernameAndPassword(username, password));

        if (result.StatusCode == 200)
        {
            var kullanici = result.Result;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, kullanici.Username),
                new Claim(ClaimTypes.Role, kullanici.Role),
                new Claim(ClaimTypes.Email, kullanici.EmailAddress),
                new Claim(ClaimTypes.GivenName, kullanici.FirstName),
                new Claim(ClaimTypes.Surname, kullanici.LastName)
            };

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenString);


        }
        else if (result.StatusCode == 404)
        {
            return NotFound("Kullanıcı adı ya da parola hatalı.");
        }
        else
        {
            return BadRequest(result);
        }

    }

    private List<ValidationFailure> Validate(Kullanici model)
    {
        KullaniciValidator validator = new();

        var validatorResult = validator.Validate(model);

        return validatorResult.Errors;

    }
}
