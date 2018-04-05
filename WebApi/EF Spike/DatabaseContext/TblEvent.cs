using System;
using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblEvent
    {
        public int EventReference { get; set; }
        public short EventType { get; set; }
        public int? Psrnumber { get; set; }
        public short? SectionNumber { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string TransactionId { get; set; }
        public int? SystemBatchReference { get; set; }
        public DateTime? NotificationDate { get; set; }
        public short EventSourceReference { get; set; }

        public TblEventSource EventSourceReferenceNavigation { get; set; }
        public TblEventType EventTypeNavigation { get; set; }
    }
}
