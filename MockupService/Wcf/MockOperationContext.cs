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
    public class MockOperationContext<T> : IDisposable where T : IOperationContext, new()
    {
        public MockOperationContext() { }

        public static IOperationContext Current
        {
            get
            {
                if (OperationContext.Current != null) return new OperationContextWrapper(OperationContext.Current);
                return new T();
            }
        }

        public void Dispose() { }
    }
}
