using System;
using System.Collections.Generic;

namespace EF_Spike.Membership.Model
{
    public class Membership
    {
        public int MembershipReference { get; set; }
        public int Psrnumber { get; set; }
        public short SectionNumber { get; set; }
        public short? LevyTagTypeReference { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StartEventReference { get; set; }
        public int? EndEventReference { get; set; }
        public int? AgeProfiling50to59 { get; set; }
        public int? AgeProfiling60Plus { get; set; }

        public ICollection<MembershipAverageAgeBasiss> TblMembershipAverageAgeBasis { get; set; }
        public ICollection<MembershipDetails> TblMembershipDetails { get; set; }
    }
}
