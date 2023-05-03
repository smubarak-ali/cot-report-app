namespace COTReport.DAL.Entity
{
    public class Sentiment
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int ShortPercentage { get; set; }
        public int LongPercentage { get; set; }
        public int LongPosition { get; set; }
        public int ShortPosition { get; set; }
        public DateTime RecordDate { get; set; }

    }
}