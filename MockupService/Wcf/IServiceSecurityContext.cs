using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MockupService.Wcf
{
    public interface IServiceSecurityContext
    {
        AuthorizationContext AuthorizationContext { get; }
        ReadOnlyCollection<IAuthorizationPolicy> AuthorizationPolicies { get; }
        bool IsAnonymous { get; }
        IIdentity PrimaryIdentity { get; }
        WindowsIdentity WindowsIdentity { get; }
    }
}
