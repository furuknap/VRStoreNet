namespace VRStore.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VRStore.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<VRStore.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "VRStore.Models.ApplicationDbContext";
        }

        protected override void Seed(VRStore.Models.ApplicationDbContext context)
        {
            context.Videos.AddOrUpdate(
                new Video
                {
                    ID = new Guid("fb51339f-da71-401d-99a2-20ab77e660e9"),
                    Title = "Spider Man",
                    ReleaseDate = new DateTime(2017, 2, 12)
                },
                new Video
                {
                    ID = new Guid("7f86ff69-8c68-4e76-a773-558310ae949b"),
                    Title = "Out of Africa",
                    ReleaseDate = new DateTime(1996, 6, 30)
                },
                new Video
                {
                    ID = new Guid("4fc55c37-bda6-4156-95ba-7356804f1d5f"),
                    Title = "Spider Man 2",
                    ReleaseDate = new DateTime(2018, 4, 23)
                },
                new Video
                {
                    ID = new Guid("991b7277-7746-4ab0-b120-82652c1416ab"),
                    Title = "Matrix 11",
                    ReleaseDate = DateTime.UtcNow.AddDays(-1)
                }



            );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
