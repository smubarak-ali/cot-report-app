using COTReport.DAL.Context;
using COTReport.DAL.Entity;

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
            foreach (Report cot in list)
            {
                _dbContext.Report.Add(cot);
            }

            _dbContext.SaveChanges();
        }
    }
}