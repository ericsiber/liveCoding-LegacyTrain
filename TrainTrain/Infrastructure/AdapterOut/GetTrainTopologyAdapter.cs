using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Domain.PortOut;

namespace TrainTrain.Infrastructure.AdapterOut
{
    public class GetTrainTopologyAdapter : GetTrainTopology
    {
        private readonly ITrainDataService _trainService;

        public GetTrainTopologyAdapter(ITrainDataService trainService)
        {
            _trainService = trainService;
        }

        public Task<Train> Execute(string id)
        {
            return _trainService.GetTrain(id);
        }
    }
}
