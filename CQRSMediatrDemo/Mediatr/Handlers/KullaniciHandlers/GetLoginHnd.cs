using MediatR;
using Demo.Mediatr.Queries.KullaniciQueries;
using Demo.Repository;
using Demo.Responses;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace Demo.Mediatr.Handlers.KullaniciHandlers;

public class GetLoginHnd : IRequestHandler<GetLogin, LoginResponse>
{
    private readonly IKullaniciRepo _repo;
    private readonly IConfiguration _config;

    public GetLoginHnd(IKullaniciRepo repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<LoginResponse> Handle(GetLogin request, CancellationToken cancellationToken)
    {
        try
        {
            var passwordHash = CreatePasswordHash(request.Password);
            var kullanici = await _repo.Login(request.Username, passwordHash);

            if (kullanici == null)
            {
                return new LoginResponse(StatusCode: 404, Error: "Kullanıcı ya da parola hatalı");
            }

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
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse(claims, kullanici, tokenString, 200);
        }
        catch (Exception ex)
        {
            return new LoginResponse(StatusCode: 400, Error: ex.Message);
        }

    }

    private string CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        hmac.Key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);


        //return Encoding.UTF8.GetString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}

