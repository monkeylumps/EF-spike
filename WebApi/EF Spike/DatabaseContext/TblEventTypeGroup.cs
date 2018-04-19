using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblEventTypeGroup
    {
        public TblEventTypeGroup()
        {
            TblEventType = new HashSet<TblEventType>();
        }

        public int EventTypeGroupReference { get; set; }
        public string EventTypeGroupDescription { get; set; }
        public int SortOrder { get; set; }

        public ICollection<TblEventType> TblEventType { get; set; }
    }
}
