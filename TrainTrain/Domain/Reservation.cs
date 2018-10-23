using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace TrainTrain.Domain
{
    public interface Reservation
    {
        bool Success();
        DomainEvent GetEvent(string trainId, string bookingReference);
    }

    public class SuccessReservation: Reservation
    {
        private readonly IList<Seat> _seats;

        public SuccessReservation(IList<Seat> seats)
        {
            _seats = seats;
        }

        public bool Success()
        {
            return true;
        }

        public DomainEvent GetEvent(string trainId, string bookingReference)
        {
            return new SeatsReserved(trainId, _seats.ToList(), bookingReference);
            
        }
    }

    public class FailReservation : Reservation
    {
        public bool Success()
        {
            return false;
        }

        public DomainEvent GetEvent(string trainId, string bookingReference)
        {
            return new SeatsReservedFailedBecauseNotEnoughAvailableSeats(trainId);
        }
    }
}