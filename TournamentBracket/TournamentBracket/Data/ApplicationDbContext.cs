using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TournamentBracket.Data
{
    public class ApplicationDbContext : IdentityDbContext<TournamentBracket.Models.ApplicationUser>
    {
        public DbSet<TournamentBracket.Models.Tournament> TournamentBrackets { get; set; }
        public DbSet<TournamentBracket.Models.Match> Matches { get; set; }

        public DbSet<TournamentBracket.Models.Participant> Participants { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
