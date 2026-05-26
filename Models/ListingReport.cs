namespace TradeNest.Models
{
    public enum ReportStatus
    {
        Pending = 0,
        PostRemoved = 1,
        UserBanned = 2,
        FalseReport = 3
    }

    public class ListingReport
    {
        public int Id { get; set; }

        public string Reason { get; set; } = null!;

        public DateTime ReportedAt { get; set; } = DateTime.Now;

        public ReportStatus Status { get; set; } = ReportStatus.Pending;

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;

        public int ReporterId { get; set; }
        public User Reporter { get; set; } = null!;
    }
}
