using System;
using System.Collections.Generic;
using System.Linq;
using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Controller;
using EF_Spike.Membership.Model;
using FeatureTests.Tools;
using MediatR;
using Xunit;

namespace FeatureTests.Membership
{
    public class MembershipTests
    {
        private readonly MembershipController sut;

        private readonly RegistryContext registryContext;

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

            var automapper = new ConfigureAutoMapper();

            automapper.Configure();

            registryContext = container.GetInstance<RegistryContext>();

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
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 2;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 2;

            // Act
            var result = await sut.Post(expected);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);

            var events = registryContext.TblMembership.Where(x => x.EndEventReference != null);

            Assert.NotNull(events);
        }

        [Fact]
        public async void PostMembershipIfTransationFails()
        {
            // Arrange
            var expected = CreateMembership(psr);

            expected.MembershipReference = 1;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 1;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 1;

            // Act
            var result = await sut.Post(expected);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);

            var events = registryContext.TblMembership.Where(x => x.EndEventReference != null);

            Assert.NotNull(events);
        }

        [Fact]
        public async void GetNotApplicableIfPsrMatches()
        {
            // Arrange
            var expected = CreateMembership(psr);

            expected.MembershipReference = 2;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 2;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipAverageAgeBasis = 3;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 2;

            await sut.Post(expected);

            // Act
            var result = await sut.GetNotApplicable(psr);

            var resolvedResult = resolver.GetObjectResult(new List<EF_Spike.Membership.Model.Membership>{expected}, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
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
                StartEventReference = 5,
                TblMembershipDetails = new List<MembershipDetails> { CreateMembershipDetails() },
                TblMembershipAverageAgeBasis = new List<MembershipAverageAgeBasiss> { CreateMembershipAverageAgeBasis() }
            };
        }

        private MembershipDetails CreateMembershipDetails()
        {
           return new MembershipDetails
           {
               MembershipReference = 1,
               MembershipBenefitTypeReference = 1,
               MembershipTypeReference = 1,
               NumberOfMembers = 10,
               NumberOfExcludedMembers = null,
               AverageAgeOfMembers = null,
           };
        }

        private MembershipAverageAgeBasiss CreateMembershipAverageAgeBasis()
        {
            return new MembershipAverageAgeBasiss
            {
                MembershipReference = 1,
                MembershipAverageAgeBasis = 2,
                StartEventReference = 1
            };
        }
    }
}