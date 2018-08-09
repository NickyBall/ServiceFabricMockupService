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
using Microsoft.ServiceFabric.Data.Notifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FabricWcfMockupService
{
    public class MockReliableDictionary<TKey, TValue> : IReliableDictionary<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private ConcurrentDictionary<TKey, TValue> _Dictionary;
        public ConcurrentDictionary<TKey, TValue> Dictionary
        {
            get
            {
                if (_Dictionary == null) _Dictionary = new ConcurrentDictionary<TKey, TValue>();
                return _Dictionary;
            }
            set => _Dictionary = value;
        }

        public Func<IReliableDictionary<TKey, TValue>, NotifyDictionaryRebuildEventArgs<TKey, TValue>, Task> RebuildNotificationAsyncCallback { get; set; }

        public Uri Name { get; set; }

        public event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> DictionaryChanged;

        public Task AddAsync(ITransaction tx, TKey key, TValue value)
        {
            if (!Dictionary.TryAdd(key, value)) throw new InvalidOperationException("key already exists: " + key.ToString());
            return Task.CompletedTask;
        }

        public Task AddAsync(ITransaction tx, TKey key, TValue value, TimeSpan timeout, CancellationToken cancellationToken) => AddAsync(tx, key, value);

        public Task<TValue> AddOrUpdateAsync(ITransaction tx, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
            => Task.FromResult(Dictionary.AddOrUpdate(key, addValueFactory, updateValueFactory));

        public Task<TValue> AddOrUpdateAsync(ITransaction tx, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
            => Task.FromResult(Dictionary.AddOrUpdate(key, addValue, updateValueFactory));

        public Task<TValue> AddOrUpdateAsync(ITransaction tx, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory, TimeSpan timeout, CancellationToken cancellationToken)
            => AddOrUpdateAsync(tx, key, addValueFactory, updateValueFactory);

        public Task<TValue> AddOrUpdateAsync(ITransaction tx, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory, TimeSpan timeout, CancellationToken cancellationToken)
            => AddOrUpdateAsync(tx, key, addValue, updateValueFactory);

        public Task ClearAsync(TimeSpan timeout, CancellationToken cancellationToken) => ClearAsync();

        public Task ClearAsync() => Task.Factory.StartNew(() => Dictionary.Clear());

        public Task<bool> ContainsKeyAsync(ITransaction tx, TKey key) => Task.FromResult(Dictionary.ContainsKey(key));

        public Task<bool> ContainsKeyAsync(ITransaction tx, TKey key, LockMode lockMode) => ContainsKeyAsync(tx, key);

        public Task<bool> ContainsKeyAsync(ITransaction tx, TKey key, TimeSpan timeout, CancellationToken cancellationToken)
            => ContainsKeyAsync(tx, key);

        public Task<bool> ContainsKeyAsync(ITransaction tx, TKey key, LockMode lockMode, TimeSpan timeout, CancellationToken cancellationToken)
            => ContainsKeyAsync(tx, key);

        public Task<IAsyncEnumerable<KeyValuePair<TKey, TValue>>> CreateEnumerableAsync(ITransaction txn)
            => Task.FromResult((IAsyncEnumerable<KeyValuePair<TKey, TValue>>)new MockAsyncEnumerable<KeyValuePair<TKey, TValue>>(Dictionary));

        public Task<IAsyncEnumerable<KeyValuePair<TKey, TValue>>> CreateEnumerableAsync(ITransaction txn, EnumerationMode enumerationMode)
            => CreateEnumerableAsync(txn);

        public Task<IAsyncEnumerable<KeyValuePair<TKey, TValue>>> CreateEnumerableAsync(ITransaction txn, Func<TKey, bool> filter, EnumerationMode enumerationMode)
            => CreateEnumerableAsync(txn);

        public Task<long> GetCountAsync(ITransaction tx) => Task.FromResult<long>(Dictionary.Count);

        public Task<TValue> GetOrAddAsync(ITransaction tx, TKey key, Func<TKey, TValue> valueFactory)
            => Task.FromResult(Dictionary.GetOrAdd(key, valueFactory));

        public Task<TValue> GetOrAddAsync(ITransaction tx, TKey key, TValue value)
            => Task.FromResult(Dictionary.GetOrAdd(key, value));

        public Task<TValue> GetOrAddAsync(ITransaction tx, TKey key, Func<TKey, TValue> valueFactory, TimeSpan timeout, CancellationToken cancellationToken)
            => GetOrAddAsync(tx, key, valueFactory);

        public Task<TValue> GetOrAddAsync(ITransaction tx, TKey key, TValue value, TimeSpan timeout, CancellationToken cancellationToken)
            => GetOrAddAsync(tx, key, value);

        public Task SetAsync(ITransaction tx, TKey key, TValue value)
            => Task.Factory.StartNew(() => Dictionary[key] = value);

        public Task SetAsync(ITransaction tx, TKey key, TValue value, TimeSpan timeout, CancellationToken cancellationToken)
            => SetAsync(tx, key, value);

        public Task<bool> TryAddAsync(ITransaction tx, TKey key, TValue value)
            => Task.FromResult(Dictionary.TryAdd(key, value));

        public Task<bool> TryAddAsync(ITransaction tx, TKey key, TValue value, TimeSpan timeout, CancellationToken cancellationToken)
            => TryAddAsync(tx, key, value);

        public Task<ConditionalValue<TValue>> TryGetValueAsync(ITransaction tx, TKey key)
        {
            bool HasValue = Dictionary.TryGetValue(key, out TValue Value);
            return Task.FromResult(new ConditionalValue<TValue>(HasValue, Value));
        }

        public Task<ConditionalValue<TValue>> TryGetValueAsync(ITransaction tx, TKey key, LockMode lockMode)
            => TryGetValueAsync(tx, key);

        public Task<ConditionalValue<TValue>> TryGetValueAsync(ITransaction tx, TKey key, TimeSpan timeout, CancellationToken cancellationToken)
            => TryGetValueAsync(tx, key);

        public Task<ConditionalValue<TValue>> TryGetValueAsync(ITransaction tx, TKey key, LockMode lockMode, TimeSpan timeout, CancellationToken cancellationToken)
            => TryGetValueAsync(tx, key);

        public Task<ConditionalValue<TValue>> TryRemoveAsync(ITransaction tx, TKey key)
        {
            bool HasValue = Dictionary.TryRemove(key, out TValue Value);
            return Task.FromResult(new ConditionalValue<TValue>(HasValue, Value));
        }

        public Task<ConditionalValue<TValue>> TryRemoveAsync(ITransaction tx, TKey key, TimeSpan timeout, CancellationToken cancellationToken)
            => TryRemoveAsync(tx, key);

        public Task<bool> TryUpdateAsync(ITransaction tx, TKey key, TValue newValue, TValue comparisonValue)
            => Task.FromResult(Dictionary.TryUpdate(key, newValue, comparisonValue));

        public Task<bool> TryUpdateAsync(ITransaction tx, TKey key, TValue newValue, TValue comparisonValue, TimeSpan timeout, CancellationToken cancellationToken)
            => TryUpdateAsync(tx, key, newValue, comparisonValue);
    }
}
