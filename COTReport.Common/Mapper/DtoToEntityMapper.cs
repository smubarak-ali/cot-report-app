using COTReport.Common.Model;
using COTReport.DAL.Entity;

namespace COTReport.Common.Mapper
{
    public static class DtoToEntityMapper
    {

        public static Report ToEntity(this ReportCsvModel obj, string code)
        {
            if (obj == null) return new Report();

            var entity = new Report();
            entity.Instrument = obj.Instrument;
            entity.ChangeInLong = obj.ChangeInLongAll;
            entity.ChangeInShort = obj.ChangeInShortAll;
            entity.PercentOfLong = obj.PercentOfLongAll;
            entity.PercentOfShort = obj.PercentOfShortAll;
            entity.ReportDate = obj.Date;
            entity.TotalLong = obj.LongAll;
            entity.TotalShort = obj.ShortAll;
            entity.Code = code;
            return entity;
        }

        public static List<Sentiment> ToEntity(this List<SentimentModel> models)
        {
            var entityList = new List<Sentiment>();
            foreach (var item in models)
            {
                var entity = new Sentiment();
                entity.LongPercentage = item.LongPercentage;
                entity.LongPosition = item.LongPositions;
                entity.Name = item.Name;
                entity.RecordDate = DateTime.Now;
                entity.ShortPercentage = item.ShortPercentage;
                entity.ShortPosition = item.ShortPositions;
                entityList.Add(entity);
            }

            return entityList;
        }
    }
}