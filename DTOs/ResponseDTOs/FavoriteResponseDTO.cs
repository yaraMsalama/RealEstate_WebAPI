namespace RealEstate_WebAPI.DTOs.ResponseDTOs
{
    public class FavoriteResponseDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}