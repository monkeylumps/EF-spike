using System;
using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblSection
    {
        public TblSection()
        {
            TblMembership = new HashSet<TblMembership>();
        }

        public int Psrnumber { get; set; }
        public short SectionNumber { get; set; }
        public int? StartEventReference { get; set; }
        public int? EndEventReference { get; set; }

        public TblScheme PsrnumberNavigation { get; set; }
        public ICollection<TblMembership> TblMembership { get; set; }
    }
}
