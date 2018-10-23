using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Domain.PortIn;

namespace TrainTrain.Infrastructure.AdapterIn
{
    public class ReservationAdapter
    {
        private readonly ITrainDataService _trainDataService;
        private readonly Reservation _reservation;

        public ReservationAdapter(ITrainDataService trainDataService, Reservation reservation)
        {
            _trainDataService = trainDataService;
            _reservation = reservation;
        }

        public async Task<string> ReserveLegacy(string trainId, int seatsRequested)
        {
            var bookingEvent = await _reservation.Reserve(trainId, seatsRequested);
            return await HandleBookingEvents(bookingEvent);
        }

        private async Task<string> HandleBookingEvents(DomainEvent bookingEvent)
        {
            if (bookingEvent is SeatsReservedFailedBecauseNotEnoughAvailableSeats seatsBookedFailed)
            {
                return InvalidReservation(seatsBookedFailed.TrainId);
            }

            var seatsBooked = (SeatsReserved)bookingEvent;
            await _trainDataService.SubmitReservation(seatsBooked.TrainId, seatsBooked.BookingReference, seatsBooked.SeatIds.ToList());
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
