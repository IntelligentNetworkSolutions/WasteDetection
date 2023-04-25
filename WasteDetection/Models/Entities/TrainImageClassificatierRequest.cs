using System.ComponentModel.DataAnnotations;

namespace WasteDetection.Models.Entities
{
    public class TrainImageClassificatierRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string InpImgPath { get; set; }
        public string InpVectorPath { get; set; }
        public string ValidationVectorPath { get; set; }
        public string InpXmlStatisticsPath { get; set; }
        public string OutModelPath { get; set; }
        public string OutConfusionMatrixPath { get; set; }
        public string LabelField { get; set; }
        public string TrainingClassifierName { get; set; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
