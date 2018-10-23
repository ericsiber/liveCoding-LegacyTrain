using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class Train
    {
        private readonly string _id;
        private readonly IList<Seat> _seats;

        public Train(string id, IList<Seat> seats)
        {
            _id = id;
            _seats = seats;
        }

        public DomainEvent TryToBook(int seatsRequestedCount, string bookingReference)
        {
            if (!CanReserve(seatsRequestedCount))
            {
                return new SeatsBookedFailedBecauseNotEnoughAvailableSeats(_id);
            }

            var availableSeats = FindAvailableSeats(seatsRequestedCount);
            var numberOfReservation = 0;

            foreach (var availableSeat in availableSeats)
            {
                availableSeat.BookingRef = bookingReference;
                
                numberOfReservation++;
            }

            if (numberOfReservation == seatsRequestedCount)
            {
                return new SeatsBooked(_id, availableSeats,bookingReference);
            }
            return new SeatsBookedFailedBecauseNotEnoughAvailableSeats(_id);
        }

        private bool CanReserve(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= GetAvailableSeatsForReservation();
        }

        private List<Seat> FindAvailableSeats(int seatsRequestedCount)
        {
            var availableSeats = new List<Seat>();
            var numberUnreservedSeats = 0;

            foreach (var seat in _seats)
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

            return availableSeats;
        }

        private int GetNbSeats()
        {
            return _seats.Count;
        }

        private int ReservedSeats
        {
            get { return _seats.Count(s => !string.IsNullOrEmpty(s.BookingRef)); }
        }

        private int GetAvailableSeatsForReservation()
        {
            return (int)Math.Floor(ThresholdManager.GetReservationMaxPercent() * GetNbSeats());
        }
    }
}