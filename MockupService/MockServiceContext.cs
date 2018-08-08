using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockupService
{
    public class MockServiceContext
    {
        public static StatefulServiceContext CreateStatefulServiceContext()
            => new StatefulServiceContext(
                new NodeContext(string.Empty, new NodeId(0, 0), 0, string.Empty, string.Empty),
                new MockCodePackageActivationContext(),
                string.Empty,
                new Uri("fabric:/Mock"),
                null,
                Guid.NewGuid(),
                0L
                );
    }
}
