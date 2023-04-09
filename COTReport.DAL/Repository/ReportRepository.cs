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
            var entityList = _dbContext.Report.ToList();
            if (entityList == null || entityList.Count == 0)
            {
                _dbContext.Report.AddRange(list);
                _dbContext.SaveChanges();
                return;
            }
        }

        public List<Report> GetReport()
        {
            return _dbContext.Report.ToList();
        }
    }
}