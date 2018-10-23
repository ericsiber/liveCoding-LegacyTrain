using System.Threading.Tasks;

namespace TrainTrain.Domain.PortIn
{
    public interface Reserve
    {
        Task<DomainEvent> Execute(string trainId, int seatsRequested);
    }
}