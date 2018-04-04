using System.Collections.Generic;

namespace EF_Spike.Membership.Model
{
    public class MembershipBenefitType
    {
        public short MembershipBenefitTypeReference { get; set; }
        public string MembershipBenefitTypeDescription { get; set; }
        public short SortOrder { get; set; }

        public ICollection<MembershipDetails> TblMembershipDetails { get; set; }
    }
}