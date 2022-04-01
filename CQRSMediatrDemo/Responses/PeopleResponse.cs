using Demo.Models;

namespace Demo.Responses;

public record PeopleResponse(IEnumerable<PersonModel>? Result = null, int StatusCode = 200, string Error = "");
