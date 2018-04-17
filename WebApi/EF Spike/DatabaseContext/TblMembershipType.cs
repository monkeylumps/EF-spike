using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblMembershipType
    {
        public TblMembershipType()
        {
            TblMembershipDetails = new HashSet<TblMembershipDetails>();
        }

        public short MembershipTypeReference { get; set; }
        public string MembershipTypeDescription { get; set; }
        public short SortOrder { get; set; }

        public ICollection<TblMembershipDetails> TblMembershipDetails { get; set; }
    }
}
