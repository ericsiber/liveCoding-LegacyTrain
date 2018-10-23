using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Train
    {
        private readonly string _id;
        private readonly IList<Seat> _seats;
        private readonly List<Coach> _coaches;
        private const decimal CapacityRatioThreshold = 0.7M;


        public Train(string id, IList<Seat> seats)
        {
            _id = id;
            _seats = seats;
            _coaches = seats.GroupBy(seat => seat.CoachName).Select(coach => new Coach(coach.Key, coach)).ToList();

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
            return _coaches.Where(coach => coach.CanReserve(seatsRequestedCount))
                .Select(coach => coach.GetSeatNotReserved().Take(seatsRequestedCount).ToList()).FirstOrDefault()??new List<Seat>();

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