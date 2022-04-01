using Demo.Models;

namespace Demo.Responses;

public record PersonResponse(PersonModel? Result = null, int StatusCode = 200, string Error = "");
