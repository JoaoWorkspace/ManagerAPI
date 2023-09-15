using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using System.Security.Cryptography.X509Certificates;
using DatabaseSettings = ManagerAPI.Persistence.Settings.DatabaseSettings;

namespace ManagerAPI.Persistence.Database
{
    // The `IdentityDocumentStoreHolder` class holds a single Document Store instance.
    public class IdentityDocumentStoreHolder
    {

        // Use Lazy<IDocumentStore> to initialize the document store lazily. 
        // This ensures that it is created only once - when first accessing the public `Store` property.
        //private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore());

        public static IDocumentStore Store;

        public IdentityDocumentStoreHolder(IOptions<DatabaseSettings> databaseSettings)
        {
            Store = CreateStore(databaseSettings.Value);
        }

        // Initialize the Document Store
        private IDocumentStore CreateStore(DatabaseSettings? settings = null, Action<IDocumentStore>? customInit = null)
        {
            IDocumentStore store = new DocumentStore()
            {
                // Define the cluster node URLs (required)
                Urls = settings.RavenDB.ServerNodeUrls,

                // Set conventions as necessary (optional)
                Conventions =
                {
                    MaxNumberOfRequestsPerSession = settings.RavenDB.MaxNumberOfRequestsPerSession,
                    UseOptimisticConcurrency = true
                },

                // Define a default database (optional)
                Database = settings.RavenDB.DatabaseName,

                // Define a client certificate (optional)
                // A public/secure instance of RavenDB requires authentication via certificate
                Certificate = !string.IsNullOrEmpty(settings.RavenDB.Certificate) ? new X509Certificate2(Convert.FromBase64String(settings.RavenDB.Certificate)) : null
            }.Initialize();

            PreInitializeDocumentStore(store);
            return store;
        }

        /// <summary>
        ///     Configure RavenDB Document Store
        /// </summary>
        public void PreInitializeDocumentStore(IDocumentStore store)
        {
            store.Conventions.UseOptimisticConcurrency = true;

            // If a property is missing in the DB, default values are assigned during serialization
            store.Conventions.Serialization = new NewtonsoftJsonSerializationConventions
            {
                CustomizeJsonSerializer = serializer => serializer.NullValueHandling = NullValueHandling.Ignore
            };

            // Set one collection for derived classes
            store.Conventions.FindCollectionName = type =>
            {
                //if (typeof(Models.Indentity.User).IsAssignableFrom(type))
                //    return DocumentConventions.DefaultGetCollectionName(typeof(Models.Indentity.User));

                return DocumentConventions.DefaultGetCollectionName(type);
            };
        }
    }
}
