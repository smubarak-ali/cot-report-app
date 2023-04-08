using COTReport.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace COTReport.DAL.Context
{
    public class ReportDbContext : DbContext
    {

        public DbSet<Report>? Report { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STR"));

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Report>().ToTable("cot_reports");
            builder.Entity<Report>().HasKey(x => x.Id);

            builder.Entity<Report>()
                    .Property(x => x.Id)
                    .HasColumnName("id");

            builder.Entity<Report>()
                    .Property(x => x.Instrument)
                    .HasColumnName("instrument");

            builder.Entity<Report>()
                    .Property(x => x.Code)
                    .HasColumnName("code");

            builder.Entity<Report>()
                    .Property(x => x.ChangeInLong)
                    .HasColumnName("change_long");

            builder.Entity<Report>()
                    .Property(x => x.ChangeInShort)
                    .HasColumnName("change_short");

            builder.Entity<Report>()
                    .Property(x => x.PercentOfLong)
                    .HasColumnName("percent_long");

            builder.Entity<Report>()
                    .Property(x => x.PercentOfShort)
                    .HasColumnName("percent_short");

            builder.Entity<Report>()
                    .Property(x => x.ReportDate)
                    .HasColumnName("report_date");

            builder.Entity<Report>()
                    .Property(x => x.TotalLong)
                    .HasColumnName("total_long");

            builder.Entity<Report>()
                    .Property(x => x.TotalShort)
                    .HasColumnName("total_short");

            base.OnModelCreating(builder);
        }
    }
}