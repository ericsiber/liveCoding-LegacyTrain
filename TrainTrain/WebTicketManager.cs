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
            List<Seat> availableSeats = new List<Seat>();
            var availableSeatsCount = 0;

            // get the train
            var trainInst = await _trainDataService.GetTrain(trainId);

            var nbSeatsReservedAfterReservation = trainInst.ReservedSeats + seatsRequestedCount;
            if (nbSeatsReservedAfterReservation <= Math.Floor(ThresholdManager.GetReservationMaxPercent() * trainInst.GetNbSeats()))
            {
                var numberOfReservation = 0;
                var i = 0;
                // find seats to reserve
                foreach (var seat in trainInst.Seats)
                {
                    if (seat.IsNotReserved())
                    {
                        i++;
                        if (i <= seatsRequestedCount)
                        {
                            availableSeats.Add(seat);
                        }
                    }
                }

                foreach (var a in availableSeats)
                {
                    availableSeatsCount++;
                }

                var reservedSets = 0;


                string bookingRef;
                if (availableSeatsCount != seatsRequestedCount)
                {
                    return string.Format("{{\"train_id\": \"{0}\", \"booking_reference\": \"\", \"seats\": []}}",
                        trainId);
                }
                else
                {
                    bookingRef = await _bookingReferenceService.GetBookingReference();

                    foreach (var availableSeat in availableSeats)
                    {
                        availableSeat.BookingRef = bookingRef;
                        numberOfReservation++;
                        reservedSets++;
                    }
                }

                if (numberOfReservation == seatsRequestedCount)
                {
                    await _trainCaching.Save(trainId, trainInst, bookingRef);

                    await _trainDataService.Reserve(trainId, bookingRef, availableSeats);

                    var todod = "[TODOD]";


                        return string.Format(
                            "{{\"train_id\": \"{0}\", \"booking_reference\": \"{1}\", \"seats\": {2}}}", 
                            trainId,
                            bookingRef, 
                            dumpSeats(availableSeats));
                    
                }
            }

            return string.Format("{{\"train_id\": \"{0}\", \"booking_reference\": \"\", \"seats\": []}}", trainId);
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