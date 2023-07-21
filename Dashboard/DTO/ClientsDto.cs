namespace Dashboard.DTO
{
    public class ClientsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubDomain { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SftpId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        public DateTime DateOfRequest { get; set; }
        public DateTime? DateOfAproval { get; set; }
        public DateTime? DateOfDiscontinue { get; set; }
        public string? Logo { get; set; }
        public IFormFile? LogoImage { get; set; }
    }
}
