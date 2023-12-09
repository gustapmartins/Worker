using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Worker.Model;

namespace Worker.Data;

public class TicketContext : IdentityDbContext<Users>
{
    public TicketContext(DbContextOptions<TicketContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

    }

    // Adicione propriedades DbSet para suas entidades
    public DbSet<Category> Categorys { get; set; }
    public DbSet<Tickets> Tickets { get; set; }
    public DbSet<Show> Shows { get; set; }
    public DbSet<Carts> Carts { get; set; }

}
