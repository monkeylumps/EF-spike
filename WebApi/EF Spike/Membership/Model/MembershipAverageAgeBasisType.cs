using System.Collections.Generic;

namespace EF_Spike.Membership.Model
{
    public class MembershipAverageAgeBasisType
    {
        public short MembershipAverageAgeBasis { get; set; }
        public string MembershipAverageAgeBasisDescription { get; set; }

        public ICollection<MembershipAverageAgeBasiss> TblMembershipAverageAgeBasis { get; set; }
    }
}