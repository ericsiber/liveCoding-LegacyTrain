namespace TrainTrain.Domain
{
    public class Seat
    {
        private readonly bool _isReserved;

        public string CoachName { get; }
        public int SeatNumber { get; }

        public Seat(string coachName, int seatNumber, bool isReserved)
        {
            _isReserved = isReserved;
            CoachName = coachName;
            SeatNumber = seatNumber;
        }

        public bool IsNotReserved()
        {
            return !_isReserved;
        }
    }
}