using System.ComponentModel.DataAnnotations;

namespace WasteDetection.Models.Entities
{
    public class RasterCalculatorRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string InpLayerAPath { get; set; }
        public string InpLayerBPath { get; set; }
        public string OutLayerPath { get; set; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
