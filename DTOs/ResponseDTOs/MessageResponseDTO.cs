namespace RealEstate_WebAPI.DTOs.ResponseDTOs
{
    public class MessageResponseDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string AgentId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPhone { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}