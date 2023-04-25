using System.ComponentModel.DataAnnotations;

namespace WasteDetection.Models.Entities
{
    public class ImageClassificationRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string InpImgPath { get; set; }
        public string InpModelPath { get; set; }
        public string InpXmlStatisticsPath { get; set; }
        public string OutRasterPath { get; set; }
        public string OutConfidenceMapPath { get; set; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
