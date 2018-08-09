using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FabricWcfMockupService;
using SharedService;

namespace ReliableStorageManagerService.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MockReliableStateManager MockStateManager = new MockReliableStateManager();
            QueueService Service = new QueueService(MockServiceContext.CreateStatefulServiceContext(), MockStateManager);
            Service.EnQueueAsync("a").GetAwaiter().GetResult();

            var x = MockStateManager.GetOrAddAsync<IReliableQueue<string>>("Queue").GetAwaiter().GetResult();
            var tx = MockStateManager.CreateTransaction();
            var c = x.GetCountAsync(tx).GetAwaiter().GetResult();

            Assert.IsTrue(c > 0);
        }
    }

    public class QueueClientTest : IQueueClient
    {
        public Task ResponseAsync(string Message)
        {
            return Task.CompletedTask;
        }
    }
}
