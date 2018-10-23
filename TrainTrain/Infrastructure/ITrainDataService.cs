using System.Collections.Generic;
using System.Threading.Tasks;
using TrainTrain.Domain;

namespace TrainTrain.Infrastructure
{
    public interface ITrainDataService
    {
        Task<Train> GetTrain(TrainId trainId);
        Task SubmitReservation(TrainId trainId, string bookingRef, List<Seat> availableSeats);
    }
}