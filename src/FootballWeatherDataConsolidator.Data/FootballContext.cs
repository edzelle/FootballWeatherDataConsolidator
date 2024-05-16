using FootballWeatherDataConsolidator.Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace FootballWeatherDataConsolidator.Data
{
    public class FootballContext : DbContext
    {
        public FootballContext(DbContextOptions<FootballContext> options)
            : base(options)
        {
            string projectDirectory = Path.Join(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "database");
            DbPath = Path.Join(projectDirectory, "football.db");
        }

        public FootballContext()
        {
            string projectDirectory = Path.Join(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "database");
            DbPath = Path.Join(projectDirectory, "football.db");
        }

        public string DbPath { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<GameEntity> Games { get; set; }

        public DbSet<GameWeatherEntity> GameWeather { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }

        public DbSet<VenueEntity> Stadiums { get; set; }

        public DbSet<TeamPlaysInStadiumEntity> TeamPlaysInStadium { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GameEntity>().HasKey(x => new { x.Id });
            builder.Entity<GameEntity>().HasOne(gameWeather => gameWeather.GameWeatherEntity).WithOne(game => game.Game);
            builder.Entity<GameWeatherEntity>().HasKey(x => new { x.GameId });
            builder.Entity<TeamEntity>().HasKey(x => new { x.Id });
            builder.Entity<TeamEntity>().HasOne(venue => venue.TeamPlaysInStadium).WithOne(team => team.Team);
            builder.Entity<VenueEntity>().HasKey(x => new { x.Id });
            builder.Entity<VenueEntity>().HasMany(venue => venue.TeamPlaysInStadiumEntites).WithOne(x => x.Stadium);
            builder.Entity<TeamPlaysInStadiumEntity>().HasKey(x => new { x.TeamId, x.StadiumId });
        }

    }
}
