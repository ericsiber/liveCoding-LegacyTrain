using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class Train
    {
        public Train(List<Seat> seats)
        {
            this.Seats = seats;
        }

        private int GetNbSeats()
        {
            return this.Seats.Count;
        }

        private int ReservedSeats
        {
            get { return Seats.Count(s => !String.IsNullOrEmpty(s.BookingRef)); }
        }
        public List<Seat> Seats { get; set; }

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
    }

    public class TrainJsonPoco
    {
        public List<SeatJsonPoco> seats { get; set;  }

        public TrainJsonPoco()
        {
            this.seats = new List<SeatJsonPoco>();
        }
    }

    public class SeatJsonPoco
    {
        public string booking_reference { get; set; }
        public string seat_number { get; set; }
        public string coach { get; set; }
    }
}