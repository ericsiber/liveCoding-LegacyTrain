using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Domain.PortOut;

namespace TrainTrain.Infrastructure.AdapterOut
{
    public class SubmitReservationAdapter: SubmitReservation
    {
        private readonly ITrainDataService _trainDataService;

        public SubmitReservationAdapter(ITrainDataService trainDataService)
        {
            _trainDataService = trainDataService;
        }

        public Task Execute(string trainId, string bookingReference, IList<Seat> seats)
        {
            return _trainDataService.SubmitReservation(trainId, bookingReference, seats.ToList());
        }
    }
}
