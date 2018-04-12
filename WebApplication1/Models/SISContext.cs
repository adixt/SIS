using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Models
{
    public partial class SISContext : DbContext
    {
        public SISContext(DbContextOptions<SISContext> options) : base(options)
        { }

        public virtual DbSet<UsersNotSercure> UsersNotSercure { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersNotSercure>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersNotSercure>().HasData(
                new UsersNotSercure
                {
                    Id = 1,
                    Name = "adam",
                    Password = "test"
                }, new UsersNotSercure
                {
                    Id = 2,
                    Name = "rafal",
                    Password = "test2"
                }, new UsersNotSercure
                {
                    Id = 3,
                    Name = "ewa",
                    Password = "test3"
                }, new UsersNotSercure
                {
                    Id = 4,
                    Name = "asia",
                    Password = "test4"
                });

        }

       
    }
}
