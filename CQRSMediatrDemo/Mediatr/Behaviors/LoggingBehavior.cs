using MediatR;
using System.Diagnostics;

namespace Demo.Mediatr.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;

    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        //pre logic

        _logger.LogInformation("{Request} is starting", request.GetType().Name);

        var timer = Stopwatch.StartNew();

        //main event

        var response = await next();

        //post logic
        
        timer.Stop();
        
        _logger.LogInformation("{Request} has finished in {Time} ms.", request.GetType().Name, timer.ElapsedMilliseconds);

        return response;
    }
}
