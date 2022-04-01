using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Queries.PersonQueries;

public record GetPeople() : IRequest<PeopleResponse>;

