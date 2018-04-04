namespace EF_Spike.Membership.Model
{
    public class MembershipDetails
    {
        public int MembershipReference { get; set; }
        public short MembershipBenefitTypeReference { get; set; }
        public short MembershipTypeReference { get; set; }
        public int NumberOfMembers { get; set; }
        public int? NumberOfExcludedMembers { get; set; }
        public short? AverageAgeOfMembers { get; set; }
    }
}