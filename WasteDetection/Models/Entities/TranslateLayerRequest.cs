using System.ComponentModel.DataAnnotations;

namespace WasteDetection.Models.Entities
{
    public class TranslateLayerRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string InpLayerPath { get; set; }
        public string OutLayerPath { get; set; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
