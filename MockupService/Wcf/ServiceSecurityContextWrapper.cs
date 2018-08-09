using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MockupService.Wcf
{
    public class ServiceSecurityContextWrapper : IServiceSecurityContext
    {
        ServiceSecurityContext context;

        public ServiceSecurityContextWrapper(ServiceSecurityContext context)
        {
            this.context = context;
        }

        public AuthorizationContext AuthorizationContext => context.AuthorizationContext;

        public ReadOnlyCollection<IAuthorizationPolicy> AuthorizationPolicies => context.AuthorizationPolicies;

        public bool IsAnonymous => context.IsAnonymous;

        public IIdentity PrimaryIdentity => context.PrimaryIdentity;

        public WindowsIdentity WindowsIdentity => context.WindowsIdentity;
    }
}
