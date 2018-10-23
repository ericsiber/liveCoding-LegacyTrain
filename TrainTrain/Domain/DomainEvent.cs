using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public interface DomainEvent
    {

    }

    public class SeatsReserved : DomainEvent
    {
        public TrainId TrainId { get; }
        public IEnumerable<Seat> SeatIds { get; }
        public string BookingReference { get; }

        public SeatsReserved(TrainId trainId, List<Seat> seatIds, string bookingReference)
        {
            TrainId = trainId;
            SeatIds = seatIds;
            BookingReference = bookingReference;
        }
    }

    public class SeatsReservedFailedBecauseNotEnoughAvailableSeats : DomainEvent
    {
        public TrainId TrainId { get; }

        public SeatsReservedFailedBecauseNotEnoughAvailableSeats(TrainId trainId)
        {
            TrainId = trainId;
        }
    }
}