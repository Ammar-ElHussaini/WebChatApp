using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebChatApp.Data_Acess_Layer.Models.DB;

namespace WebChatApp.Models;

public partial class WebchatAppContext : IdentityDbContext<ApplicationUser>
{
    public WebchatAppContext()
    {
    }

    public WebchatAppContext(DbContextOptions<WebchatAppContext> options)
        : base(options)
    {
    }


    public DbSet<TbGroupsMessages> GroupsMessages { get; set; }
    public DbSet<TbGroups> Groups { get; set; }
    public DbSet<TbGroupMember> GroupMembers { get; set; }
    public virtual DbSet<TbSendMessage> TbSendMessages { get; set; }

    public virtual DbSet<TbmangeGroup> TbmangeGroups { get; set; }

    public virtual DbSet<TbUserOnline> UserOnlines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=WebchatApp; Trusted_Connection = true; trustservercertificate =true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TbGroupMember>()
            .HasKey(gm => new { gm.GroupId, gm.UserId });

        modelBuilder.Entity<TbGroupMember>()
            .HasOne(gm => gm.Group)
            .WithMany(gm => gm.UserId)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TbGroupsMessages>()
            .HasKey(m => m.MessageId);

        modelBuilder.Entity<TbGroupsMessages>()
            .HasOne(m => m.Group)
            .WithMany(g => g.Messages)
            .HasForeignKey(m => m.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TbSendMessage>()
            .HasKey(m => m.IdPr);

        modelBuilder.Entity<TbSendMessage>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(m => m.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TbSendMessage>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(m => m.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TbUserOnline>()
            .HasKey(uo => uo.IdPr);

        modelBuilder.Entity<TbmangeGroup>()
            .HasNoKey();

        modelBuilder.Entity<TbMessageReadStatusGroups>()
            .HasKey(r => new { r.GroupId, r.UserId, r.MessageId });

        modelBuilder.Entity<TbMessageReadStatusGroups>()
            .HasOne<TbGroupsMessages>()
            .WithMany()
            .HasForeignKey(r => r.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TbMessageReadStatusGroups>()
            .HasOne<TbGroups>()
            .WithMany()
            .HasForeignKey(r => r.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ErrorLog>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<TbGroups>()
            .Property(g => g.GroupName)
            .HasMaxLength(250);

        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.ProfileImage)
            .HasMaxLength(500);

        modelBuilder.Entity<TbGroupsMessages>()
            .Property(m => m.MessageContent)
            .HasMaxLength(1000);

        modelBuilder.Entity<TbGroupsMessages>()
            .Property(m => m.EncryptedMessage)
            .HasMaxLength(1000);

        modelBuilder.Entity<TbGroupsMessages>()
            .Property(m => m.EncryptedKey)
            .HasMaxLength(1000);

        modelBuilder.Entity<TbGroupsMessages>()
            .Property(m => m.Iv)
            .HasMaxLength(1000);

        modelBuilder.Entity<TbGroupsMessages>()
            .Property(m => m.DecryptedMessage)
            .HasMaxLength(1000);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
