namespace TrainTrain
{
    public class Seat
    {
        private const string NoReservation = "";
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; set;  }

        public Seat(string coachName, int seatNumber) : this(coachName, seatNumber, string.Empty)
        {
        }

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            CoachName = coachName;
            SeatNumber = seatNumber;
            BookingRef = bookingRef;
        }

        public bool IsNotReserved()
        {
            return BookingRef == NoReservation;
        }
    }
}