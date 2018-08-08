using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockupService
{
    public class MockTransaction : ITransaction
    {
        public long CommitSequenceNumber => 0L;

        public long TransactionId => 0L;

        public void Abort() { }

        public Task CommitAsync() => Task.CompletedTask;

        public void Dispose() { }

        public Task<long> GetVisibilitySequenceNumberAsync() => Task.FromResult(0L);
    }
}
