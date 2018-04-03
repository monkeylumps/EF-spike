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

        public Membership(TblMembership membership)
        {
            MembershipReference = membership.MembershipReference;
            Psrnumber = membership.Psrnumber;
            SectionNumber = membership.SectionNumber;
            LevyTagTypeReference = membership.LevyTagTypeReference;
            EffectiveDate = membership.EffectiveDate;
            EndDate = membership.EndDate;
            StartEventReference = membership.StartEventReference;
            EndEventReference = membership.EndEventReference;
            AgeProfiling50to59 = membership.AgeProfiling50to59;
            AgeProfiling60Plus = membership.AgeProfiling60Plus;
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
