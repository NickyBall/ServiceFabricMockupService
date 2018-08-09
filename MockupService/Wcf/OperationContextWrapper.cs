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

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace FabricWcfMockupService.Wcf
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
