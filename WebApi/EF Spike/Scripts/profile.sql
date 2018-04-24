new sql .queryable

1.

SELECT TOP(1) [x].[LevyTagTypeReference]
FROM [tbl_LevyTagType] AS [x]
WHERE [x].[LevyTagDescription] = 'Less than 2'

2.

exec sp_executesql N'SELECT TOP(1) [x].[MembershipReference], [x].[AgeProfiling50to59], [x].[AgeProfiling60Plus], [x].[EffectiveDate], [x].[EndDate], [x].[EndEventReference], [x].[LevyTagTypeReference], [x].[PSRNumber], [x].[SectionNumber], [x].[StartEventReference], [e].[NotificationDate]
FROM [tbl_Membership] AS [x]
INNER JOIN [tbl_Event] AS [e] ON [x].[StartEventReference] = [e].[EventReference]
WHERE (((([x].[PSRNumber] = @__8__locals1_request_Membership_Psrnumber_0) AND ([x].[SectionNumber] = @__8__locals1_request_Membership_SectionNumber_1)) AND [x].[EndEventReference] IS NULL) AND [x].[EndDate] IS NULL) AND ([x].[LevyTagTypeReference] = @__8__locals1_request_Membership_LevyTagTypeReference_2)',N'@__8__locals1_request_Membership_Psrnumber_0 int,@__8__locals1_request_Membership_SectionNumber_1 smallint,@__8__locals1_request_Membership_LevyTagTypeReference_2 smallint',@__8__locals1_request_Membership_Psrnumber_0=10000005,@__8__locals1_request_Membership_SectionNumber_1=0,@__8__locals1_request_Membership_LevyTagTypeReference_2=2

3.

exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [tbl_Membership] ([AgeProfiling50to59], [AgeProfiling60Plus], [EffectiveDate], [EndDate], [EndEventReference], [LevyTagTypeReference], [PSRNumber], [SectionNumber], [StartEventReference])
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8);
SELECT [MembershipReference]
FROM [tbl_Membership]
WHERE @@ROWCOUNT = 1 AND [MembershipReference] = scope_identity();

',N'@p0 int,@p1 int,@p2 datetime,@p3 datetime,@p4 int,@p5 smallint,@p6 int,@p7 smallint,@p8 int',@p0=NULL,@p1=NULL,@p2='1996-03-31 00:00:00',@p3=NULL,@p4=NULL,@p5=2,@p6=10000005,@p7=0,@p8=12210182

4.

exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [tbl_MembershipDetails] ([MembershipReference], [MembershipBenefitTypeReference], [MembershipTypeReference], [AverageAgeOfMembers], [NumberOfExcludedMembers], [NumberOfMembers])
VALUES (@p9, @p10, @p11, @p12, @p13, @p14);
',N'@p9 int,@p10 smallint,@p11 smallint,@p12 smallint,@p13 int,@p14 int',@p9=1739159,@p10=1,@p11=1,@p12=NULL,@p13=NULL,@p14=10

5.

exec sp_executesql N'SELECT TOP(1) [x].[MembershipReference], [x].[AgeProfiling50to59], [x].[AgeProfiling60Plus], [x].[EffectiveDate], [x].[EndDate], [x].[EndEventReference], [x].[LevyTagTypeReference], [x].[PSRNumber], [x].[SectionNumber], [x].[StartEventReference]
FROM [tbl_Membership] AS [x]
WHERE [x].[MembershipReference] = @__replaceMembershipReference_0',N'@__replaceMembershipReference_0 int',@__replaceMembershipReference_0=1739158

6.

exec sp_executesql N'SET NOCOUNT ON;
UPDATE [tbl_Membership] SET [AgeProfiling50to59] = @p0, [AgeProfiling60Plus] = @p1, [EffectiveDate] = @p2, [EndDate] = @p3, [EndEventReference] = @p4, [LevyTagTypeReference] = @p5, [PSRNumber] = @p6, [SectionNumber] = @p7, [StartEventReference] = @p8
WHERE [MembershipReference] = @p9;
SELECT @@ROWCOUNT;

',N'@p9 int,@p0 int,@p1 int,@p2 datetime,@p3 datetime,@p4 int,@p5 smallint,@p6 int,@p7 smallint,@p8 int',@p9=1739158,@p0=NULL,@p1=NULL,@p2='1996-03-31 00:00:00',@p3=NULL,@p4=12210182,@p5=2,@p6=10000005,@p7=0,@p8=12210181