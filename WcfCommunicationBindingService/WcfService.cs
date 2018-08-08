using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfCommunicationBindingService
{
    public class WcfService
    {
        public static o GetNetTcpDuplexService<o>(string Endpoint, object CallbackClient)
        {
            InstanceContext Client = new InstanceContext(CallbackClient);
            NetTcpBinding Binding = new NetTcpBinding(SecurityMode.None);

            DuplexChannelFactory<o> Service = new DuplexChannelFactory<o>(Client, Binding);
            return Service.CreateChannel(new EndpointAddress(Endpoint));
        }
    }
}
