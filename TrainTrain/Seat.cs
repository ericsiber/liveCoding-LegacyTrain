namespace TrainTrain
{
    public class Seat
    {
        private readonly string _bookingReference;
        private const string NoReservation = "";

        public string CoachName { get; }
        public int SeatNumber { get; }

        public Seat(string coachName, int seatNumber) 
            : this(coachName, seatNumber, string.Empty)
        {
        }

        public Seat(string coachName, int seatNumber, string bookingReference)
        {
            CoachName = coachName;
            SeatNumber = seatNumber;
            _bookingReference = bookingReference;
        }

        public bool IsNotReserved()
        {
            return _bookingReference == NoReservation;
        }
    }
}