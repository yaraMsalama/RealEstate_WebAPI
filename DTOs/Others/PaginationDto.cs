namespace RealEstate_WebAPI.DTOs.Others
{
    public class PaginationDto
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
    }
}
