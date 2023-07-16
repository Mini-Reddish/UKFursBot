using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UKFursBot.Entities;

namespace UKFursBot.Context;

public partial class UKFursBotDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public virtual DbSet<AnnouncementMessage> AnnouncementMessages { get; set; }
    public virtual DbSet<BotConfiguration> BotConfigurations { get; set; }
    public virtual DbSet<Warning> Warnings { get; set; }
    public virtual DbSet<UserNote> UserNotes { get; set; }
    public virtual DbSet<BanOnJoin> BansOnJoin { get; set; }
    public virtual DbSet<ModMail> ModMails { get; set; }
    public virtual DbSet<ErrorLogging> ErrorLogging { get; set; }
    
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
