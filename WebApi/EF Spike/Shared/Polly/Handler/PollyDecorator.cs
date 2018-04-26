using System;
using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;
using Polly;

namespace EF_Spike.Shared.Polly.Handler
{
    public class PollyDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly RegistryContext context;

        public PollyDecorator(RegistryContext context)
        {
            this.context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(6, // We can also do this with WaitAndRetryForever... but chose WaitAndRetry this time.
                    attempt => TimeSpan.FromSeconds(0.1 * Math.Pow(2, attempt)), // Back off!  2, 4, 8, 16 etc times 1/4-second
                    (exception, calculatedWaitDuration) =>  // Capture some info for logging!
                    {});

            var thing = await  policy.ExecuteAndCaptureAsync(async () =>
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var result = await next();

                    transaction.Commit();

                    return result;
                }
            });

            return thing.Result;
        }
    }
}