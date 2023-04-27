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
        public DbSet<TranslateLayerRequest> TranslateLayerRequests { get; set; }
        public DbSet<BuildPyramidsRequest> BuildPyramidsRequests { get; set; }
        public DbSet<RasterCalculatorRequest> RasterCalculatorRequests { get; set; }
        public DbSet<SieveRequest> SieveRequests { get; set; }
        public DbSet<ContourRequest> ContourRequests { get; set; }
        public DbSet<PolygonizeRequest> PolygonizeRequests { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
