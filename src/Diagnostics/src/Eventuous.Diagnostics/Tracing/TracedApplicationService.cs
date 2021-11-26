using System.Diagnostics;

namespace Eventuous.Diagnostics.Tracing;

public class TracedApplicationService<T> : IApplicationService<T> where T : Aggregate {
    public static IApplicationService<T> Trace(IApplicationService<T> appService)
        => new TracedApplicationService<T>(appService);

    IApplicationService<T> Inner { get; }

    TracedApplicationService(IApplicationService<T> appService) => Inner = appService;

    public async Task<Result> Handle<TCommand>(
        TCommand          command,
        CancellationToken cancellationToken
    ) where TCommand : class {
        using var activity = EventuousDiagnostics.ActivitySource.CreateActivity(
            Constants.HandleCommand,
            ActivityKind.Internal,
            parentContext: default,
            idFormat: ActivityIdFormat.W3C
        )?
            .SetTag(Constants.CommandTag, typeof(TCommand).Name)
            .Start();

        try {
            var result = await Inner.Handle(command, cancellationToken).NoContext();

            if (result is ErrorResult error)
                activity?.SetException(error.Exception);

            return result;
        }
        catch (Exception e) {
            activity?.SetException(e);
            throw;
        }
    }
}

public class TracedApplicationService<T, TState, TId> : IApplicationService<T, TState, TId>
    where TState : AggregateState<TState, TId>, new() 
    where TId : AggregateId 
    where T : Aggregate<TState, TId> {
    public static IApplicationService<T, TState, TId> Trace(IApplicationService<T, TState, TId> appService)
        => new TracedApplicationService<T, TState, TId>(appService);

    IApplicationService<T, TState, TId> Inner { get; }

    TracedApplicationService(IApplicationService<T, TState, TId> appService) => Inner = appService;

    public async Task<Result<TState, TId>> Handle<TCommand>(
        TCommand          command,
        CancellationToken cancellationToken
    ) where TCommand : class {
        using var activity = EventuousDiagnostics.ActivitySource.CreateActivity(
            Constants.HandleCommand,
            ActivityKind.Internal,
            parentContext: default,
            idFormat: ActivityIdFormat.W3C
        )?.Start();

        try {
            var result = await Inner.Handle(command, cancellationToken).NoContext();

            if (result is ErrorResult<TState, TId> error)
                activity?.SetException(error.Exception);

            return result;
        }
        catch (Exception e) {
            activity?.SetException(e);
            throw;
        }
    }
}