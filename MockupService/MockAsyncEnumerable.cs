using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MockupService
{
    internal class MockAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<T> enumerable;

        public MockAsyncEnumerable(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator() => new MockAsyncEnumerator<T>(enumerable.GetEnumerator());
    }

    internal class MockAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        public T Current => enumerator.Current;

        public MockAsyncEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public void Dispose() { }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(enumerator.MoveNext());

        public void Reset() => enumerator.Reset();
    }
}
