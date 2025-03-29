namespace RealEstate_WebAPI.DTOs.Others
{
    public class PropertyImagesDto
    {
        public int PropertyId { get; set; }
        public IFormFile FeaturedImage { get; set; }
        public IEnumerable<IFormFile> NewImages { get; set; }
    }
}
