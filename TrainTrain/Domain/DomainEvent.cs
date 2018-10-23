using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public interface DomainEvent
    {

    }

    public class SeatsReserved : DomainEvent
    {
        public string TrainId { get; }
        public IEnumerable<Seat> SeatIds { get; }
        public string BookingReference { get; }

        public SeatsReserved(string trainId, List<Seat> seatIds, string bookingReference)
        {
            TrainId = trainId;
            SeatIds = seatIds;
            BookingReference = bookingReference;
        }
    }

    public class SeatsReservedFailedBecauseNotEnoughAvailableSeats : DomainEvent
    {
        public string TrainId { get; }

        public SeatsReservedFailedBecauseNotEnoughAvailableSeats(string trainId)
        {
            TrainId = trainId;
        }
    }
}