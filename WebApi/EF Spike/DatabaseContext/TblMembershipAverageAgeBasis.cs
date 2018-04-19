namespace EF_Spike.DatabaseContext
{
    public partial class TblMembershipAverageAgeBasis
    {
        public int MembershipReference { get; set; }
        public short MembershipAverageAgeBasis { get; set; }
        public int StartEventReference { get; set; }

        public TblMembershipAverageAgeBasisType MembershipAverageAgeBasisNavigation { get; set; }
        public TblMembership MembershipReferenceNavigation { get; set; }
    }
}
