using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Train
    {
        private readonly string _id;
        private readonly IList<Seat> _seats;
        private const decimal CapacityRatioThreshold = 0.7M;

        public Train(string id, IList<Seat> seats)
        {
            _id = id;
            _seats = seats;
        }

        public DomainEvent TryToReserve(int seatsRequestedCount, string bookingReference)
        {
            if (!CanReserve(seatsRequestedCount))
            {
                return new SeatsReservedFailedBecauseNotEnoughAvailableSeats(_id);
            }

            var availableSeats = FindAvailableSeats(seatsRequestedCount);
            if (availableSeats.Count == seatsRequestedCount)
            {
                return new SeatsReserved(_id, availableSeats,bookingReference);
            }
            return new SeatsReservedFailedBecauseNotEnoughAvailableSeats(_id);
        }

        private bool CanReserve(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= GetAvailableSeatsForReservation();
        }

        private List<Seat> FindAvailableSeats(int seatsRequestedCount)
        {
            var availableSeats = new List<Seat>();
            var numberUnreservedSeats = 0;

            foreach (var seatsOfCoach in _seats.GroupBy(seat => seat.CoachName))
            {
                var coach = seatsOfCoach.Key;
                var seats = seatsOfCoach;

                if (seats.Count(seat => seat.IsNotReserved()) < seatsRequestedCount) continue;

                foreach (var seat in seats)
                {
                    if (seat.IsNotReserved())
                    {
                        numberUnreservedSeats++;
                        if (numberUnreservedSeats <= seatsRequestedCount)
                        {
                            availableSeats.Add(seat);
                        }
                    }
                }
            }

            return availableSeats;
        }

        private int GetNbSeats()
        {
            return _seats.Count;
        }

        private int ReservedSeats
        {
            get { return _seats.Count(seat => !seat.IsNotReserved()); }
        }

        private int GetAvailableSeatsForReservation()
        {
            return (int)Math.Floor(CapacityRatioThreshold * GetNbSeats());
        }
    }
}