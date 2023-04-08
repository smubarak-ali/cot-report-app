using COTReport.App.DTO;
using COTReport.DAL.Entity;

namespace COTReport.App.Mapper
{
    public static class DtoToEntityMapper
    {

        public static Report ToEntity(this ReportDto obj, string code)
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
    }
}