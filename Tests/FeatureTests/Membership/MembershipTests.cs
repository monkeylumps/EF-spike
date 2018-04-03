using System;
using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Controller;
using EF_Spike.Membership.Handler;
using FeatureTests.Tools;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FeatureTests.Membership
{
    public class MembershipTests
    {
        private readonly MembershipController sut;

        private readonly IGetMembership getMembership;
        private readonly RegistryContext registryContext;
        //private readonly DbContextOptionsBuilder<RegistryContext> optionsBuilder;

        private readonly OkRequestResolver resolver;

        public MembershipTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<RegistryContext>()
                .UseInMemoryDatabase("MembershipTests")
                .Options;

            registryContext = new RegistryContext(optionsBuilder);
            getMembership = new GetMembership(registryContext);
            sut = new MembershipController(getMembership);

            resolver = new OkRequestResolver();

            AutoMapper.Mapper.Initialize(x =>
            {
                x.CreateMap<TblMembership, EF_Spike.Membership.Model.Membership>();
                x.CreateMap<EF_Spike.Membership.Model.Membership, TblMembership>();
            });
        }

        [Fact]
        public void GetMembershipIfValidPsr()
        {
            // Arrange
            var psr = 10000005;

            var expected = new EF_Spike.Membership.Model.Membership
            {
                Psrnumber = 10000005,
                MembershipReference = 1,
                SectionNumber = 0,
                LevyTagTypeReference = 2,
                EffectiveDate = Convert.ToDateTime("1996-03-31T00:00:00"),
                EndDate = null,
                EndEventReference = null,
                AgeProfiling50to59 = null,
                AgeProfiling60Plus = null,
                StartEventReference = 5
            };

            var tblMembership = AutoMapper.Mapper.Map<TblMembership>(expected);

            registryContext.Add(tblMembership);

            // Act
            var result = sut.Get(psr);

            var resolvedResult = resolver.GetOkResult(expected, result);

            // Assert
            Assert.True(resolvedResult.isOkResult);
            Assert.Equal(resolvedResult.expected, resolvedResult.result);
        }
    }
}