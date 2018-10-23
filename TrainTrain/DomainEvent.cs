using System.Collections;
using System.Collections.Generic;

namespace TrainTrain
{
    public interface DomainEvent
    {

    }

    public class SeatsBooked : DomainEvent
    {
        public string TrainId { get; }
        public IEnumerable<Seat> SeatIds { get; }
        public string BookingReference { get; }

        public SeatsBooked(string trainId, List<Seat> seatIds, string bookingReference)
        {
            TrainId = trainId;
            SeatIds = seatIds;
            BookingReference = bookingReference;
        }
    }

    public class SeatsBookedFailedBecauseNotEnoughAvailableSeats : DomainEvent
    {
        public string TrainId { get; }

        public SeatsBookedFailedBecauseNotEnoughAvailableSeats(string trainId)
        {
            TrainId = trainId;
        }
    }
}