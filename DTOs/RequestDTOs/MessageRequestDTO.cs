namespace RealEstate_WebAPI.DTOs.RequestDTOs
{
    public class MessageRequestDTO
    {
        public int PropertyId { get; set; }
        public string AgentId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPhone { get; set; }
        public string MessageText { get; set; }
    }
}