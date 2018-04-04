using System;
using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Controller;
using FeatureTests.Tools;
using MediatR;
using Xunit;

namespace FeatureTests.Membership
{
    public class MembershipTests
    {
        private readonly MembershipController sut;

        private readonly ObjectResultResolver resolver;

        private const int psr = 10000005;

        public MembershipTests()
        {
            var ioc = new ConfigureIOC();
            var container = ioc.Configure();

            sut = new MembershipController(container.GetInstance<IMediator>());

            resolver = new ObjectResultResolver();

            var databaseBuilder = new InMemoryDatabaseBuilder();

            AutoMapper.Mapper.Reset();

            AutoMapper.Mapper.Initialize(x =>
            {
                x.CreateMap<TblMembership, EF_Spike.Membership.Model.Membership>();
                x.CreateMap<EF_Spike.Membership.Model.Membership, TblMembership>();
            });

            var registryContext = container.GetInstance<RegistryContext>();

            registryContext.Database.EnsureDeleted();

            databaseBuilder.AddEntityToDb(CreateTblMembership(psr), registryContext);
        }

        [Fact]
        public async void GetMembershipIfValidPsr()
        {
            // Arrange
            var expected = CreateMembership(psr);

            // Act
            var result = await sut.Get(psr);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);
        }

        [Fact]
        public async void GetMembershipIfNoPsrMatch()
        {
            // Arrange
            var expected = new EF_Spike.Membership.Model.Membership();

            // Act
            var result = await sut.Get(10000006);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);
        }

        [Fact]
        public async void PostMembershipIfPsrMatches()
        {
            // Arrange
            var expected = CreateMembership(psr);

            expected.MembershipReference = 2;

            // Act
            var result = await sut.Post(expected);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);
        }

        private TblMembership CreateTblMembership(int psr)
        {
            var result = CreateMembership(psr);
            return AutoMapper.Mapper.Map<EF_Spike.Membership.Model.Membership, TblMembership>(result);
        }

        private EF_Spike.Membership.Model.Membership CreateMembership(int psr)
        {
            return new EF_Spike.Membership.Model.Membership
            {
                Psrnumber = psr,
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
        }
    }
}