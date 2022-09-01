using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;

namespace OnlineLibrary.Data
{
    public class LibraryDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }






        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {

        }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorToBook>().HasKey(ab => new { ab.AuthorId, ab.BookId });
           
            modelBuilder.Entity<AuthorToBook>()
             .HasOne(ab => ab.Author)
             .WithMany(ab => ab.AuthorToBook)
             .HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<AuthorToBook>()
            .HasOne(ab => ab.Book)
            .WithMany(ab => ab.AuthorToBook)
            .HasForeignKey(b => b.BookId);

            base.OnModelCreating(modelBuilder);

            var adminRoleId = Guid.NewGuid().ToString();
            var adminUserId = Guid.NewGuid().ToString();

            modelBuilder.Entity<AppRole>().HasData(new List<AppRole>
            {
                new AppRole {
                 Id = adminRoleId,
                 Name = "Admin",
                 NormalizedName = "ADMIN"
                 },
               new AppRole {
               Id = Guid.NewGuid().ToString(),
               Name = "Staff",
               NormalizedName = "STAFF"
               }
             });

            var hasher = new PasswordHasher<AppUser>();

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = adminUserId, // primary key
                    UserName = "admin@gotech.ps",
                    NormalizedUserName = "ADMIN@GOTECH.PS",
                    PasswordHash = hasher.HashPassword(null, "123@Qwe"),
                    Email = "admin@gotech.ps"
                }

            );


            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId, // for admin username
                    UserId = adminUserId  // for admin role
                }

            );

        }
       


        public DbSet<OnlineLibrary.Models.AuthToBookViewModel>? AuthToBookViewModel { get; set; }
      
    }
}
