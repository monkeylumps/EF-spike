using System;
using System.Collections.Generic;
using System.Linq;
using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Controller;
using EF_Spike.Membership.Model;
using FeatureTests.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntergrationTests.Membership
{
    public class MembershipTests
    {
        private readonly MembershipController sut;

        private readonly RegistryContext registryContext;

        private readonly ObjectResultResolver resolver;

        private const int Psr = 10000005;

        public MembershipTests()
        {
            const string connection = @"Server=Registry;Database=Registry;Trusted_Connection=True;";

            var optionsBuilder = new DbContextOptionsBuilder<RegistryContext>();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection);
            }

            var ioc = new ConfigureIoc();
            var container = ioc.Configure(optionsBuilder);

            sut = new MembershipController(container.GetInstance<IMediator>());

            resolver = new ObjectResultResolver();

            AutoMapper.Mapper.Reset();

            var automapper = new ConfigureAutoMapper();

            automapper.Configure();

            registryContext = container.GetInstance<RegistryContext>();
        }

        [Fact(Skip = "IntTest")]
        public async void GetMembershipIfValidPsr()
        {
            // Arrange
            var expected = CreateMembership(Psr);

            // Act
            var result = await sut.Get(Psr);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
        }

        [Fact(Skip = "IntTest")]
        public async void GetMembershipIfNoPsrMatch()
        {
            // Act
            var result = await sut.Get(9999999);

            var resolvedResult = resolver.GetObjectResult(string.Empty, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
            Assert.True(resolvedResult.Value.result == "null");
        }

        [Fact(Skip = "IntTest")]
        public async void PostMembership()
        {
            // Arrange
            var expected = CreateMembership(Psr);

            var memberToPost = CreateMembership(Psr);

            // Act
            var result = await sut.Post(memberToPost);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);

            var members = registryContext.TblMembership.Where(x => x.Psrnumber == Psr && x.EndEventReference != null);

            Assert.NotEmpty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == Psr && x.EventType == 8);

            Assert.NotEmpty(events);
        }

        [Fact(Skip = "IntTest")]
        public async void PostMembershipIfPsrMatches()
        {
            // Arrange
            const int testPsr = 12007081;
            var expected = CreateMembership(testPsr);

            var memberToPost = CreateMembership(testPsr);

            expected.MembershipReference = 2;
            //expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 2;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 2;

            CreateScheme(testPsr);

            // Act
            var result = await sut.Post(memberToPost);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);

            var members = registryContext.TblMembership.Where(x => x.Psrnumber == Psr && x.EndEventReference != null);

            Assert.NotEmpty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == Psr && x.EventType == 8);

            Assert.NotEmpty(events);

            var dbMember = registryContext.TblMembership.Include(x => x.TblMembershipDetails)
                .Include(x => x.TblMembershipAverageAgeBasis).FirstOrDefault(x =>
                    x.Psrnumber == testPsr && x.EndDate != null && x.EndEventReference != null);

            Assert.NotNull(dbMember);
        }

        [Fact(Skip = "IntTest")]
        public async void PostMembershipIfTransationFails()
        {
            // Arrange
            const int testPsr = 1000006;
            var expected = CreateMembership(testPsr);

            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 1;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 1;

            // Act
            var result = await sut.Post(expected);

            var resolvedResult = resolver.GetObjectResult(expected, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(201, resolvedResult.Value.objectResult.StatusCode);

            var members = registryContext.TblMembership.Where(x => x.Psrnumber == testPsr && x.EndEventReference != null);

            Assert.Empty(members);

            var events = registryContext.TblEvent.Where(x => x.Psrnumber == testPsr);

            Assert.Empty(events);

            var dbMember = registryContext.TblMembership.Include(x => x.TblMembershipDetails)
                .Include(x => x.TblMembershipAverageAgeBasis).FirstOrDefault(x =>
                    x.Psrnumber == testPsr && x.EndDate != null && x.EndEventReference != null);

            Assert.Null(dbMember);
        }

        [Fact(Skip = "IntTest")]
        public async void GetNotApplicableIfPsrMatches()
        {
            // Arrange
            var testPsr = 9999997;
            var expected = CreateMembership(testPsr);

            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipReference = 2;
            expected.TblMembershipAverageAgeBasis.FirstOrDefault().MembershipAverageAgeBasis = 3;
            expected.TblMembershipDetails.FirstOrDefault().MembershipReference = 2;

            CreateScheme(testPsr);

            await sut.Post(expected);

            // Act
            var result = await sut.GetNotApplicable(testPsr);

            var resolvedResult = resolver.GetObjectResult(new List<EF_Spike.Membership.Model.Membership> { expected }, result);

            // Assert
            Assert.NotNull(resolvedResult);
            Assert.Equal(200, resolvedResult.Value.objectResult.StatusCode);
        }

        private async void CreateScheme(int psr)
        {
            var scheme = registryContext.TblScheme.FirstOrDefault(x => x.Psrnumber == psr);

            if (scheme == null)
            {
                registryContext.Add(new TblScheme
                {
                    Psrnumber = psr
                });

                var section = registryContext.TblSection.FirstOrDefault(x => x.Psrnumber == psr);

                if (section == null)
                {
                    registryContext.Add(new TblSection
                    {
                        Psrnumber = psr,
                        SectionNumber = 0
                    });
                }
            }

            await registryContext.SaveChangesAsync();
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
                TblMembershipDetails = new List<MembershipDetails> { CreateMembershipDetails() }
                //TblMembershipAverageAgeBasis = new List<MembershipAverageAgeBasiss> { CreateMembershipAverageAgeBasis() }
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
                StartEventReference = 1,
            };
        }
    }
}