
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WasteDetection.Models.Entities
{
    public class PreTrainedDetectionExample
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        [ForeignKey("DetectionTraining")]
        public int FkDetectionTraining { get; set; }

        [ForeignKey("DetectionClassification")]
        public int FkDetectionClassification { get; set; }

        [NotMapped]
        public virtual TrainImageClassificatierRequest? DetectionTraining { get; set; }

        [NotMapped]
        public virtual ImageClassificationRequest? DetectionClassification { get; set; }
    }
}
