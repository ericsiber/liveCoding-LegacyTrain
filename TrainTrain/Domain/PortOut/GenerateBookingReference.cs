using System.Threading.Tasks;

namespace TrainTrain.Domain.PortOut
{
    public interface GenerateBookingReference
    {
        Task<string> Execute();
    }
}