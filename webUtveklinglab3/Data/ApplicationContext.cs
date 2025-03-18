using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public partial class ApplicationContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC079E237D89");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Link).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Skills__3214EC07E30254E4");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SkillLevel).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

