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

            var seatsByCoach = _seats
                .GroupBy(seat => seat.CoachName)
                .Where(coach => coach.Count(seat => seat.IsNotReserved()) >= seatsRequestedCount);
            foreach (var seatsOfCoach in seatsByCoach)
            {
                foreach (var seat in seatsOfCoach.Where(seat => seat.IsNotReserved()))
                {
                    numberUnreservedSeats++;
                    if (numberUnreservedSeats <= seatsRequestedCount)
                    {
                        availableSeats.Add(seat);
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