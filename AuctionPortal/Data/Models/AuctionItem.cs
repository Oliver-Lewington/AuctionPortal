namespace AuctionPortal.Data.Models;

public class AuctionItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal ReservePrice { get; set; }
    public DateTime EndTime { get; set; }
    public List<Bid> Bids { get; set; } = new();
}