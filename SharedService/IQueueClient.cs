using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedService
{
    [ServiceContract]
    public interface IQueueClient
    {
        [OperationContract(IsOneWay = true)]
        Task ResponseAsync(string Message);
    }
}
