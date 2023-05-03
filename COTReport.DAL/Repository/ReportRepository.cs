using COTReport.DAL.Context;
using COTReport.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace COTReport.DAL.Repository
{
    public class ReportRepository
    {
        private readonly ReportDbContext _dbContext;

        public ReportRepository(ReportDbContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public void SaveRecords(List<Report> list)
        {
            _dbContext.Report.AddRange(list);
            _dbContext.SaveChanges();
            return;
        }

        public List<Report> GetReport()
        {
            return _dbContext.Report.OrderByDescending(x => x.ReportDate).ToList();
        }

        public async Task<List<Report>> GetReportAsync()
        {
            return await _dbContext.Report.OrderByDescending(x => x.ReportDate).ToListAsync();

        }

        public List<Report> GetReportByCode(string code)
        {
            return _dbContext.Report.Where(x => x.Code.ToLower().Equals(code.ToLower())).OrderByDescending(x => x.ReportDate).ToList();
        }

        public async Task<List<Report>> GetReportByCodeAsync(string code)
        {
            return await _dbContext.Report.Where(x => x.Code.ToLower().Equals(code.ToLower())).OrderByDescending(x => x.ReportDate).ToListAsync();
        }
    }
}