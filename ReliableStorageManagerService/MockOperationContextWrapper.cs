using FabricWcfMockupService.Wcf;
using SharedService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace ReliableStorageManagerService
{
    public class MockOperationContextWrapper : IOperationContext
    {
        public IContextChannel Channel => throw new NotImplementedException();

        public IExtensionCollection<OperationContext> Extensions => throw new NotImplementedException();

        public bool HasSupportingTokens => throw new NotImplementedException();

        public ServiceHostBase Host => throw new NotImplementedException();

        public MessageHeaders IncomingMessageHeaders => throw new NotImplementedException();

        public MessageProperties IncomingMessageProperties => throw new NotImplementedException();

        public MessageVersion IncomingMessageVersion => throw new NotImplementedException();

        public InstanceContext InstanceContext => throw new NotImplementedException();

        public bool IsUserContext => throw new NotImplementedException();

        public MessageHeaders OutgoingMessageHeaders => throw new NotImplementedException();

        public MessageProperties OutgoingMessageProperties => throw new NotImplementedException();

        public RequestContext RequestContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IServiceSecurityContext ServiceSecurityContext => throw new NotImplementedException();

        public string SessionId => throw new NotImplementedException();

        public ICollection<SupportingTokenSpecification> SupportingTokens => throw new NotImplementedException();

        public event EventHandler OperationCompleted;

        public T GetCallbackChannel<T>()
        {
            if (typeof(T) == typeof(IQueueClient))
            {
                IQueueClient x = new MockQueueClient();
                return (T)x;
            }
            return default(T);
        }

        public void SetTransactionComplete()
        {
            throw new NotImplementedException();
        }
    }

    public class MockQueueClient : IQueueClient
    {
        public Task ResponseAsync(string Message)
        {
            return Task.CompletedTask;
        }
    }
}
