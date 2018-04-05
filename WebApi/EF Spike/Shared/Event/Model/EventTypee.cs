namespace EF_Spike.Shared.Model
{
    public class EventTypee
    {
        public short EventType { get; set; }
        public string EventTypeDescription { get; set; }
        public short SortOrder { get; set; }
        public int EventTypeGroupReference { get; set; }
        public bool? ScoreUivisible { get; set; }
    }
}