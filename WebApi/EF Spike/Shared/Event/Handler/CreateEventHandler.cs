using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;

namespace EF_Spike.Shared.Handler
{
    public class CreateEventHandler : IRequestHandler<CreateEvent, int>
    {
        private readonly RegistryContext context;

        public CreateEventHandler(RegistryContext context)
        {
            this.context = context;
        }

        public async Task<int> Handle(CreateEvent request, CancellationToken cancellationToken)
        {
            var tblEvent = AutoMapper.Mapper.Map<Model.Event, TblEvent>(request.Event);

            await context.AddAsync(tblEvent, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return tblEvent.EventReference;
        }
    }
}