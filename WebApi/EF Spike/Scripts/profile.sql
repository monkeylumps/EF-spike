version 2

Get single

1

exec sp_executesql N'SELECT TOP(1) [x].[MembershipReference], [x].[AgeProfiling50to59], [x].[AgeProfiling60Plus], [x].[EffectiveDate], [x].[EndDate], [x].[EndEventReference], [x].[LevyTagTypeReference], [x].[PSRNumber], [x].[SectionNumber], [x].[StartEventReference]
FROM [tbl_Membership] AS [x]
WHERE (([x].[PSRNumber] = @__query_Psr_0) AND [x].[EndDate] IS NULL) AND [x].[EndEventReference] IS NULL
ORDER BY [x].[MembershipReference]',N'@__query_Psr_0 int',@__query_Psr_0=10000005

2

exec sp_executesql N'SELECT [x.TblMembershipDetails].[MembershipReference], [x.TblMembershipDetails].[MembershipBenefitTypeReference], [x.TblMembershipDetails].[MembershipTypeReference], [x.TblMembershipDetails].[AverageAgeOfMembers], [x.TblMembershipDetails].[NumberOfExcludedMembers], [x.TblMembershipDetails].[NumberOfMembers]
FROM [tbl_MembershipDetails] AS [x.TblMembershipDetails]
INNER JOIN (
    SELECT TOP(1) [x0].[MembershipReference]
    FROM [tbl_Membership] AS [x0]
    WHERE (([x0].[PSRNumber] = @__query_Psr_0) AND [x0].[EndDate] IS NULL) AND [x0].[EndEventReference] IS NULL
    ORDER BY [x0].[MembershipReference]
) AS [t] ON [x.TblMembershipDetails].[MembershipReference] = [t].[MembershipReference]
ORDER BY [t].[MembershipReference]',N'@__query_Psr_0 int',@__query_Psr_0=10000005

3

exec sp_executesql N'SELECT [x.TblMembershipAverageAgeBasis].[MembershipReference], [x.TblMembershipAverageAgeBasis].[StartEventReference], [x.TblMembershipAverageAgeBasis].[MembershipAverageAgeBasis]
FROM [tbl_MembershipAverageAgeBasis] AS [x.TblMembershipAverageAgeBasis]
INNER JOIN (
    SELECT TOP(1) [x1].[MembershipReference]
    FROM [tbl_Membership] AS [x1]
    WHERE (([x1].[PSRNumber] = @__query_Psr_0) AND [x1].[EndDate] IS NULL) AND [x1].[EndEventReference] IS NULL
    ORDER BY [x1].[MembershipReference]
) AS [t0] ON [x.TblMembershipAverageAgeBasis].[MembershipReference] = [t0].[MembershipReference]
ORDER BY [t0].[MembershipReference]',N'@__query_Psr_0 int',@__query_Psr_0=10000005

Post
1

exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [tbl_Event] ([CreateDateTime], [EventSourceReference], [EventType], [NotificationDate], [PSRNumber], [SectionNumber], [SystemBatchReference], [TransactionId], [UserId])
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8);
SELECT [EventReference]
FROM [tbl_Event]
WHERE @@ROWCOUNT = 1 AND [EventReference] = scope_identity();

',N'@p0 datetime,@p1 smallint,@p2 smallint,@p3 datetime,@p4 int,@p5 smallint,@p6 int,@p7 varchar(50),@p8 varchar(300)',@p0='2018-04-06 11:48:48.727',@p1=1,@p2=8,@p3='2018-04-06 11:48:48.727',@p4=9999998,@p5=0,@p6=NULL,@p7=NULL,@p8='user'

2

exec sp_executesql N'SELECT CASE
    WHEN EXISTS (
        SELECT 1
        FROM [tbl_Membership] AS [x]
        WHERE (([x].[PSRNumber] = @__request_Membership_Psrnumber_0) AND [x].[EndDate] IS NULL) AND [x].[EndEventReference] IS NULL)
    THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
END',N'@__request_Membership_Psrnumber_0 int',@__request_Membership_Psrnumber_0=9999998

3

exec sp_executesql N'SELECT [x].[MembershipReference], [x].[AgeProfiling50to59], [x].[AgeProfiling60Plus], [x].[EffectiveDate], [x].[EndDate], [x].[EndEventReference], [x].[LevyTagTypeReference], [x].[PSRNumber], [x].[SectionNumber], [x].[StartEventReference]
FROM [tbl_Membership] AS [x]
WHERE (([x].[PSRNumber] = @__request_Membership_Psrnumber_0) AND [x].[EndDate] IS NULL) AND [x].[EndEventReference] IS NULL',N'@__request_Membership_Psrnumber_0 int',@__request_Membership_Psrnumber_0=9999998

4

exec sp_executesql N'SET NOCOUNT ON;
UPDATE [tbl_Membership] SET [EndDate] = @p0, [EndEventReference] = @p1
WHERE [MembershipReference] = @p2;
SELECT @@ROWCOUNT;

