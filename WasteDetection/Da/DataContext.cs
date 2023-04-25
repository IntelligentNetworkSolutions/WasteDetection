using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using WasteDetection.Models.Entities;

namespace WasteDetection.Da
{
    public class DataContext : DbContext
    {
        public DbSet<ComputeImageStatisticsRequest> ComputeImageStatisticsRequests { get; set; }
        public DbSet<TrainImageClassificatierRequest> TrainImageClassificatierRequests { get; set; }
        public DbSet<ImageClassificationRequest> ImageClassificationRequests { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
