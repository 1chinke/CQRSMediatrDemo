using Demo.Models;

namespace Demo.Responses;

public record KullaniciResponse(Kullanici Result = null, int StatusCode = 200, string Error = "");

