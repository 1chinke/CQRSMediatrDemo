using Demo.Models;

namespace Demo.Responses;

public record PersonResponse(Person Result = null, int StatusCode = 200, string Error = "");
