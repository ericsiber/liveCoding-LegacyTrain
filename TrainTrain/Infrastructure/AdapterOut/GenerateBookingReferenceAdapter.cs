using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain.PortOut;

namespace TrainTrain.Infrastructure.AdapterOut
{
    public class GenerateBookingReferenceAdapter : GenerateBookingReference
    {
        private readonly IBookingReferenceService _bookingReferenceService;

        public GenerateBookingReferenceAdapter(IBookingReferenceService bookingReferenceService)
        {
            _bookingReferenceService = bookingReferenceService;
        }

        public Task<string> Execute()
        {
            return _bookingReferenceService.GetBookingReference();
        }
    }
}
