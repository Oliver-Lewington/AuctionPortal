using AuctionPortal.Data.Models;
using Microsoft.EntityFrameworkCore;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

    public DbSet<AuctionItem> Items { get; set; }
    public DbSet<Bid> Bids { get; set; }
}
