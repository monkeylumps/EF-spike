using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var result = await next();
                    transaction.Commit();

                    return result;
                }
            });
        }
    }
}