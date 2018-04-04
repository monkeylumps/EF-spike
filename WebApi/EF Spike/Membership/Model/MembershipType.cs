using System.Collections.Generic;

namespace EF_Spike.Membership.Model
{
    public class MembershipType
    {
        public short MembershipTypeReference { get; set; }
        public string MembershipTypeDescription { get; set; }
        public short SortOrder { get; set; }

        public ICollection<MembershipDetails> TblMembershipDetails { get; set; }
    }
}