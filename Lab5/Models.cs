using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace Lab5.Models
{
    public class Lab5Context : DbContext
    {
        public Lab5Context(): base(){ }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["L5"].ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasOne(p => p.Basket)
            .WithOne(i => i.User)
            .HasForeignKey<Basket>(b => b.UserId);

            modelBuilder.Entity<UserRole>()
                .HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.Role)
                .WithMany(t => t.UserRoles)
                .HasForeignKey(pt => pt.RoleId);
        }
    }

    public static class DbInitializer
    {
        public static void Initialize(Lab5Context context)
        {
            context.Database.EnsureCreated();

            var users = new User[]
            {
            new User{Surname="Carson",Name="Alexander"},
            new User{Surname="Meredith",Name="Alonso"},
            new User{Surname="Arturo",Name="Anand"},

            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var roles = new Role[]
            {
            new Role{Name="Student"},
            new Role{Name="Proffesor"},
            new Role{Name="Worker"}
            };
            foreach (Role r in roles)
            {
                context.Roles.Add(r);
            }
            context.SaveChanges();

            var baskets = new Basket[]
            {
            new Basket{Name="first", User=users[0]},
            new Basket{Name="second", User=users[1]},
            new Basket{Name="third", User=users[2]}
            };
            foreach (Basket b in baskets)
            {
                context.Baskets.Add(b);
            }
            context.SaveChanges();
        }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "User`s name length should be less then 50 characters")]
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Basket Basket { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }

    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Role`s name length should be less then 50 characters")]
        public string Name { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }

    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class Basket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Basket`s name length should be less then 50 characters")]
        public string Name { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
