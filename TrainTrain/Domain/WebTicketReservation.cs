using System.Linq;
using System.Threading.Tasks;
using TrainTrain.Domain.PortIn;
using TrainTrain.Domain.PortOut;

namespace TrainTrain.Domain
{
    public class WebTicketReservation : Reserve
    {
       
        private readonly SubmitReservation _submitReservation;
        private readonly GetTrainTopology _getTrainTopology;
        private readonly GenerateBookingReference _generateBookingReference;

        public WebTicketReservation(SubmitReservation submitReservation, GetTrainTopology getTrainTopology, GenerateBookingReference generateBookingReference)
        {
            _submitReservation = submitReservation;
            _getTrainTopology = getTrainTopology;
            _generateBookingReference = generateBookingReference;
        }

        public async Task<DomainEvent> Execute(string trainId, int seatsRequested)
        {
            var train = await _getTrainTopology.Execute(trainId);
            var bookingReference = await _generateBookingReference.Execute();

            var bookingEvent = train.TryToReserve(seatsRequested, bookingReference);

            if (bookingEvent is SeatsReserved seatsBooked)
            {
                await _submitReservation.Execute(seatsBooked.TrainId, seatsBooked.BookingReference, seatsBooked.SeatIds.ToList());
            }

            return bookingEvent;
        }
    }
}