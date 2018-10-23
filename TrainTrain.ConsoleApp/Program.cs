using System;
using TrainTrain.Infrastructure;
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
            var manager = new WebTicketReservation(new SubmitReservationAdapter(trainDataService), new GetTrainTopologyAdapter(trainDataService), new GenerateBookingReferenceAdapter(bookingReferenceService));

            var jsonResult = manager.Execute(train, seats);

            Console.WriteLine(jsonResult.Result);

            Console.WriteLine("Type <enter> to exit.");
            Console.ReadLine();
        }
    }
}