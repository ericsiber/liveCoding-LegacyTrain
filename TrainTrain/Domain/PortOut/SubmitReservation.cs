using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainTrain.Domain.PortOut
{
    public interface SubmitReservation
    {
        Task Execute(string trainId, string bookingReference, IList<Seat> seats);
    }
}