INSERT INTO [tbl_Membership] ([AgeProfiling50to59], [AgeProfiling60Plus], [EffectiveDate], [EndDate], [EndEventReference], [LevyTagTypeReference], [PSRNumber], [SectionNumber], [StartEventReference])
VALUES (@p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11);
SELECT [MembershipReference]
FROM [tbl_Membership]
WHERE @@ROWCOUNT = 1 AND [MembershipReference] = scope_identity();

',N'@p2 int,@p0 datetime,@p1 int,@p3 int,@p4 int,@p5 datetime,@p6 datetime,@p7 int,@p8 smallint,@p9 int,@p10 smallint,@p11 int',@p2=1738102,@p0='2018-04-06 11:48:49.050',@p1=12209128,@p3=NULL,@p4=NULL,@p5='1996-03-31 00:00:00',@p6=NULL,@p7=NULL,@p8=2,@p9=9999998,@p10=0,@p11=5

5

exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [tbl_MembershipAverageAgeBasis] ([MembershipReference], [StartEventReference], [MembershipAverageAgeBasis])
VALUES (@p12, @p13, @p14);
INSERT INTO [tbl_MembershipDetails] ([MembershipReference], [MembershipBenefitTypeReference], [MembershipTypeReference], [AverageAgeOfMembers], [NumberOfExcludedMembers], [NumberOfMembers])
VALUES (@p15, @p16, @p17, @p18, @p19, @p20);
',N'@p12 int,@p13 int,@p14 smallint,@p15 int,@p16 smallint,@p17 smallint,@p18 smallint,@p19 int,@p20 int',@p12=1738103,@p13=1,@p14=1,@p15=1738103,@p16=1,@p17=1,@p18=NULL,@p19=NULL,@p20=10

Get not applicable

1

exec sp_executesql N'SELECT [x].[MembershipReference], [x].[AgeProfiling50to59], [x].[AgeProfiling60Plus], [x].[EffectiveDate], [x].[EndDate], [x].[EndEventReference], [x].[LevyTagTypeReference], [x].[PSRNumber], [x].[SectionNumber], [x].[StartEventReference]
FROM [tbl_Membership] AS [x]
WHERE ((([x].[PSRNumber] = @__request_Psr_0) AND [x].[EndDate] IS NULL) AND [x].[EndEventReference] IS NULL) AND EXISTS (
    SELECT 1
    FROM [tbl_MembershipAverageAgeBasis] AS [y]
    WHERE ([y].[MembershipAverageAgeBasis] = 3) AND ([x].[MembershipReference] = [y].[MembershipReference]))
ORDER BY [x].[MembershipReference]',N'@__request_Psr_0 int',@__request_Psr_0=9999997

2

exec sp_executesql N'SELECT [x.TblMembershipDetails].[MembershipReference], [x.TblMembershipDetails].[MembershipBenefitTypeReference], [x.TblMembershipDetails].[MembershipTypeReference], [x.TblMembershipDetails].[AverageAgeOfMembers], [x.TblMembershipDetails].[NumberOfExcludedMembers], [x.TblMembershipDetails].[NumberOfMembers]
FROM [tbl_MembershipDetails] AS [x.TblMembershipDetails]
INNER JOIN (
    SELECT [x0].[MembershipReference]
    FROM [tbl_Membership] AS [x0]
    WHERE ((([x0].[PSRNumber] = @__request_Psr_0) AND [x0].[EndDate] IS NULL) AND [x0].[EndEventReference] IS NULL) AND EXISTS (
        SELECT 1
        FROM [tbl_MembershipAverageAgeBasis] AS [y0]
        WHERE ([y0].[MembershipAverageAgeBasis] = 3) AND ([x0].[MembershipReference] = [y0].[MembershipReference]))
) AS [t] ON [x.TblMembershipDetails].[MembershipReference] = [t].[MembershipReference]
ORDER BY [t].[MembershipReference]',N'@__request_Psr_0 int',@__request_Psr_0=9999997

3

exec sp_executesql N'SELECT [x.TblMembershipAverageAgeBasis].[MembershipReference], [x.TblMembershipAverageAgeBasis].[StartEventReference], [x.TblMembershipAverageAgeBasis].[MembershipAverageAgeBasis]
FROM [tbl_MembershipAverageAgeBasis] AS [x.TblMembershipAverageAgeBasis]
INNER JOIN (
    SELECT [x1].[MembershipReference]
    FROM [tbl_Membership] AS [x1]
    WHERE ((([x1].[PSRNumber] = @__request_Psr_0) AND [x1].[EndDate] IS NULL) AND [x1].[EndEventReference] IS NULL) AND EXISTS (
        SELECT 1
        FROM [tbl_MembershipAverageAgeBasis] AS [y1]
        WHERE ([y1].[MembershipAverageAgeBasis] = 3) AND ([x1].[MembershipReference] = [y1].[MembershipReference]))
) AS [t0] ON [x.TblMembershipAverageAgeBasis].[MembershipReference] = [t0].[MembershipReference]
ORDER BY [t0].[MembershipReference]',N'@__request_Psr_0 int',@__request_Psr_0=9999997