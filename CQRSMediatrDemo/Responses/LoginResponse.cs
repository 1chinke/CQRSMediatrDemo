using Demo.Models;
using System.Security.Claims;

namespace Demo.Responses;

public record LoginResponse(Claim[] Claims = null, Kullanici Kullanici = null, string Token = null, int StatusCode = 200, string Error = "");


