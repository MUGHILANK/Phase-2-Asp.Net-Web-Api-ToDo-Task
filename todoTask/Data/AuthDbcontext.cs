using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace todoTask.Data
{
    public class AuthDbcontext : IdentityDbContext
    {
        public AuthDbcontext(DbContextOptions options) : base(options)
        {
        }


        // OnmodelCreating Mehtod used to Seeding role
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var readerRoleId = "c203d655-f003-4c18-aa01-4514ab9883ac";
            var writeRoleId = "327941cf-926e-4cc9-9c93-8b10595c1641"; 

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole
                {
                    Id = writeRoleId,
                    ConcurrencyStamp = writeRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            /*
             When we run entity Framework Core migrations, Now it will See this data
            that we have to inject these two roles. If the roles don't exist in the
            Database, this entity Framework core migrations will add or seed this 
            data into the database
             */
            
            modelBuilder.Entity<IdentityRole>().HasData(roles);



        }
    }
}
