using System;
using EF_Spike.DatabaseContext;

namespace FeatureTests.Tools
{
    public class InMemoryDatabaseBuilder
    {
        public void AddEntityToDb<T>(T entity, RegistryContext context) where T : class
        {
            context.Add(entity);
            context.SaveChanges();
        }

        public void AddReferanceData(RegistryContext registryContext, int psr)
        {
            AddEntityToDb(new TblScheme
            {
                Psrnumber = psr
            }, registryContext);

            AddEntityToDb(new TblScheme
            {
                Psrnumber = 1000006
            }, registryContext);

            AddEntityToDb(new TblSection
            {
                Psrnumber = 1000006,
                SectionNumber = 0
            }, registryContext);

            AddEntityToDb(new TblSection
            {
                Psrnumber = psr,
                SectionNumber = 0
            }, registryContext);

            AddEntityToDb(new TblMembershipBenefitType
            {
                MembershipBenefitTypeReference = 1,
                MembershipBenefitTypeDescription = "test",
                SortOrder = 1
            }, registryContext);

            AddEntityToDb(new TblMembershipType
            {
                MembershipTypeReference = 1,
                MembershipTypeDescription = "test",
                SortOrder = 1
            }, registryContext);

            AddEntityToDb(new TblEventTypeGroup()
            {
                EventTypeGroupReference = 1,
                SortOrder = 1,
                EventTypeGroupDescription = "test"
            }, registryContext);

            AddEntityToDb(new TblEventType
            {
                EventTypeDescription = "test",
                EventTypeGroupReference = 1,
                SortOrder = 1
            }, registryContext);

            AddEntityToDb(new TblEventType
            {
                EventType = 8,
                EventTypeDescription = "test2",
                EventTypeGroupReference = 1,
                SortOrder = 1
            }, registryContext);

            AddEntityToDb(new TblEventSource
            {
                EventSourceDescription = "test",
                EventSourceReference = 1,
                SortOrder = 1
            }, registryContext);

            AddEntityToDb(new TblMembershipAverageAgeBasisType
            {
                MembershipAverageAgeBasisDescription = "test",
                MembershipAverageAgeBasis = 1
            }, registryContext);

            AddEntityToDb(new TblMembershipAverageAgeBasisType
            {
                MembershipAverageAgeBasisDescription = "test3",
                MembershipAverageAgeBasis = 3,
            }, registryContext);

            AddEntityToDb(new TblLevyTagType
            {
                LevyTagTypeReference = 2,
                LevyTagDescription = "test"
            }, registryContext);

            AddEntityToDb(new TblLevyTagType
            {
                LevyTagTypeReference = 3,
                LevyTagDescription = "Less than 2"
            }, registryContext);

            AddEntityToDb(new TblEvent
            {
                EventType = 1,
                Psrnumber = psr,
                SectionNumber = 0,
                UserId = "test",
                CreateDateTime = DateTime.Now.AddDays(-7),
                NotificationDate = DateTime.Now.AddDays(-7),
                EventSourceReference = 1
            }, registryContext);

            AddEntityToDb(new TblEvent
            {
                EventReference = 5,
                EventType = 1,
                Psrnumber = psr,
                SectionNumber = 0,
                UserId = "test",
                CreateDateTime = DateTime.Now,
                NotificationDate = DateTime.Now.AddDays(-7),
                EventSourceReference = 1
            }, registryContext);

            AddEntityToDb(new TblEvent
            {
                EventReference = 2,
                EventType = 1,
                Psrnumber = 1000006,
                SectionNumber = 0,
                UserId = "test",
                CreateDateTime = DateTime.Now,
                NotificationDate = DateTime.Now.AddDays(7),
                EventSourceReference = 1
            }, registryContext);
        }
    }
}