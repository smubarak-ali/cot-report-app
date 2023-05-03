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
            await _dbContext.Sentiment.AddRangeAsync(list);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Sentiment>> GetSentimentsAsync()
        {
            return await _dbContext.Sentiment.ToListAsync();
        }

    }
}