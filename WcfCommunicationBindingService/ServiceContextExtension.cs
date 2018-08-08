using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfCommunicationBindingService
{
    public static class ServiceContextExtension
    {
        public static WcfCommunicationListener<o> CreateWcfNetTcpListener<o>(this ServiceContext Context, o Service, string UrlSuffix, string ServiceEndpointName = "ServiceEndpoint")
        {
            string host = Context.NodeContext.IPAddressOrFQDN;
            var endpointconfig = Context.CodePackageActivationContext.GetEndpoint(ServiceEndpointName);
            int port = endpointconfig.Port;
            //var scheme = endpointconfig.Protocol.ToString();
            string scheme = "net.tcp";
            string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/{3}", scheme, host, port, UrlSuffix);

            NetTcpBinding Binding = new NetTcpBinding(SecurityMode.None);
            return new WcfCommunicationListener<o>(
                Context,
                Service,
                Binding,
                new EndpointAddress(uri)
                );
        }
    }
}
