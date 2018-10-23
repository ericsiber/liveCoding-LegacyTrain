using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class Train
    {
        public Train(List<Seat> seats)
        {
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
        public List<Seat> Seats { get; private set; }

        private int GetAvailableSeatsForReservation()
        {
            return (int)Math.Floor(ThresholdManager.GetReservationMaxPercent() * GetNbSeats());
        }

        public List<Seat> FindAvailableSeats(int seatsRequestedCount)
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

        public DomainEvent TryToBook(int seatsRequestedCount, string bookingReference, string trainId)
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
                return new SeatsBooked(trainId, availableSeats,bookingReference);
            }
            return new SeatsBookedFailedBecauseNotEnoughAvailableSeats(trainId);
        }
    }

    public class SeatJsonPoco
    {
        public string booking_reference { get; set; }
        public string seat_number { get; set; }
        public string coach { get; set; }
    }
}