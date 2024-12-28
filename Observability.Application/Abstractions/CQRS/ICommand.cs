using MediatR;
using Observability.Shared;

namespace Observability.Application.Abstractions.CQRS;

public interface ICommand : IRequest<Result>, IBaseCommand;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
public interface IBaseCommand;