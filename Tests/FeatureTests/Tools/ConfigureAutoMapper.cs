using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Model;
using EF_Spike.Shared.Model;

namespace FeatureTests.Tools
{
    public class ConfigureAutoMapper
    {
        public void Configure()
        {
            AutoMapper.Mapper.Initialize(x =>
            {
                x.CreateMap<TblMembership, EF_Spike.Membership.Model.Membership>();
                x.CreateMap<EF_Spike.Membership.Model.Membership, TblMembership>();

                x.CreateMap<TblMembershipType, MembershipType>();
                x.CreateMap<MembershipType, TblMembershipType>();

                x.CreateMap<TblMembershipDetails, MembershipDetails>();
                x.CreateMap<MembershipDetails, TblMembershipDetails>();

                x.CreateMap<TblMembershipBenefitType, MembershipBenefitType>();
                x.CreateMap<MembershipBenefitType, TblMembershipBenefitType>();

                x.CreateMap<TblMembershipAverageAgeBasisType, MembershipAverageAgeBasisType>();
                x.CreateMap<MembershipAverageAgeBasisType, TblMembershipAverageAgeBasisType>();

                x.CreateMap<TblMembershipAverageAgeBasis, MembershipAverageAgeBasiss>();
                x.CreateMap<MembershipAverageAgeBasiss, TblMembershipAverageAgeBasis>();

                x.CreateMap<Event, TblEvent>();
                x.CreateMap<TblEvent, Event>();

                x.CreateMap<EventSource, TblEventSource>();
                x.CreateMap<TblEventSource, EventSource>();

                x.CreateMap<EventTypee, TblEventType>();
                x.CreateMap<TblEventType, EventTypee>();

                x.CreateMap<EventTypeGroup, TblEventTypeGroup>();
                x.CreateMap<TblEventTypeGroup, EventTypeGroup>();
            });
        }
    }
}