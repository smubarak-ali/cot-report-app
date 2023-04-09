namespace COTReport.Common.Model
{
    public class MyFxbookmodel
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public string Session { get; set; }
        public List<SentimentModel> Symbols { get; set; }
    }

    public class SentimentModel
    {
        public string Name { get; set; }
        public int ShortPercentage { get; set; }
        public int LongPercentage { get; set; }

    }
}