using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace MockupService.Wcf
{
    public class OperationContextWrapper : IOperationContext
    {
        OperationContext context;

        public OperationContextWrapper(OperationContext context)
        {
            this.context = context;
        }

        public IContextChannel Channel => context.Channel;

        public IExtensionCollection<OperationContext> Extensions => context.Extensions;

        public bool HasSupportingTokens => context.HasSupportingTokens;

        public ServiceHostBase Host => context.Host;

        public MessageHeaders IncomingMessageHeaders => context.IncomingMessageHeaders;

        public MessageProperties IncomingMessageProperties => context.IncomingMessageProperties;

        public MessageVersion IncomingMessageVersion => context.IncomingMessageVersion;

        public InstanceContext InstanceContext => context.InstanceContext;

        public bool IsUserContext => context.IsUserContext;

        public MessageHeaders OutgoingMessageHeaders => context.OutgoingMessageHeaders;

        public MessageProperties OutgoingMessageProperties => context.OutgoingMessageProperties;

        public RequestContext RequestContext { get => context.RequestContext; set => context.RequestContext = value; }

        public IServiceSecurityContext ServiceSecurityContext => new ServiceSecurityContextWrapper(context.ServiceSecurityContext);

        public string SessionId => context.SessionId;

        public ICollection<SupportingTokenSpecification> SupportingTokens => context.SupportingTokens;

        public event EventHandler OperationCompleted;

        public T GetCallbackChannel<T>() => context.GetCallbackChannel<T>();

        void IOperationContext.SetTransactionComplete() => context.SetTransactionComplete();
    }
}
