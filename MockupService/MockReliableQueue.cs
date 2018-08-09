/* Copyright (c) 2018 Jakkrit Junrat
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FabricWcfMockupService
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
