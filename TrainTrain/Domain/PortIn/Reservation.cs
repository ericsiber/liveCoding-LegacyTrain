using System.Threading.Tasks;

namespace TrainTrain.Domain.PortIn
{
    public interface Reservation
    {
        Task<DomainEvent> Reserve(string trainId, int seatsRequested);
    }
}