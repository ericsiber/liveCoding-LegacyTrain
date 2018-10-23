using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Domain.PortIn;

namespace TrainTrain.Infrastructure.AdapterIn
{
    public class ReserveAdapter
    {
        private readonly Reserve _reserve;

        public ReserveAdapter(Reserve reserve)
        {
            _reserve = reserve;
        }

        public async Task<string> Execute(string trainId, int seatsRequested)
        {
            var bookingEvent = await _reserve.Execute(trainId, seatsRequested);
            return HandleBookingEvents(bookingEvent);
        }

        private string HandleBookingEvents(DomainEvent bookingEvent)
        {
            if (bookingEvent is SeatsReservedFailedBecauseNotEnoughAvailableSeats seatsBookedFailed)
            {
                return InvalidReservation(seatsBookedFailed.TrainId);
            }

            var seatsBooked = (SeatsReserved)bookingEvent;
            return ValidReservation(seatsBooked.TrainId, seatsBooked.BookingReference, seatsBooked.SeatIds.ToList());
        }


        private string ValidReservation(string trainId, string bookingReference, List<Seat> availableSeats)
        {
            return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bookingReference}\"," +
                   $" \"seats\": {dumpSeats(availableSeats)}}}";
        }

        private static string InvalidReservation(string trainId)
        {
            return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
        }

        private string dumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                {
                    sb.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                sb.Append(string.Format("\"{0}{1}\"", seat.SeatNumber, seat.CoachName));
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
