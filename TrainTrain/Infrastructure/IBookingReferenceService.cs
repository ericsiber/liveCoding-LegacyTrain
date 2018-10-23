using System.Threading.Tasks;

namespace TrainTrain.Infrastructure
{
    public interface IBookingReferenceService
    {
        Task<string> GetBookingReference();
    }
}