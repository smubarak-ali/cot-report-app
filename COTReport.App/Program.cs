using COTReport.Common.Helper;
using COTReport.Common.Mapper;
using COTReport.Common.Model;
using COTReport.DAL.Context;
using COTReport.DAL.Entity;
using COTReport.DAL.Repository;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDbContext<ReportDbContext>();
services.AddSingleton<ReportRepository>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
ServiceProvider serviceProvider = services.BuildServiceProvider();

ReportRepository cotRepo;
List<Report> existingList;
var entityList = new List<Report>();

try
{
    string filePath = Environment.GetEnvironmentVariable("FULL_FILE_PATH");

    using (var reader = new StreamReader(filePath))
    using (var csv = new CsvReader(reader, culture: System.Globalization.CultureInfo.InvariantCulture))
    {
        csv.Context.RegisterClassMap<ReportCsvModelMap>();
        var records = csv.GetRecords<ReportCsvModel>().ToList();
        cotRepo = serviceProvider.GetService<ReportRepository>();
        existingList = cotRepo.GetReport();

        foreach (var record in records)
        {
            if (string.Equals(record.Instrument, Constants.InstrumentName.GOLD, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.GOLD, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.CAD, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.CAD, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.CHF, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.CHF, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.GBP, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.GBP, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.JPY, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.JPY, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.USD, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.USD, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.EUR, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.EUR, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.NZD, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.NZD, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.AUD, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.AUD, record);
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.BITCOIN, StringComparison.OrdinalIgnoreCase))
            {
                ValidationOfRecord(Constants.InstrumentCode.BTC, record);
            }
        }

        cotRepo?.SaveRecords(entityList);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

void ValidationOfRecord(string code, ReportCsvModel record)
{
    var existingRecords = existingList.Where(x => x.Code.ToLower().Equals(code.ToLower()));
    if (existingRecords != null)
    {
        bool isMatched = existingRecords.Any(x => DateTime.Compare(x.ReportDate.Date, record.Date.Date) == 0);

        if (!isMatched)
        {
            entityList.Add(record.ToEntity(code));
        }
    }
}


