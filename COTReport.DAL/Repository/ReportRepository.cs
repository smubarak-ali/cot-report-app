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
            _dbContext.Report.AddRange(list);
            _dbContext.SaveChanges();
            return;
        }

        public List<Report> GetReport()
        {
            return _dbContext.Report.OrderBy(x => x.Code).ToList();
        }

        public List<Report> GetReportByCode(string code)
        {
            return _dbContext.Report.Where(x => x.Code.ToLower().Equals(code.ToLower())).OrderBy(x => x.ReportDate).ToList();
        }
    }
}