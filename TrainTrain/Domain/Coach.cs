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
            return _seats.Count(seat => seat.IsNotReserved()) >= seatsRequestedCount;
        }

        public IEnumerable<Seat> GetSeatNotReserved()
        {
            return _seats.Where(seat => seat.IsNotReserved());
        }
    }
}