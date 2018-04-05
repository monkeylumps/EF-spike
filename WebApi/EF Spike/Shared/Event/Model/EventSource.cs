namespace EF_Spike.Shared.Model
{
    public class EventSource
    {
        public short EventSourceReference { get; set; }
        public string EventSourceDescription { get; set; }
        public int SortOrder { get; set; }
    }
}