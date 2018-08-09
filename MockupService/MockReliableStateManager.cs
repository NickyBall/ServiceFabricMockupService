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
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FabricWcfMockupService
{
    public class MockReliableStateManager : IReliableStateManagerReplica
    {
        private ConcurrentDictionary<Uri, IReliableState> store = new ConcurrentDictionary<Uri, IReliableState>();
        private Dictionary<Type, Type> dependencyMap = new Dictionary<Type, Type>()
        {
            [typeof(IReliableDictionary<,>)] = typeof(MockReliableDictionary<,>),
            [typeof(IReliableQueue<>)] = typeof(MockReliableQueue<>)
        };

        public Func<CancellationToken, Task<bool>> OnDataLossAsync { get; set; }

        public event EventHandler<NotifyTransactionChangedEventArgs> TransactionChanged;
        public event EventHandler<NotifyStateManagerChangedEventArgs> StateManagerChanged;

        public void Abort() { }

        public Task BackupAsync(Func<BackupInfo, CancellationToken, Task<bool>> backupCallback) => Task.CompletedTask;

        public Task BackupAsync(BackupOption option, TimeSpan timeout, CancellationToken cancellationToken, Func<BackupInfo, CancellationToken, Task<bool>> backupCallback) => Task.CompletedTask;

        public Task ChangeRoleAsync(ReplicaRole newRole, CancellationToken cancellationToken) => Task.CompletedTask;

        public Task CloseAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public ITransaction CreateTransaction() => new MockTransaction();

        public IAsyncEnumerator<IReliableState> GetAsyncEnumerator()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOrAddAsync<T>(ITransaction tx, Uri name, TimeSpan timeout) where T : IReliableState => GetOrAddAsync<T>(name);

        public Task<T> GetOrAddAsync<T>(ITransaction tx, Uri name) where T : IReliableState => GetOrAddAsync<T>(name);

        public Task<T> GetOrAddAsync<T>(Uri name, TimeSpan timeout) where T : IReliableState => GetOrAddAsync<T>(name);

        public Task<T> GetOrAddAsync<T>(Uri name) where T : IReliableState => Task.FromResult((T)store.GetOrAdd(name, GetDependency(typeof(T))));

        public Task<T> GetOrAddAsync<T>(ITransaction tx, string name, TimeSpan timeout) where T : IReliableState => GetOrAddAsync<T>(ToUri(name));

        public Task<T> GetOrAddAsync<T>(ITransaction tx, string name) where T : IReliableState => GetOrAddAsync<T>(ToUri(name));

        public Task<T> GetOrAddAsync<T>(string name, TimeSpan timeout) where T : IReliableState => GetOrAddAsync<T>(ToUri(name));

        public Task<T> GetOrAddAsync<T>(string name) where T : IReliableState => GetOrAddAsync<T>(ToUri(name));

        public void Initialize(StatefulServiceInitializationParameters initializationParameters)
        {
            throw new NotImplementedException();
        }

        public Task<IReplicator> OpenAsync(ReplicaOpenMode openMode, IStatefulServicePartition partition, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ITransaction tx, Uri name, TimeSpan timeout) => RemoveAsync(name);

        public Task RemoveAsync(ITransaction tx, Uri name) => RemoveAsync(name);

        public Task RemoveAsync(Uri name, TimeSpan timeout) => RemoveAsync(name);

        public Task RemoveAsync(Uri name) => Task.Factory.StartNew(() => store.TryRemove(name, out IReliableState result));

        public Task RemoveAsync(ITransaction tx, string name, TimeSpan timeout) => RemoveAsync(ToUri(name));

        public Task RemoveAsync(ITransaction tx, string name) => RemoveAsync(ToUri(name));

        public Task RemoveAsync(string name, TimeSpan timeout) => RemoveAsync(ToUri(name));

        public Task RemoveAsync(string name) => RemoveAsync(ToUri(name));

        public Task RestoreAsync(string backupFolderPath) => Task.CompletedTask;

        public Task RestoreAsync(string backupFolderPath, RestorePolicy restorePolicy, CancellationToken cancellationToken) => Task.CompletedTask;

        public bool TryAddStateSerializer<T>(IStateSerializer<T> stateSerializer) => true;

        public Task<ConditionalValue<T>> TryGetAsync<T>(Uri name) where T : IReliableState
        {
            bool result = store.TryGetValue(name, out IReliableState item);
            return Task.FromResult(new ConditionalValue<T>(result, (T)item));
        }

        public Task<ConditionalValue<T>> TryGetAsync<T>(string name) where T : IReliableState => TryGetAsync<T>(ToUri(name));

        private IReliableState GetDependency(Type t)
        {
            Type mockType = dependencyMap[t.GetGenericTypeDefinition()];
            return (IReliableState)Activator.CreateInstance(mockType.MakeGenericType(t.GetGenericArguments()));
        }

        private Uri ToUri(string name)
        {
            return new Uri("mock://" + name, UriKind.Absolute);
        }
    }
}
