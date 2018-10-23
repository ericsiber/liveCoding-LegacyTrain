using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainCaching _trainCaching;
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public WebTicketManager():this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
        {
        }

        public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
            _trainCaching = new TrainCaching();
            _trainCaching.Clear();
        }

        public WebTicketManager(ITrainCaching trainCaching, ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainCaching = trainCaching;
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }

        public async Task<string> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await _trainDataService.GetTrain(trainId);

            var nbSeatsReservedAfterReservation = train.ReservedSeats + seatsRequestedCount;
            if (nbSeatsReservedAfterReservation > train.GetAvailableSeatsForReservation())
            {
                return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
            }

            var numberOfReservation = 0;
            var availableSeats = train.FindAvailableSeats(seatsRequestedCount);


            if (availableSeats.Count != seatsRequestedCount)
            {
                return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
            }

            var bookingReference = await _bookingReferenceService.GetBookingReference();

            foreach (var availableSeat in availableSeats)
            {
                availableSeat.BookingRef = bookingReference;
                numberOfReservation++;
            }

            if (numberOfReservation != seatsRequestedCount)
            {
                return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
            }

            await _trainCaching.Save(trainId, train, bookingReference);

            await _trainDataService.Reserve(trainId, bookingReference, availableSeats);

            return
                $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bookingReference}\", \"seats\": {dumpSeats(availableSeats)}}}";

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