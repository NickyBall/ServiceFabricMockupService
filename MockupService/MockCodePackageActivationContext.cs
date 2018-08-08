using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Fabric;
using System.Fabric.Description;
using System.Fabric.Health;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockupService
{
    public class MockCodePackageActivationContext : ICodePackageActivationContext
    {
        public string WorkDirectory => throw new NotImplementedException();

        public string LogDirectory => throw new NotImplementedException();

        public string TempDirectory => throw new NotImplementedException();

        public string ContextId => throw new NotImplementedException();

        public string CodePackageName => throw new NotImplementedException();

        public string CodePackageVersion => throw new NotImplementedException();

        public string ApplicationName => throw new NotImplementedException();

        public string ApplicationTypeName => throw new NotImplementedException();

        public event EventHandler<PackageAddedEventArgs<CodePackage>> CodePackageAddedEvent;
        public event EventHandler<PackageRemovedEventArgs<CodePackage>> CodePackageRemovedEvent;
        public event EventHandler<PackageModifiedEventArgs<CodePackage>> CodePackageModifiedEvent;
        public event EventHandler<PackageAddedEventArgs<ConfigurationPackage>> ConfigurationPackageAddedEvent;
        public event EventHandler<PackageRemovedEventArgs<ConfigurationPackage>> ConfigurationPackageRemovedEvent;
        public event EventHandler<PackageModifiedEventArgs<ConfigurationPackage>> ConfigurationPackageModifiedEvent;
        public event EventHandler<PackageAddedEventArgs<DataPackage>> DataPackageAddedEvent;
        public event EventHandler<PackageRemovedEventArgs<DataPackage>> DataPackageRemovedEvent;
        public event EventHandler<PackageModifiedEventArgs<DataPackage>> DataPackageModifiedEvent;

        public void Dispose() { }

        public ApplicationPrincipalsDescription GetApplicationPrincipals()
        {
            throw new NotImplementedException();
        }

        public IList<string> GetCodePackageNames()
        {
            throw new NotImplementedException();
        }

        public CodePackage GetCodePackageObject(string packageName)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetConfigurationPackageNames()
        {
            throw new NotImplementedException();
        }

        public ConfigurationPackage GetConfigurationPackageObject(string packageName)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetDataPackageNames()
        {
            throw new NotImplementedException();
        }

        public DataPackage GetDataPackageObject(string packageName)
        {
            throw new NotImplementedException();
        }

        public EndpointResourceDescription GetEndpoint(string endpointName)
        {
            throw new NotImplementedException();
        }

        public KeyedCollection<string, EndpointResourceDescription> GetEndpoints()
        {
            throw new NotImplementedException();
        }

        public KeyedCollection<string, ServiceGroupTypeDescription> GetServiceGroupTypes()
        {
            throw new NotImplementedException();
        }

        public string GetServiceManifestName()
        {
            throw new NotImplementedException();
        }

        public string GetServiceManifestVersion()
        {
            throw new NotImplementedException();
        }

        public KeyedCollection<string, ServiceTypeDescription> GetServiceTypes()
        {
            throw new NotImplementedException();
        }

        public void ReportApplicationHealth(HealthInformation healthInfo)
        {
            throw new NotImplementedException();
        }

        public void ReportApplicationHealth(HealthInformation healthInfo, HealthReportSendOptions sendOptions)
        {
            throw new NotImplementedException();
        }

        public void ReportDeployedApplicationHealth(HealthInformation healthInfo)
        {
            throw new NotImplementedException();
        }

        public void ReportDeployedApplicationHealth(HealthInformation healthInfo, HealthReportSendOptions sendOptions)
        {
            throw new NotImplementedException();
        }

        public void ReportDeployedServicePackageHealth(HealthInformation healthInfo)
        {
            throw new NotImplementedException();
        }

        public void ReportDeployedServicePackageHealth(HealthInformation healthInfo, HealthReportSendOptions sendOptions)
        {
            throw new NotImplementedException();
        }
    }
}
