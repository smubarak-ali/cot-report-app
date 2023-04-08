namespace COTReport.DAL.Entity
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public string Instrument { get; set; }
        public string Code { get; set; }
        public decimal TotalLong { get; set; }
        public decimal TotalShort { get; set; }
        public decimal? ChangeInLong { get; set; }
        public decimal? ChangeInShort { get; set; }
        public decimal? PercentOfLong { get; set; }
        public decimal? PercentOfShort { get; set; }

    }
}