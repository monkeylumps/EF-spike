using System;
using EF_Spike.DatabaseContext;
using Microsoft.EntityFrameworkCore;

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

        public void CreateSproc(RegistryContext registryContext)
        {
            registryContext.Database.ExecuteSqlCommandAsync("USE[Registry]" +
"GO" +
"" +
"/****** Object:  StoredProcedure [dbo].[usp_Create_Membership]    Script Date: 24/04/2018 15:19:42 ******/" +
"SET ANSI_NULLS ON" +
"GO" +
"" +
"SET QUOTED_IDENTIFIER ON" +
"GO" +
"" +
"" +
"" +
"" +
"" +
"" +
"" +
"" +
"CREATE PROCEDURE [dbo].[usp_Create_Membership]" +
    "-- UserDetails" +
    "@CreatedBy VARCHAR(50)," +
"" +
    "-- Membership Data" +
    "@PSRNumber INT," +
    "@SectionNumber SMALLINT," +
    "@LevyTagTypeReference SMALLINT = NULL," +
    "@EffectiveDate SMALLDATETIME," +
    "@NotificationDate SMALLDATETIME," +
    "@MembershipReference INT," +
"" +
    "-- MembershipDetail data" +
    "-- DC Data" +
    "@ActiveDC INT = NULL," +
    "@DeferredDC INT = NULL," +
    "@PensionerDC INT = NULL," +
    "@TotalDC INT = NULL," +
    "@PensionCreditDC INT = NULL," +
"" +
    "-- DB Data" +
    "@ActiveDB INT = NULL," +
    "@DeferredDB INT = NULL," +
    "@PensionerDB INT = NULL," +
    "@TotalDB INT = NULL," +
    "@PensionCreditDB INT = NULL," +
"" +
    "-- Partial DB Data	" +
    "@ActivePartialDB INT = NULL," +
    "@DeferredPartialDB INT = NULL," +
    "@PensionerPartialDB INT = NULL," +
    "@TotalPartialDB INT = NULL," +
    "@PensionCreditPartialDB INT = NULL," +
"" +
    "-- Excluded Members" +
    "@ActiveExcludedMembers INT = NULL," +
    "@DeferredExcludedMembers INT = NULL," +
    "@PensionerExcludedMembers INT = NULL," +
    "@TotalExcludedMembers INT = NULL," +
"" +
    "-- Whole Membership" +
    "@WholeMembership INT = NULL," +
"	" +
    "-- Average Age" +
    "@ActiveAverageAge INT = NULL," +
    "@DeferredAverageAge INT = NULL," +
    "@PensionerAverageAge INT = NULL," +
"" +
    "@AgeProfiling50to59	int = NULL ," +
    "@AgeProfiling60Plus	int = NULL " +
"AS" +
"" +
"SET NOCOUNT ON" +
"" +
"DECLARE @EventReference INT" +
"" +
"IF @MembershipReference <> 0" +
"BEGIN" +
    "EXEC usp_Create_Event 'Amend Membership', @PSRNumber, @SectionNumber, @CreatedBy, @NotificationDate, NULL, @EventReference OUTPUT " +
    "IF @@ERROR <> 0" +
        "RETURN" +
"END" +
"ELSE" +
"BEGIN" +
    "EXEC usp_Create_Event 'Add Membership', @PSRNumber, @SectionNumber, @CreatedBy, @NotificationDate, NULL, @EventReference OUTPUT " +
    "IF @@ERROR <> 0" +
        "RETURN" +
"END" +
"" +
"-- Common variables used in both paths of the if statement" +
"DECLARE @ExistingNotificationDate SMALLDATETIME" +
"DECLARE @ExistingEffectiveDate SMALLDATETIME" +
"DECLARE @EndEventReference INT" +
"DECLARE @NewMembershipReference INT" +
"" +
"--Get Less than 2 levy type reference" +
"DECLARE @LessThan2Type SMALLINT" +
"SELECT @LessThan2Type =  LevyTagTypeReference " +
"FROM tbl_LevyTagType" +
"WHERE LevyTagDescription='Less than 2'" +
"" +
"IF @MembershipReference = 0" +
"BEGIN" +
    "-- Perform an 'add' operation" +
"" +
    "-- Check to see if this is a 'historic insert'/'replacement insert'" +
    "DECLARE @ExistingMembershipReference INT" +
    "DECLARE @ReplaceMembershipReference INT" +
"" +
    "IF @LevyTagTypeReference IS NOT NULL" +
    "BEGIN" +
"" +
    "IF(@LevyTagTypeReference = @LessThan2Type)" +
        "BEGIN" +
            "--Get existing 'Less than 2' Levy tag with same effective date" +
            "SELECT @ExistingNotificationDate = ev.NotificationDate," +
                "@ExistingMembershipReference = m.MembershipReference," +
                "@ExistingEffectiveDate = m.EffectiveDate" +
            "FROM tbl_Membership m " +
            "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
            "WHERE " +
                "m.LevyTagTypeReference = @LevyTagTypeReference AND" +
                "m.PSRNumber = @PSRNumber AND" +
                "m.SectionNumber = @SectionNumber AND" +
                "m.EndEventReference IS NULL AND" +
                "m.EndDate IS NULL AND" +
                "m.EffectiveDate = @EffectiveDate" +
            "IF @@ERROR <> 0" +
                "RETURN" +
        "END" +
    "ELSE" +
        "BEGIN" +
            "SELECT	@ExistingNotificationDate = ev.NotificationDate," +
                "@ExistingMembershipReference = m.MembershipReference," +
                "@ExistingEffectiveDate = m.EffectiveDate" +
            "FROM tbl_Membership m " +
            "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
            "WHERE " +
                "m.LevyTagTypeReference = @LevyTagTypeReference AND" +
                "m.PSRNumber = @PSRNumber AND" +
                "m.SectionNumber = @SectionNumber AND" +
                "m.EndEventReference IS NULL AND" +
                "m.EndDate IS NULL " +
            "IF @@ERROR <> 0" +
                "RETURN" +
        "END	" +
"" +
"" +
        "IF @ExistingNotificationDate <= @NotificationDate" +
        "BEGIN" +
            "SET @ReplaceMembershipReference = @ExistingMembershipReference" +
        "END" +
"" +
        "IF @ExistingNotificationDate > @NotificationDate" +
        "BEGIN" +
            "SET @EndEventReference = @EventReference" +
        "END" +
    "END" +
    "ELSE" +
    "BEGIN" +
        "SELECT	@ExistingNotificationDate = ev.NotificationDate," +
            "@ExistingMembershipReference = m.MembershipReference," +
            "@ExistingEffectiveDate = m.EffectiveDate" +
        "FROM tbl_Membership m " +
        "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
        "WHERE " +
            "m.LevyTagTypeReference IS NULL AND" +
            "m.PSRNumber = @PSRNumber AND" +
            "m.SectionNumber = @SectionNumber AND" +
            "m.EndEventReference IS NULL AND" +
            "m.EndDate IS NULL AND" +
            "m.EffectiveDate = @EffectiveDate" +
        "IF @@ERROR <> 0" +
            "RETURN" +
"" +
        "IF @ExistingNotificationDate <= @NotificationDate" +
        "BEGIN" +
            "SET @ReplaceMembershipReference = @ExistingMembershipReference" +
        "END" +
"" +
        "IF @ExistingNotificationDate > @NotificationDate" +
        "BEGIN" +
            "SET @EndEventReference = @EventReference" +
        "END" +
    "END" +
"" +
    "-- Create the Membership Reference" +
    "EXEC usp_Create_Membership_Row " +
        "@PSRNumber, " +
        "@SectionNumber, " +
        "@LevyTagTypeReference, " +
        "@EffectiveDate, " +
        "NULL, " +
        "@EventReference, " +
        "@EndEventReference, " +
        "@AgeProfiling50to59," +
        "@AgeProfiling60Plus, " +
        "@NewMembershipReference OUTPUT " +
    "IF @@ERROR <> 0" +
        "RETURN" +
"" +
    "-- Create the Membership Details" +
    "EXEC usp_Create_MembershipDetails " +
        "@NewMembershipReference, " +
        "@ActiveDC, " +
        "@DeferredDC, " +
        "@PensionerDC, " +
        "@TotalDC, " +
        "@PensionCreditDC, " +
        "@ActiveDB, " +
        "@DeferredDB, " +
        "@PensionerDB, " +
        "@TotalDB, " +
        "@PensionCreditDB, " +
        "@ActivePartialDB, " +
        "@DeferredPartialDB, " +
        "@PensionerPartialDB, " +
        "@TotalPartialDB, " +
        "@PensionCreditPartialDB, " +
        "@ActiveExcludedMembers, " +
        "@DeferredExcludedMembers, " +
        "@PensionerExcludedMembers, " +
        "@TotalExcludedMembers, " +
        "@WholeMembership, " +
        "@ActiveAverageAge, " +
        "@DeferredAverageAge, " +
        "@PensionerAverageAge" +
    "IF @@ERROR <> 0" +
        "RETURN" +
"" +
    "-- Update existing membership if required" +
    "--For levy tag 'Less than 2', only end event reference if same effective date otherwise it should be treated as a new record" +
    "IF(@LevyTagTypeReference<>@LessThan2Type OR ( @LevyTagTypeReference = @LessThan2Type AND @EffectiveDate=@ExistingEffectiveDate))" +
    "BEGIN" +
        "IF @ReplaceMembershipReference IS NOT NULL" +
        "BEGIN" +
            "UPDATE tbl_Membership" +
            "SET EndEventReference = @EventReference" +
            "FROM tbl_Membership m" +
            "WHERE m.MembershipReference = @ReplaceMembershipReference" +
            "IF @@ERROR <> 0" +
                "RETURN" +
        "END" +
    "END" +
"" +
"END" +
"ELSE" +
"BEGIN" +
    "-- Perform an amendment" +
"" +
    "-- Check to see if this is a 'Historic Insert'" +
    "SELECT @ExistingNotificationDate = ev.NotificationDate," +
    "@ExistingEffectiveDate = m.EffectiveDate" +
    "FROM tbl_Membership m" +
    "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
    "WHERE m.MembershipReference = @MembershipReference" +
    "IF @@ERROR <> 0" +
        "RETURN" +
"" +
    "IF @ExistingNotificationDate > @NotificationDate" +
    "BEGIN" +
        "SET @EndEventReference = @EventReference" +
    "END" +
"" +
    "-- Create the Membership Reference" +
    "EXEC usp_Create_Membership_Row " +
        "@PSRNumber, " +
        "@SectionNumber, " +
        "@LevyTagTypeReference, " +
        "@EffectiveDate, " +
        "NULL, " +
        "@EventReference, " +
        "@EndEventReference, " +
        "@AgeProfiling50to59," +
        "@AgeProfiling60Plus, " +
        "@NewMembershipReference OUTPUT " +
    "IF @@ERROR <> 0" +
        "RETURN" +
"" +
    "-- Create the Membership Details" +
    "EXEC usp_Create_MembershipDetails " +
        "@NewMembershipReference, " +
        "@ActiveDC, " +
        "@DeferredDC, " +
        "@PensionerDC, " +
        "@TotalDC, " +
        "@PensionCreditDC, " +
        "@ActiveDB, " +
        "@DeferredDB, " +
        "@PensionerDB, " +
        "@TotalDB, " +
        "@PensionCreditDB, " +
        "@ActivePartialDB, " +
        "@DeferredPartialDB, " +
        "@PensionerPartialDB, " +
        "@TotalPartialDB, " +
        "@PensionCreditPartialDB, " +
        "@ActiveExcludedMembers, " +
        "@DeferredExcludedMembers, " +
        "@PensionerExcludedMembers, " +
        "@TotalExcludedMembers, " +
        "@WholeMembership, " +
        "@ActiveAverageAge, " +
        "@DeferredAverageAge, " +
        "@PensionerAverageAge" +
    "IF @@ERROR <> 0" +
        "RETURN" +
"" +
    "-- LevyYear replacement update processing -- exclude 'Less than 2'	 leg type processing" +
    "IF(@LevyTagTypeReference IS NULL OR @LevyTagTypeReference<>@LessThan2Type)" +
        "BEGIN" +
        "UPDATE tbl_Membership" +
            "SET EndEventReference = @EventReference" +
            "FROM tbl_Membership m" +
            "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
            "WHERE" +
                "m.MembershipReference <> @NewMembershipReference AND" +
                "m.PSRNumber = @PSRNumber AND" +
                "m.SectionNumber = @SectionNumber AND" +
                "m.LevyTagTypeReference = @LevyTagTypeReference AND" +
                "m.EndEventReference IS NULL AND" +
                "ev.NotificationDate <=  @NotificationDate" +
            "IF @@ERROR <> 0" +
                "RETURN" +
"	" +
        "-- End Event existing record if notified earlier and still non-levy specific" +
        "UPDATE tbl_Membership" +
            "SET EndEventReference = @EventReference" +
            "FROM tbl_Membership m" +
            "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
            "WHERE" +
                "m.MembershipReference = @MembershipReference AND" +
                "m.PSRNumber = @PSRNumber AND" +
                "m.SectionNumber = @SectionNumber AND" +
                "m.LevyTagTypeReference IS NULL AND" +
                "m.EndEventReference IS NULL AND" +
                "ev.NotificationDate <=  @NotificationDate" +
            "IF @@ERROR <> 0" +
                "RETURN" +
        "END" +
    "ELSE IF(@LevyTagTypeReference = @LessThan2Type AND @EffectiveDate=@ExistingEffectiveDate)" +
    "BEGIN" +
        "--For 'Less than 2' type, only end event reference is has same effective date" +
        "UPDATE tbl_Membership" +
            "SET EndEventReference = @EventReference" +
            "FROM tbl_Membership m" +
            "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
            "WHERE" +
                "m.MembershipReference <> @NewMembershipReference AND" +
                "m.PSRNumber = @PSRNumber AND" +
                "m.SectionNumber = @SectionNumber AND" +
                "m.LevyTagTypeReference = @LevyTagTypeReference AND" +
                "m.EndEventReference IS NULL AND" +
                "ev.NotificationDate <=  @NotificationDate AND" +
                "m.EffectiveDate  =@EffectiveDate" +
            "IF @@ERROR <> 0" +
                "RETURN" +
"	" +
        "-- End Event existing record if notified earlier and still non-levy specific" +
        "UPDATE tbl_Membership" +
            "SET EndEventReference = @EventReference" +
            "FROM tbl_Membership m" +
            "JOIN tbl_Event ev ON ev.EventReference = m.StartEventReference" +
            "WHERE" +
                "m.MembershipReference = @MembershipReference AND" +
                "m.PSRNumber = @PSRNumber AND" +
                "m.SectionNumber = @SectionNumber AND" +
                "m.LevyTagTypeReference IS NULL AND" +
                "m.EndEventReference IS NULL AND" +
                "ev.NotificationDate <=  @NotificationDate AND" +
                "m.EffectiveDate  =@EffectiveDate" +
            "IF @@ERROR <> 0" +
                "RETURN" +
    "END" +
"END" +
"" +
"" +
"" +
"" +
"GO");
        }
    }
}