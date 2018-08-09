using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using SharedService;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

//using OperationContext = System.ServiceModel.Web.MockedOperationContext;
using OperationContext = FabricWcfMockupService.Wcf.MockOperationContext<ReliableStorageManagerService.MockOperationContextWrapper>;

namespace ReliableStorageManagerService
{
    public class QueueService : IQueue
    {
        private readonly ServiceContext Context;
        private readonly IReliableStateManager StateManager;
        private readonly Func<IQueueClient> CreateCallback;

        private const string QUEUE_NAME = "Queue";

        public QueueService(ServiceContext Context, IReliableStateManager StateManager, Func<IQueueClient> CreateCallback = null)
        {
            this.Context = Context;
            this.StateManager = StateManager;
            this.CreateCallback = CreateCallback;
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
            if (Operation != null) Callback = CreateCallback == null ? Operation.GetCallbackChannel<IQueueClient>() : CreateCallback();
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
