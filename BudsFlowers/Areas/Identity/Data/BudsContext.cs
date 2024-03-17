using BudsFlowers.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudsFlowers.Areas.Identity.Data;

public class BudsContext : IdentityDbContext<User>
{
    public BudsContext(DbContextOptions<BudsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
