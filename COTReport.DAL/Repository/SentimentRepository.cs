using COTReport.DAL.Context;
using COTReport.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace COTReport.DAL.Repository
{
    public class SentimentRepository
    {
        private readonly ReportDbContext _dbContext;

        public SentimentRepository(ReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveRecords(List<Sentiment> list)
        {
            var existingRecords = await _dbContext.Sentiment.ToListAsync();
            if (existingRecords != null && existingRecords.Count > 0)
            {
                foreach (var record in existingRecords)
                {
                    var existingItem = list.Where(x => x.PairName.Equals(record.PairName)).FirstOrDefault();
                    if (existingItem != null)
                    {
                        record.LongPercentage = existingItem.LongPercentage;
                        record.LongPosition = existingItem.LongPosition;
                        record.ShortPercentage = existingItem.ShortPercentage;
                        record.ShortPosition = existingItem.ShortPosition;
                        record.RecordDate = DateTime.Now;
                        _dbContext.Sentiment.Update(record);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return;
            }

            await _dbContext.Sentiment.AddRangeAsync(list);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Sentiment>> GetSentimentsAsync()
        {
            return await _dbContext.Sentiment.ToListAsync();
        }

    }
}