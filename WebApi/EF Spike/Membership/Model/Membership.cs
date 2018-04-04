using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;

namespace EF_Spike.Membership.Model
{
    public class Membership
    {
        public Membership()
        {

        }
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
