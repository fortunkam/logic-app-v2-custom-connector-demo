
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.Azure.Workflows.ServiceProviders.WebJobs.Abstractions.Providers;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoryleek.CustomConnector
{
    [ServiceOperationsProvider(Id = MyCustomConnector.ServiceId, Name = MyCustomConnector.ServiceName)]
    public class MyCustomConnector : IServiceOperationsTriggerProvider
    {
        public MyCustomConnector()
        {
            this.apiOperationsList = new InsensitiveDictionary<ServiceOperation>();
            this.apiOperationsList.AddRange(new InsensitiveDictionary<ServiceOperation>
            {
                { "doAction", DoAction }
            });

        }
        public string GetBindingConnectionInformation(string operationId, InsensitiveDictionary<JToken> connectionParameters)
        {
            return ServiceOperationsProviderUtilities
                    .GetRequiredParameterValue(
                        serviceId: ServiceId,
                        operationId: operationId,
                        parameterName: "connectionString",
                        parameters: connectionParameters)?
                    .ToValue<string>();
        }

        public string GetFunctionTriggerType()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ServiceOperation> GetOperations(bool expandManifest)
        {
            return this.apiOperationsList.Values;
        }

        /// <summary>
        /// The set of all API Operations.
        /// </summary>
        private readonly InsensitiveDictionary<ServiceOperation> apiOperationsList;

        /// <summary>
        /// The service name.
        /// </summary>
        public const string ServiceName = "myConnector";

        /// <summary>
        /// The service id.
        /// </summary>
        public const string ServiceId = "/serviceProviders/myConnector";

        public static ServiceOperationApi MyOperationApi = new ServiceOperationApi
        {
            Name = ServiceName,
            Id = "/serviceProviders/myConnector",
            Type = DesignerApiType.ServiceProvider,
            Properties = new ServiceOperationApiProperties
            {
                BrandColor = 0x00CC11,
                Capabilities = new[] { ApiCapability.Actions },
                ConnectionParameters = new ConnectionParameters
                {
                    ConnectionString = new ConnectionStringParameters()
                },
                Description = "My Custom connector",
                DisplayName = "My Custom Connector",
                Summary = "Its a connector",
                IconUri = new Uri("https://github.com/praveensri/LogicAppCustomConnector/blob/main/ServiceProviders.CosmosDb.Extensions/icon.png")

            }
        };

        public static readonly ServiceOperation DoAction = new ServiceOperation
        {
            Id = "/serviceProviders/doAction",
            Name = "doAction",
            Type = "doAction",
            Properties = new ServiceOperationProperties
            {
                Api = new ServiceOperationApi
                {
                    Properties = new ServiceOperationApiProperties
                    {
                        BrandColor = 0xC4D5FF,
                        Description = "Do a thing",
                        DisplayName = "Do Thing",
                        IconUri = new Uri("https://github.com/praveensri/LogicAppCustomConnector/blob/main/ServiceProviders.CosmosDb.Extensions/icon.png"),
                        Capabilities = new[] { ApiCapability.Actions }
                    }
                }.GetFlattenedApi(),
                Summary = "doAction",
                Description = "Do an Action",
                Visibility = Visibility.Important,
                OperationType = OperationType.ServiceProvider,
                BrandColor = 0x1C3A56,
                IconUri = new Uri("https://github.com/praveensri/LogicAppCustomConnector/blob/main/ServiceProviders.CosmosDb.Extensions/icon.png"),
                Trigger = TriggerType.NotSpecified
            }
        };

        public ServiceOperationApi GetService()
        {
            return MyOperationApi;

        }

        public async Task<ServiceOperationResponse> InvokeActionOperation(string operationId, InsensitiveDictionary<JToken> connectionParameters, ServiceOperationRequest serviceOperationRequest)
        {
            return await Task.Run(()=> new ServiceOperationResponse(JToken.Parse("{}")));
        }
    }
}
