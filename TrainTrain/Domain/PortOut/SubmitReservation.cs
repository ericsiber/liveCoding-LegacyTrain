using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainTrain.Domain.PortOut
{
    public interface SubmitReservation
    {
        Task Execute(TrainId trainId, string bookingReference, IList<Seat> seats);
    }
}
