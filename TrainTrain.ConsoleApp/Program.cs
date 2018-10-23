using System;
using TrainTrain.Domain;
using TrainTrain.Infrastructure;
using TrainTrain.Infrastructure.AdapterIn;
using TrainTrain.Infrastructure.AdapterOut;

namespace TrainTrain.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var train = args[0];
            var seats = int.Parse(args[1]);

            var trainDataService = new TrainDataService();
            var bookingReferenceService = new BookingReferenceService();
            var webTicketReservation = new WebTicketReservation(new SubmitReservationAdapter(trainDataService), new GetTrainTopologyAdapter(trainDataService), new GenerateBookingReferenceAdapter(bookingReferenceService));

            var adapter = new ReserveAdapter(webTicketReservation);

            var jsonResult = adapter.Execute(train, seats);

            Console.WriteLine(jsonResult.Result);

            Console.WriteLine("Type <enter> to exit.");
            Console.ReadLine();
        }
    }
}