using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace TrainTrain.Domain
{
    public interface Reservation
    {
        bool Success();
        List<Seat> Seats();
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

        public List<Seat> Seats()
        {
            return _seats.ToList();
        }
    }

    public class FailReservation : Reservation
    {
        public bool Success()
        {
            return false;
        }

        public List<Seat> Seats()
        {
            return new List<Seat>();
        }
    }
}