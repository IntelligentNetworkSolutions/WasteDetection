namespace WasteDetection.Models.Entities
{
    public class PolygonizeRequest
    {
        public Guid Id { get; set; }
        public string InpLayerPath { get; set; }
        public string OutVectorizedPath { get; set; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
