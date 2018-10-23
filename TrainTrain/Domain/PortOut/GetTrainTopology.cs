using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain.Domain.PortOut
{
    public interface GetTrainTopology
    {
        Task<Train> Execute(TrainId id);
    }

   
}
