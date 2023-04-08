using System.Globalization;
using COTReport.App.DTO;
using CsvHelper.Configuration;

namespace COTReport.App.Mapper
{
    public sealed class ReportDtoMap : ClassMap<ReportDto>
    {
        public ReportDtoMap()
        {
            Map(x => x.Instrument).Name("Market_and_Exchange_Names");
            Map(x => x.Date).Name("As_of_Date_In_Form_YYMMDD")
                    .Convert(row => DateTime.ParseExact(row.Row.GetField("As_of_Date_In_Form_YYMMDD").ToString(), "yyMMdd", CultureInfo.InvariantCulture));
            Map(x => x.LongAll).Name("NonComm_Positions_Long_All");
            Map(x => x.ShortAll).Name("NonComm_Positions_Short_All");
            Map(x => x.ChangeInLongAll).Name("Change_in_NonComm_Long_All");
            Map(x => x.ChangeInShortAll).Name("Change_in_NonComm_Short_All");
            Map(x => x.PercentOfLongAll).Name("Pct_of_OI_NonComm_Long_All");
            Map(x => x.PercentOfShortAll).Name("Pct_of_OI_NonComm_Short_All");
        }
    }
}