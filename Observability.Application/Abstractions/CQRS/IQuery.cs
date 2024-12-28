using MediatR;
using Observability.Shared;

namespace Observability.Application.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;