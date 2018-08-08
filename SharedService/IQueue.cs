using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedService
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IQueueClient))]
    public interface IQueue
    {
        [OperationContract(IsOneWay = true)]
        Task EnQueueAsync(string Message);
        [OperationContract(IsOneWay = true)]
        Task DeQueueAsync();
    }
}
