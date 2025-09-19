namespace AuctionPortal.Data.Models;

public class Bid
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
    public int AuctionItemId { get; set; }
    public AuctionItem AuctionItem { get; set; }
}
