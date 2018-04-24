using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF_Spike.Membership.Handler
{
    public class PostMembershipSprocHandler : IRequestHandler<PostMembershipSproc, Model.Membership>
    {
        private readonly RegistryContext context;
        private readonly IMediator mediator;

        public PostMembershipSprocHandler(RegistryContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<Model.Membership> Handle(PostMembershipSproc request, CancellationToken cancellationToken)
        {
            var memberToAdd = AutoMapper.Mapper.Map<Model.Membership, TblMembership>(request.Membership);

            //var paramss = new SqlParameter(memberToAdd.Psrnumber, memberToAdd.SectionNumber);
            var parameters = new object[]
                {
                    request.Event.UserId,
                    request.Membership.Psrnumber,
                    request.Membership.SectionNumber,
                    request.Membership.LevyTagTypeReference,
                    request.Membership.EffectiveDate,
                    request.Event.NotificationDate,
                    request.Membership.MembershipReference,
                    request.Membership.TblMembershipDetails.First().NumberOfMembers,
                    request.Membership.TblMembershipDetails.First().AverageAgeOfMembers,
                    request.Membership.AgeProfiling50to59,
                    request.Membership.AgeProfiling60Plus
                };

            var sql = "usp_Create_Membership " +
                      $"@CreatedBy = '{request.Event.UserId}'" +
                      $", @PSRNumber = {request.Membership.Psrnumber}" +
                      $", @SectionNumber = {request.Membership.SectionNumber}" +
                      $", @LevyTagTypeReference = {request.Membership.LevyTagTypeReference}" +
                      $", @EffectiveDate = '{request.Membership.EffectiveDate.ToString("s")}'" +
                      $", @NotificationDate = '{request.Event.NotificationDate.Value.ToString("s")}'" +
                      $", @MembershipReference = {request.Membership.MembershipReference}" +
                      $", @WholeMembership = {request.Membership.TblMembershipDetails.First().NumberOfMembers}";

            await context.Database.ExecuteSqlCommandAsync(sql, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return request.Membership;
        }
    }
}