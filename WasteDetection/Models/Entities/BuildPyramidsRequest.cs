namespace WasteDetection.Models.Entities
{
    public class BuildPyramidsRequest
    {
        public Guid Id { get; set; }
        public string InpLayerPath { get; set; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
