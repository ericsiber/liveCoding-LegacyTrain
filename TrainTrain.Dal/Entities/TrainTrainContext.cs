using System.Data.Entity;

namespace TrainTrain.Dal.Entities
{
    public class TrainTrainContext : DbContext
    {
        public TrainTrainContext()
            : base("Server=(localdb)\\MSSQLLocalDB")
        {
            Configuration.LazyLoadingEnabled = false;
        }


        public DbSet<TrainEntity> Trains { get; set; }
        public DbSet<SeatEntity> Seats { get; set; }
    }
}