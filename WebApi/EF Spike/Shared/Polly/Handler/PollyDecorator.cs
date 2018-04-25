using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF_Spike.Shared.Polly.Handler
{
    public class PollyDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> requestHandler;
        private readonly RegistryContext context;

        public PollyDecorator(IRequestHandler<TRequest, TResponse> requestHandler, RegistryContext context)
        {
            this.requestHandler = requestHandler;
            this.context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.Execute(() => requestHandler.Handle(request, cancellationToken));
        }
    }
}