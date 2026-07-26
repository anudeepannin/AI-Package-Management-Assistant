namespace PackageManagement.Models
{
    public class Package
    {
        public int PackageId { get; set; }

        public string PackageName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string OwnerName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
