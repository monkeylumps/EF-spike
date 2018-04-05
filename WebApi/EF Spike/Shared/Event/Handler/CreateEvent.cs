using MediatR;

namespace EF_Spike.Shared.Handler
{
    public class CreateEvent : IRequest<int>
    {
        public Model.Event Event { get; set; }
    }
}