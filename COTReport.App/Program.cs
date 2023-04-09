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

try
{
    string filePath = Environment.GetEnvironmentVariable("FULL_FILE_PATH");

    using (var reader = new StreamReader(filePath))
    using (var csv = new CsvReader(reader, culture: System.Globalization.CultureInfo.InvariantCulture))
    {
        csv.Context.RegisterClassMap<ReportCsvModelMap>();
        var records = csv.GetRecords<ReportCsvModel>().ToList();

        var cotRepo = serviceProvider.GetService<ReportRepository>();
        var entityList = new List<Report>();
        foreach (var record in records)
        {
            if (string.Equals(record.Instrument, Constants.InstrumentName.GOLD, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.GOLD));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.CAD, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.CAD));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.CHF, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.CHF));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.GBP, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.GBP));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.JPY, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.JPY));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.USD, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.USD));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.EUR, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.EUR));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.NZD, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.NZD));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.AUD, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.AUD));
            }
            else if (string.Equals(record.Instrument, Constants.InstrumentName.BITCOIN, StringComparison.OrdinalIgnoreCase))
            {
                entityList.Add(record.ToEntity(code: Constants.InstrumentCode.BTC));
            }
        }

        cotRepo?.SaveRecords(entityList);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}




