using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class Train
    {
        private readonly string _id;

        public Train(string id, List<Seat> seats)
        {
            _id = id;
            Seats = seats;
        }

        private int GetNbSeats()
        {
            return Seats.Count;
        }

        private int ReservedSeats
        {
            get { return Seats.Count(s => !string.IsNullOrEmpty(s.BookingRef)); }
        }
        public List<Seat> Seats { get; }

        private int GetAvailableSeatsForReservation()
        {
            return (int)Math.Floor(ThresholdManager.GetReservationMaxPercent() * GetNbSeats());
        }

        private List<Seat> FindAvailableSeats(int seatsRequestedCount)
        {
            var availableSeats = new List<Seat>();
            var numberUnreservedSeats = 0;

            foreach (var seat in Seats)
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

        public bool CanReserve(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= GetAvailableSeatsForReservation();
        }

        public DomainEvent TryToBook(int seatsRequestedCount, string bookingReference)
        {
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
    }
}