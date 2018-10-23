using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Domain.PortIn;
using TrainTrain.Domain.PortOut;
using TrainTrain.Infrastructure;
using TrainTrain.Infrastructure.AdapterOut;

namespace TrainTrain
{
    public class WebTicketReservation : Reserve
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;
        private readonly SubmitReservation _submitReservation;
        private readonly GetTrainTopology _getTrainTopology;

        public WebTicketReservation():this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
        {
        }

        public WebTicketReservation(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
            _submitReservation = new SubmitReservationAdapter(trainDataService);
            _getTrainTopology = new GetTrainTopologyAdapter(trainDataService);
        }

        public async Task<DomainEvent> Execute(string trainId, int seatsRequested)
        {
            var train = await _getTrainTopology.Execute(trainId);
            var bookingReference = await _bookingReferenceService.GetBookingReference();

            var bookingEvent = train.TryToReserve(seatsRequested, bookingReference);

            if (bookingEvent is SeatsReserved seatsBooked)
            {
                await _submitReservation.Execute(seatsBooked.TrainId, seatsBooked.BookingReference, seatsBooked.SeatIds.ToList());
            }

            return bookingEvent;
        }
    }
}