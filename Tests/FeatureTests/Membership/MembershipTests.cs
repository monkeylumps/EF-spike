using System;
using System.Collections.Generic;
using System.Linq;
using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Controller;
using EF_Spike.Membership.Model;
using FeatureTests.Tools;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FeatureTests.Membership
{
    public class MembershipTests : IDisposable
    {
        private readonly MembershipController sut;

        private readonly RegistryContext registryContext;

        private readonly ObjectResultResolver resolver;

        private const int Psr = 10000005;

        private readonly SqliteConnection connection;

        private readonly InMemoryDatabaseBuilder databaseBuilder;

        public MembershipTests()
        {
            connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();

            var optionsBuilder = new DbContextOptionsBuilder<RegistryContext>();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(connection);
            }

            var ioc = new ConfigureIoc();
            var container = ioc.Configure(optionsBuilder);

            sut = new MembershipController(container.GetInstance<IMediator>());

            resolver = new ObjectResultResolver();

            databaseBuilder = new InMemoryDatabaseBuilder();

            AutoMapper.Mapper.Reset();

            var automapper = new ConfigureAutoMapper();

            automapper.Configure();

            registryContext = container.GetInstance<RegistryContext>();

            registryContext.Database.EnsureDeleted();
            registryContext.Database.EnsureCreated();

            databaseBuilder.AddReferanceData(registryContext, Psr);

            databaseBuilder.AddEntityToDb(CreateTblMembership(Psr), registryContext);
        }

        public void Dispose()
        {
            connection.Close();
        }

        [Fact]
        public async void GetMembershipIfValidPsr()
        {
            // Arrange
            var expected = CreateMembership(Psr);
            expected.MembershipReference = 1;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 1;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 1;

            // Act
            var result = await sut.Get(Psr);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);
        }

        [Fact]
        public async void GetMembershipIfNoPsrMatch()
        {
            // Act
            var result = await sut.Get(10000006);

            var resolvedResult = resolver.GetObjectResult(string.Empty, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
            Assert.True(resolvedResult.Value.result == "null");
        }

        [Fact]
        public async void PostMembershipIfPsrMatches()
        {
            // Arrange
            var expected = CreateMembership(Psr);

            var memberToPost = CreateMembership(Psr);

            expected.MembershipReference = 2;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 2;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 2;
            expected.StartEventReference = 6;
            expected.EndEventReference = null;

            // Act
            var result = await sut.Post(memberToPost);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);

            var members = registryContext.TblMembership.Where(x => x.Psrnumber == Psr && x.EndEventReference != null);

            Assert.NotEmpty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == Psr && x.EventType == 8);

            Assert.NotEmpty(events);
        }

        [Fact]
        public async void PostMembershipIfTransationFails()
        {
            // Arrange
            var expected = CreateMembership(1000007);

            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 1;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 1;

            // Act
            var result = await sut.Post(expected);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);

            var members = registryContext.TblMembership.Where(x => x.Psrnumber == 1000007 && x.EndEventReference != null);

            Assert.Empty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == 1000007);

            Assert.Empty(events);
        }

        [Fact]
        public async void PostMembershipIfLessThan2Type()
        {
            // Arrange
            databaseBuilder.AddEntityToDb(CreateTblMembership(1000006), registryContext);

            var expected = CreateMembership(1000006);

            var memberToPost = CreateMembership(1000006);

            expected.MembershipReference = 3;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 3;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 3;
            expected.StartEventReference = 6;
            expected.EndEventReference = null;
            expected.LevyTagTypeReference = 3;

            memberToPost.LevyTagTypeReference = 3;

            // Act
            var result = await sut.Post(memberToPost);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);

            var members = registryContext.TblMembership.Where(x => x.Psrnumber == 1000006 && x.EndEventReference != null);

            Assert.NotEmpty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == 1000006 && x.EventType == 8);

            Assert.NotEmpty(events);
        }

        [Fact]
        public async void PostMembershipIfNoLevyTagType()
        {
            // Arrange
            var existingMember = CreateTblMembership(1000006);
            existingMember.LevyTagTypeReference = null;

            databaseBuilder.AddEntityToDb(existingMember, registryContext);

            var expected = CreateMembership(1000006);

            var memberToPost = CreateMembership(1000006);

            expected.MembershipReference = 3;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 3;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 3;
            expected.StartEventReference = 6;
            expected.EndEventReference = null;
            expected.LevyTagTypeReference = null;

            memberToPost.LevyTagTypeReference = null;
            memberToPost.EffectiveDate = Convert.ToDateTime("1996-03-31T00:00:00");

            // Act
            var result = await sut.Post(memberToPost);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);

            var member = registryContext.TblMembership.ToList();
            var members = registryContext.TblMembership.Where(x => x.Psrnumber == 1000006 && x.EndEventReference != null);

            Assert.NotEmpty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == 1000006 && x.EventType == 8);

            Assert.NotEmpty(events);
        }

        [Fact]
        public async void PostMembershipIfNotificatinDateOlder()
        {
            // Arrange
            var existingMember = CreateTblMembership(1000006);
            existingMember.StartEventReference = 2;

            databaseBuilder.AddEntityToDb(existingMember, registryContext);

            var expected = CreateMembership(1000006);

            var memberToPost = CreateMembership(1000006);

            expected.MembershipReference = 3;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 3;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 3;
            expected.StartEventReference = 6;
            expected.EndEventReference = 6;

            memberToPost.StartEventReference = 2;

            var member = registryContext.TblMembership.Where(x => x.Psrnumber == 1000006).ToList();
            // Act
            var result = await sut.Post(memberToPost);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);
            Assert.Equal(resolvedResult.Value.expected, resolvedResult.Value.result);

            var members = registryContext.TblMembership.Where(x => x.Psrnumber == 1000006 && x.EndEventReference != null);

            Assert.NotEmpty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == 1000006 && x.EventType == 8);

            Assert.NotEmpty(events);
        }

        [Fact]
        public async void GetNotApplicableIfPsrMatches()
        {
            // Arrange
            var expected = CreateMembership(Psr);

            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipAverageAgeBasis = 3;

            await sut.Post(expected);

            expected.MembershipReference = 2;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 2;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 2;

            // Act
            var result = await sut.GetNotApplicable(Psr);

            var resolvedResult = resolver.GetObjectResult(new List<EF_Spike.Membership.Model.Membership> { expected }, result);

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
                MembershipAverageAgeBasis = 1,
                StartEventReference = 1
            };
        }
    }
}