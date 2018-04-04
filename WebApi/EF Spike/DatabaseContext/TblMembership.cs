using System;

namespace EF_Spike.DatabaseContext
{
    public partial class TblMembership
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
    }
}
