using Microsoft.EntityFrameworkCore;
using TranslationManagement.Core.Domain;
using TranslationManagement.Persistence.Abstractions;

namespace TranslationManagement.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
	    public DbSet<Translator> Translators { get; set; }
	    public DbSet<TranslationJob> TranslationJobs { get; set; }

	    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

	    protected override void OnModelCreating(ModelBuilder modelBuilder)
	    {
		    modelBuilder.Entity<Translator>().ToTable("Translators");
		    modelBuilder.Entity<TranslationJob>().ToTable("TranslationJobs");
	    }
    }
}