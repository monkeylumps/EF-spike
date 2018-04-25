using MediatR;

namespace EF_Spike.Shared.Polly.Handler
{
    public class Polly<T, P> : IRequest<T>
    {
        public P Message { get; set; }
    }
}