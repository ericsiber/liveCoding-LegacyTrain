using System.Collections.Generic;
using System.Threading.Tasks;
using TrainTrain.Domain;

namespace TrainTrain.Infrastructure
{
    public interface ITrainDataService
    {
        Task<Train> GetTrain(string trainId);
        Task SubmitReservation(string trainId, string bookingRef, List<Seat> availableSeats);
    }
}