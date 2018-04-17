using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblScheme
    {
        public TblScheme()
        {
            TblSection = new HashSet<TblSection>();
        }

        public int Psrnumber { get; set; }
        public bool SegregatedSectionalised { get; set; }
        public int StartEventReference { get; set; }
        public int? EndEventReference { get; set; }

        public ICollection<TblSection> TblSection { get; set; }
    }
}
