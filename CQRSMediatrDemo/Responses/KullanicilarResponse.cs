using Demo.Models;

namespace Demo.Responses;

public record KullanicilarResponse(IEnumerable<Kullanici> Result = null, int StatusCode = 200, string Error = "");

