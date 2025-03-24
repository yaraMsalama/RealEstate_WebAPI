namespace RealEstate_WebAPI.ViewModels.Property
{
    public class PropertyImagesViewModel
    {
        public int Id { get; set; }
        public IFormFile FeaturedImage { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public IEnumerable<string>? Images { get; set; }
    }
}
