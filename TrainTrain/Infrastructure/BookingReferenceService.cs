using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TrainTrain.Infrastructure
{
    public class BookingReferenceService : IBookingReferenceService
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        
        public async Task<string> GetBookingReference()
        {
            string bookingRef;
            using (var client = new HttpClient())
            {
                bookingRef = await GetBookRef(client);
            }

            return bookingRef;
        }

        public async Task<string> GetBookRef(HttpClient client)
        {
            var value = new MediaTypeWithQualityHeaderValue("application/json");
            client.BaseAddress = new Uri(UriBookingReferenceService);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(value);

            // HTTP GET
            var response = await client.GetAsync("/booking_reference");
            response.EnsureSuccessStatusCode();

            var bookingRef = await response.Content.ReadAsStringAsync();
            return bookingRef;
        }
    }
}