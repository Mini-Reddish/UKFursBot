using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UKFursBot.Entities;

namespace UKFursBot.Context;

public partial class UKFursBotDbContext : DbContext
{
    private readonly IConfiguration _configuration = null!;

    public virtual DbSet<AnnouncementMessage> AnnouncementMessages { get; set; } = null!;
    public virtual DbSet<BotConfiguration> BotConfigurations { get; set; } = null!;
    public virtual DbSet<Warning> Warnings { get; set; } = null!;
    public virtual DbSet<UserNote> UserNotes { get; set; } = null!;
    public virtual DbSet<BanOnJoin> BansOnJoin { get; set; } = null!;
    public virtual DbSet<ModMail> ModMails { get; set; } = null!;
    public virtual DbSet<ErrorLogging> ErrorLogging { get; set; } = null!;
    public DbSet<BanLog> BanLogs { get; set; } = null!;

    public UKFursBotDbContext()
    {
    }

    public UKFursBotDbContext(DbContextOptions<UKFursBotDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("db"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
