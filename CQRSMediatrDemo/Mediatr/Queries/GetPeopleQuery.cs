using Demo.Responses;
using MediatR;

namespace Demo.Mediatr.Queries;

public record GetPeopleQuery() : IRequest<PeopleResponse>;

