using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Coach
    {
        private readonly string _name;
        private readonly IEnumerable<Seat> _seats;

        public Coach(string name, IEnumerable<Seat> seats)
        {
            _name = name;
            _seats = seats;
        }

        public bool CanReserve(int seatsRequestedCount)
        {
            return GetSeatNotReserved().Count() >= seatsRequestedCount;
        }

        public IEnumerable<Seat> GetSeatNotReserved()
        {
            return _seats.Where(seat => seat.IsNotReserved());
        }

        public Reservation TryReserve(int seatsRequestedCount)
        {
            if (CanReserve(seatsRequestedCount))
            {
                return new SuccessReservation(GetSeatNotReserved().Take(seatsRequestedCount).ToList());
            }

            return new FailReservation();
        }
    }
}