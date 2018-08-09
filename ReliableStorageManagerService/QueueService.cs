using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Runtime;
using SharedService;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

//using OperationContext = System.ServiceModel.Web.MockedOperationContext;
using OperationContext = FabricWcfMockupService.Wcf.MockOperationContext<ReliableStorageManagerService.MockOperationContextWrapper>;

namespace ReliableStorageManagerService
{
    public class QueueService : IQueue, IService
    {
        private readonly StatefulServiceContext Context;
        private readonly IReliableStateManager StateManager;

        private const string QUEUE_NAME = "Queue";

        public QueueService(StatefulServiceContext Context, IReliableStateManager StateManager)
        {
            this.Context = Context;
            this.StateManager = StateManager;
        }

        public async Task DeQueueAsync()
        {
            var Callback = OperationContext.Current.GetCallbackChannel<IQueueClient>();
            var Queue = await StateManager.GetOrAddAsync<IReliableQueue<string>>(QUEUE_NAME);
            using (var tx = StateManager.CreateTransaction())
            {
                var Result = await Queue.TryDequeueAsync(tx);
                if (!Result.HasValue) await Callback.ResponseAsync("No Data in Queue");
                else
                {
                    var Message = Result.Value;
                    await Callback.ResponseAsync($"Dequeue: {Message}");
                }
                await tx.CommitAsync();
            }
        }

        public async Task EnQueueAsync(string Message)
        {
            var Operation = OperationContext.Current;
            IQueueClient Callback = null;
            if (Operation != null) Callback = Operation.GetCallbackChannel<IQueueClient>();
            var Queue = await StateManager.GetOrAddAsync<IReliableQueue<string>>(QUEUE_NAME);
            using (var tx = StateManager.CreateTransaction())
            {
                await Queue.EnqueueAsync(tx, Message);
                if (Callback != null) await Callback.ResponseAsync($"Enqueue: {Message}");
                await tx.CommitAsync();
            }
        }
    }
}
