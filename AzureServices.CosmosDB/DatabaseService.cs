using AzureServices.Common.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServices.CosmosDB
{
    public class DatabaseService
    {
        private static Database database;
        private static Container familyContainer;
        private static ItemRequestOptions itemRequestOptions;
        public DatabaseService()
        {
            if (database == null)
                database = DatabaseConnection.database;
            if (itemRequestOptions == null)
                itemRequestOptions = new ItemRequestOptions() { EnableContentResponseOnWrite = false };
            // https://docs.microsoft.com/en-us/azure/cosmos-db/index-policy
            // 
        }

        public static Container InitializeContainer(Container container, string containerName, string partitionKey, int containerTTL)
        {
            if (container == null)
            {
                ContainerProperties containerProperties = new ContainerProperties(containerName, $"/{partitionKey}");
                containerProperties.IndexingPolicy.Automatic = true;
                containerProperties.DefaultTimeToLive = containerTTL;
                containerProperties.IndexingPolicy.IndexingMode = IndexingMode.Consistent;
                container = database.CreateContainerIfNotExistsAsync(containerProperties).Result;
            }
            return container;
        }

        public async Task CreateItem(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                // Creates an item - Partition Key retrieved automatically from item, by matching the given partition key while creation of container
                // Advised to provide partition key while the creation of an item, it will reduce the computation
                //ItemResponse<Family> response1 = await familyContainer.CreateItemAsync<Family>(family);

                // with partition key
                //ItemResponse<Family> response2 = await familyContainer.CreateItemAsync<Family>(family, new PartitionKey(family.Category));


                ItemResponse<Family> response3 = await familyContainer.CreateItemAsync<Family>(family,
                    new PartitionKey(family.Category),
                    itemRequestOptions
                   );
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task DeleteItem(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                // ItemRequestOptions -> null
                ItemResponse<Family> response1 = await familyContainer.DeleteItemAsync<Family>(family.Id, new PartitionKey(family.Category));

                // ItemRequestOptions -> EnableContentResponseOnWrite: false
                ItemResponse<Family> response2 = await familyContainer.DeleteItemAsync<Family>(family.Id, new PartitionKey(family.Category), itemRequestOptions);

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /*
         GetItemLinqQueryable<T>(bool allowSynchronousQueryExecution = false, string continuationToken = null, QueryRequestOptions requestOptions = null, CosmosLinqSerializerOptions linqSerializerOptions = null);
        */
        // Costlier than Feed iterator
        public async void GetItemLinqQueryable(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                //var resonse1 = familyContainer.GetItemLinqQueryable<Family>(true).Where(x => x.Category.Equals(family.Category)).AsEnumerable().FirstOrDefault();
                var query = familyContainer.GetItemLinqQueryable<Family>(true).Where(x => x.Category.Equals(family.Category));
                var iterator = query.ToFeedIterator();
                var response = await iterator.ReadNextAsync();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /*
        GetItemQueryIterator<T>(QueryDefinition queryDefinition, string continuationToken = null, QueryRequestOptions requestOptions = null);
        GetItemQueryIterator<T>(FeedRange feedRange, QueryDefinition queryDefinition, string continuationToken = null, QueryRequestOptions requestOptions = null);
        GetItemQueryIterator<T>(string queryText = null, string continuationToken = null, QueryRequestOptions requestOptions = null);
         */
        public async Task GetItemQueryIterator(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                string query = "select c.FamilyName, c.Caste from c";
                QueryDefinition queryDef = new QueryDefinition(query);
                FeedResponse<Family> response1 = await familyContainer.GetItemQueryIterator<Family>(queryDef).ReadNextAsync();
                FeedResponse<Family> response2 = await familyContainer.GetItemQueryIterator<Family>().ReadNextAsync();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Work only for maximum 10 items
        /// 
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public async Task PatchItem(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                PatchItemRequestOptions patchItemRequestOptions = new PatchItemRequestOptions()
                {
                    EnableContentResponseOnWrite = false
                };

                List<PatchOperation> operations = new List<PatchOperation>()
                {
                    // Add<T>(string path, T value);
                    PatchOperation.Add("/prop1", "new prop"),
                    PatchOperation.Add<long>("/prop2_long", 25),
                    PatchOperation.Add<double>("/prop2_double", 25),
                    PatchOperation.Add("/prop3", "remove it"),
                    PatchOperation.Add("/prop4", "replace it"),
                    PatchOperation.Add("/prop5", "set it")
                };

                var response1 = await familyContainer.PatchItemAsync<Family>(family.Id, new PartitionKey(family.Category), operations);

                operations = new List<PatchOperation>()
                {
                    // Increment(string path, long value);
                    PatchOperation.Increment("/prop2_long", 10),

                    // Increment(string path, double value)
                    PatchOperation.Increment("/prop2_double", 15),

                    // Remove(string path);
                    PatchOperation.Remove("/prop3"),

                    // Replace<T>(string path, T value);
                    PatchOperation.Replace("/prop4", "it is replaced"),

                    // Set<T>(string path, T value);
                    PatchOperation.Set("/prop5", "It is setted")
                };

                var response2 = await familyContainer.PatchItemAsync<Family>(family.Id, new PartitionKey(family.Category), operations);

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public async Task ReadItem(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                ItemResponse<Family> response1 = await familyContainer.ReadItemAsync<Family>(family.Id, new PartitionKey(family.Category));
                ItemResponse<Family> response2 = await familyContainer.ReadItemAsync<Family>(family.Id, new PartitionKey(family.Category), itemRequestOptions);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task ReadManyItems(List<Family> families)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                IReadOnlyList<(string, PartitionKey)> itemList = new List<(string, PartitionKey)>();
                foreach (var family in families)
                {
                    itemList.Append((family.Id, new PartitionKey(family.Category)));
                }
                var response = await familyContainer.ReadManyItemsAsync<Family>(itemList);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task ReplaceItem(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);

                var response = await familyContainer.ReplaceItemAsync<Family>(family, family.Id, new PartitionKey(family.Category), itemRequestOptions);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task UpsertItem(Family family)
        {
            try
            {
                familyContainer = InitializeContainer(familyContainer, "Family", "Category", 15552000);
                string query = $"select * from c where c.Category = '{family.Category}' and c.Caste = '{family.Caste}'";
                var iterator = await familyContainer.GetItemQueryIterator<Family>(query).ReadNextAsync();
                List<Family> families = new List<Family>();
                foreach(var item in iterator)
                {
                    families.Add(item);
                }
                var response1 = await familyContainer.UpsertItemAsync<Family>(family, new PartitionKey(family.Category));
                var response2 = await familyContainer.UpsertItemAsync<Family>(family, new PartitionKey(family.Category), itemRequestOptions);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task GetData()
        {
            try
            {
                Container container = database.GetContainer("Maintenance");
                var data = await container.GetItemQueryIterator<object>().ReadNextAsync();
                foreach(var item in data)
                {

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
