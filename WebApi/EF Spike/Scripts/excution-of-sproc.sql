USE [Registry]
GO

DECLARE @RC int

-- Membership Data
DECLARE @CreatedBy varchar(50)
DECLARE @PSRNumber int
DECLARE @SectionNumber smallint
DECLARE @LevyTagTypeReference smallint
DECLARE @EffectiveDate smalldatetime
DECLARE @NotificationDate smalldatetime
DECLARE @MembershipReference int

-- MembershipDetail data
-- DC Data
DECLARE @ActiveDC int
DECLARE @DeferredDC int
DECLARE @PensionerDC int
DECLARE @TotalDC int
DECLARE @PensionCreditDC int

-- DB Data
DECLARE @ActiveDB int
DECLARE @DeferredDB int
DECLARE @PensionerDB int
DECLARE @TotalDB int
DECLARE @PensionCreditDB int

-- Partial DB Data	
DECLARE @ActivePartialDB int
DECLARE @DeferredPartialDB int
DECLARE @PensionerPartialDB int
DECLARE @TotalPartialDB int
DECLARE @PensionCreditPartialDB int

-- Excluded Members
DECLARE @ActiveExcludedMembers int
DECLARE @DeferredExcludedMembers int
DECLARE @PensionerExcludedMembers int
DECLARE @TotalExcludedMembers int

-- Whole Membership
DECLARE @WholeMembership int

-- Average Age
DECLARE @ActiveAverageAge int
DECLARE @DeferredAverageAge int
DECLARE @PensionerAverageAge int


DECLARE @AgeProfiling50to59 int
DECLARE @AgeProfiling60Plus int

-- TODO: Set parameter values here.

EXECUTE @RC = [dbo].[usp_Create_Membership] 
   @CreatedBy
  ,@PSRNumber
  ,@SectionNumber
  ,@LevyTagTypeReference
  ,@EffectiveDate
  ,@NotificationDate
  ,@MembershipReference
  ,@ActiveDC
  ,@DeferredDC
  ,@PensionerDC
  ,@TotalDC
  ,@PensionCreditDC
  ,@ActiveDB
  ,@DeferredDB
  ,@PensionerDB
  ,@TotalDB
  ,@PensionCreditDB
  ,@ActivePartialDB
  ,@DeferredPartialDB
  ,@PensionerPartialDB
  ,@TotalPartialDB
  ,@PensionCreditPartialDB
  ,@ActiveExcludedMembers
  ,@DeferredExcludedMembers
  ,@PensionerExcludedMembers
  ,@TotalExcludedMembers
  ,@WholeMembership
  ,@ActiveAverageAge
  ,@DeferredAverageAge
  ,@PensionerAverageAge
  ,@AgeProfiling50to59
  ,@AgeProfiling60Plus
GO


