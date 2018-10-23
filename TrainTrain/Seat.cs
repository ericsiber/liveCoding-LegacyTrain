namespace TrainTrain
{
    public class Seat
    {
        private readonly bool _isBooked;

        public string CoachName { get; }
        public int SeatNumber { get; }

        public Seat(string coachName, int seatNumber, bool isBooked)
        {
            _isBooked = isBooked;
            CoachName = coachName;
            SeatNumber = seatNumber;
        }

        public bool IsNotBooked()
        {
            return !_isBooked;
        }
    }
}