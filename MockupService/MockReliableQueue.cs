using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MockupService
{
    public class MockReliableQueue<T> : IReliableQueue<T>
    {
        private ConcurrentQueue<T> _Queue;
        public ConcurrentQueue<T> Queue
        {
            get
            {
                if (_Queue == null) _Queue = new ConcurrentQueue<T>();
                return _Queue;
            }
            set => _Queue = value;

        }
        public Uri Name => throw new NotImplementedException();

        public Task ClearAsync() => Task.Factory.StartNew(() =>
        {
            while (!Queue.IsEmpty)
            {
                Queue.TryDequeue(out T result);
            }
        });

        public Task<IAsyncEnumerable<T>> CreateEnumerableAsync(ITransaction tx) => Task.FromResult((IAsyncEnumerable<T>) new MockAsyncEnumerable<T>(Queue));

        public Task EnqueueAsync(ITransaction tx, T item) => Task.Factory.StartNew(() => Queue.Enqueue(item));

        public Task EnqueueAsync(ITransaction tx, T item, TimeSpan timeout, CancellationToken cancellationToken) => EnqueueAsync(tx, item);

        public Task<long> GetCountAsync(ITransaction tx) => Task.FromResult<long>(Queue.Count);

        public Task<ConditionalValue<T>> TryDequeueAsync(ITransaction tx)
        {
            bool HasValue = Queue.TryDequeue(out T Value);
            return Task.FromResult(new ConditionalValue<T>(HasValue, Value));
        }

        public Task<ConditionalValue<T>> TryDequeueAsync(ITransaction tx, TimeSpan timeout, CancellationToken cancellationToken) => TryDequeueAsync(tx);

        public Task<ConditionalValue<T>> TryPeekAsync(ITransaction tx)
        {
            bool result = Queue.TryPeek(out T item);
            return Task.FromResult(new ConditionalValue<T>(result, item));
        }

        public Task<ConditionalValue<T>> TryPeekAsync(ITransaction tx, TimeSpan timeout, CancellationToken cancellationToken) => TryPeekAsync(tx);

        public Task<ConditionalValue<T>> TryPeekAsync(ITransaction tx, LockMode lockMode) => TryPeekAsync(tx);

        public Task<ConditionalValue<T>> TryPeekAsync(ITransaction tx, LockMode lockMode, TimeSpan timeout, CancellationToken cancellationToken) => TryPeekAsync(tx);
    }
}
