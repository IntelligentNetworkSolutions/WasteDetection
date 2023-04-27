using System.ComponentModel.DataAnnotations;

namespace WasteDetection.Models.Entities
{
    public class ContourRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string InpLayerPath { get; set; }
        public string OutContoursPath { get; set; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
