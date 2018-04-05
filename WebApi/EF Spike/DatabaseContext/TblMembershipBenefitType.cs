using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblMembershipBenefitType
    {
        public TblMembershipBenefitType()
        {
            TblMembershipDetails = new HashSet<TblMembershipDetails>();
        }

        public short MembershipBenefitTypeReference { get; set; }
        public string MembershipBenefitTypeDescription { get; set; }
        public short SortOrder { get; set; }

        public ICollection<TblMembershipDetails> TblMembershipDetails { get; set; }
    }
}
