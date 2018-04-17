using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblEventType
    {
        public TblEventType()
        {
            TblEvent = new HashSet<TblEvent>();
        }

        public short EventType { get; set; }
        public string EventTypeDescription { get; set; }
        public short SortOrder { get; set; }
        public int EventTypeGroupReference { get; set; }
        public bool? ScoreUivisible { get; set; }

        public TblEventTypeGroup EventTypeGroupReferenceNavigation { get; set; }
        public ICollection<TblEvent> TblEvent { get; set; }
    }
}
