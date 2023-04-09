namespace COTReport.Common.Model
{
    public class ReportCsvModel
    {
        public string? Instrument { get; set; }
        public DateTime Date { get; set; }
        public decimal LongAll { get; set; }
        public decimal ShortAll { get; set; }
        public decimal? ChangeInLongAll { get; set; }
        public decimal? ChangeInShortAll { get; set; }
        public decimal? PercentOfLongAll { get; set; }
        public decimal? PercentOfShortAll { get; set; }
    }
}