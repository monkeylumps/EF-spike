using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblEventSource
    {
        public TblEventSource()
        {
            TblEvent = new HashSet<TblEvent>();
        }

        public short EventSourceReference { get; set; }
        public string EventSourceDescription { get; set; }
        public int SortOrder { get; set; }

        public ICollection<TblEvent> TblEvent { get; set; }
    }
}
