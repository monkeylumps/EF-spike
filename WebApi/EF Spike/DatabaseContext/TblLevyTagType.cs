using System;
using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblLevyTagType
    {
        public TblLevyTagType()
        {
            TblMembership = new HashSet<TblMembership>();
        }

        public short LevyTagTypeReference { get; set; }
        public string LevyTagDescription { get; set; }
        public short SortOrder { get; set; }
        public short? LevyYear { get; set; }
        public DateTime? LevyYearStartDate { get; set; }
        public DateTime? LevyYearEndDate { get; set; }
        public bool? SpecialCase { get; set; }
        public short? Sryear { get; set; }

        public ICollection<TblMembership> TblMembership { get; set; }
    }
}
