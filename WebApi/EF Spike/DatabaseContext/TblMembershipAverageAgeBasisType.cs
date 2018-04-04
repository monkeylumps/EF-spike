using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblMembershipAverageAgeBasisType
    {
        public TblMembershipAverageAgeBasisType()
        {
            TblMembershipAverageAgeBasis = new HashSet<TblMembershipAverageAgeBasis>();
        }

        public short MembershipAverageAgeBasis { get; set; }
        public string MembershipAverageAgeBasisDescription { get; set; }

        public ICollection<TblMembershipAverageAgeBasis> TblMembershipAverageAgeBasis { get; set; }
    }
}
