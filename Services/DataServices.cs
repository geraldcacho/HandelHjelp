
using HandleHjelp.Data;
using HandleHjelp.Data.Models;
using HandleHjelp.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace HandleHjelp.Services;

public class DataServices
{
    //private Container _storesContainer;
    //private Container _featuresContainer;
    //private Container _countriesContainer;
    //private readonly CosmosClient _cosmosClient;
    private readonly HandelhjelpContext _handelhjelpContext;

    public DataServices(HandelhjelpContext handelhjelpContext)
    {
        //var databaseName = configuration["Database:Name"];
        //var connectionString = configuration["Database:ConnectionString"];
        //var serializerOptions = new CosmosSerializationOptions
        //{
        //    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
        //    IgnoreNullValues = true
        //};

        //_cosmosClient = new CosmosClientBuilder(connectionString)
        //    .WithSerializerOptions(serializerOptions)
        //    .Build();

        //InitializeDatabaseAndContainer(databaseName).Wait();

        _handelhjelpContext = handelhjelpContext;

        //var test = _handelhjelpContext.Orders.ToList();
    }

    private async Task InitializeDatabaseAndContainer(string databaseName)
    {
        // Check if the database exists
        //DatabaseResponse database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);

        //// Create a container if it doesn't exist
        //_storesContainer = await database.Database.CreateContainerIfNotExistsAsync("stores", "/address/countryCode");
        //_featuresContainer = await database.Database.CreateContainerIfNotExistsAsync("features", "/id");
        //_countriesContainer = await database.Database.CreateContainerIfNotExistsAsync("countries", "/id");

        //if (database.StatusCode == HttpStatusCode.Created)
        //{
        //    // Load initial demo data
        //    await InsertDataAsync<Store>("./Data/stores.json", _storesContainer);
        //    await InsertDataAsync<Feature>("./Data/features.json", _featuresContainer);
        //    await InsertDataAsync<Country>("./Data/countries.json", _countriesContainer);
        //}
    }

    //private static async Task InsertDataAsync<T>(string jsonFilePath, Container container)
    //{
    //    try
    //    {
    //        if (!File.Exists(jsonFilePath))
    //        {
    //            throw new Exception($"JSON file '{jsonFilePath}' not found. Make sure the file exists and try again.");
    //        }

    //        string jsonContent = File.ReadAllText(jsonFilePath);

    //        var options = new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        };

    //        var items = JsonSerializer.Deserialize<List<T>>(jsonContent, options);

    //        foreach (var item in items)
    //        {
    //            await container.UpsertItemAsync(item);
    //        }
    //    }
    //    catch (JsonException ex)
    //    {
    //        Console.WriteLine($"Invalid JSON format in the file '{jsonFilePath}'. Please make sure the JSON is valid. - {ex.Message}");

    //        throw;
    //    }
    //    catch (CosmosException ex)
    //    {
    //        Console.WriteLine($"Cosmos DB error: {ex.StatusCode} - {ex.Message}");

    //        throw;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"An error occurred while inserting {typeof(T).Name} data: {ex.Message}");

    //        throw;
    //    }
    //}

    //public async Task<List<ListStoreModel>> GetStoresAsync()
    //{
    //var queryText = "SELECT s.id, s.name, s.address.streetAddressLine1 AS address, s.address.city AS city, s.address.countryName AS country FROM s ORDER BY s.name";
    //var queryDefinition = new QueryDefinition(queryText);

    //return await QueryStoresAsync<ListStoreModel>(queryDefinition);

    //    throw new NotImplementedException();
    //}

    public async Task<Country> GetCountryByIdAsync(string id)
    {
        return await _handelhjelpContext.Countries.FirstAsync(self => self.Code == id);
    }

    public async Task<List<SupportedOrderType>> GetSupportedOrderTypeAsync()
    {
        var orderTypes = await _handelhjelpContext.ProductTypes
                                .Select(self => new OrderTypeSubCategory { Id = self.Code, Name = self.Name })
                                .ToListAsync();

        return [new SupportedOrderType
        {
            OrderTypeId = 1,
            Name = "Please select Product Types",
            SubCategories = orderTypes
        }];
    }

    //public async Task<List<Feature>> GetFeaturesAsync()
    //{
    //var queryText = "SELECT * FROM f";
    //var queryDefinition = new QueryDefinition(queryText);

    //var queryResult = _featuresContainer.GetItemQueryIterator<Feature>(queryDefinition);
    //var items = new List<Feature>();

    //while (queryResult.HasMoreResults)
    //{
    //    var response = await queryResult.ReadNextAsync();
    //    items.AddRange(response);
    //}

    //return items;
    //    throw new NotImplementedException();
    //}

    //public async Task<List<Country>> GetCountriesAsync()
    //{
    //var queryText = "SELECT * FROM c";
    //var queryDefinition = new QueryDefinition(queryText);

    //var queryResult = _countriesContainer.GetItemQueryIterator<Country>(queryDefinition);
    //var items = new List<Country>();

    //while (queryResult.HasMoreResults)
    //{
    //    var response = await queryResult.ReadNextAsync();
    //    items.AddRange(response);
    //}

    //return items;
    //    throw new NotImplementedException();
    //}

    //public async Task<Store> GetStoreByIdAsync(string id)
    //{
    //var queryText = "SELECT * FROM s WHERE s.id = @id";
    //var queryDefinition = new QueryDefinition(queryText)
    //    .WithParameter("@id", id);

    //return (await QueryStoresAsync<Store>(queryDefinition)).FirstOrDefault();
    //    throw new NotImplementedException();
    //}

    public async Task<List<OrderWithDistance>> GetOrdersAsync(string query, int? limit, string countryCode, string tags, double? latitude, double? longitude, double? rangeInKm)
    {
        var sqlQuery = "SELECT * FROM [hand].[Order] WHERE ";
        var queryParams = new List<SqlParameter>();
        var parameterCount = 0;

        if (!string.IsNullOrEmpty(query))
        {
            parameterCount++;

            var searchText = query.ToLower();
            sqlQuery += "LOWER(City) LIKE @city";
            queryParams.Add(new SqlParameter("city", $"{searchText}%"));
        }

        if (!string.IsNullOrEmpty(countryCode))
        {
            parameterCount++;

            if (queryParams.Any())
                sqlQuery += " AND ";

            sqlQuery += "LOWER(CountryCode) = @country";
            queryParams.Add(new SqlParameter("country", countryCode.ToLower()));
        }

        if (longitude.HasValue && latitude.HasValue && rangeInKm.HasValue)
        {
            parameterCount++;

            if (queryParams.Any())
                sqlQuery += " AND ";

            sqlQuery += "(geometry::Point(@longitude, @latitude, 4326)).STDistance(geometry::Point(Longitude, Latitude, 4326)) <= @rangeInKm * 1000";

            queryParams.Add(new SqlParameter("longitude", longitude));
            queryParams.Add(new SqlParameter("latitude", latitude));
            queryParams.Add(new SqlParameter("rangeInKm", rangeInKm));
        }

        // If no parameters are specified, return all stores
        if (parameterCount == 0)
        {
            sqlQuery += " 1 = 1";
        }

        //    // Do not use limit if you are using a range search
        if (limit.HasValue && !rangeInKm.HasValue)
        {
            sqlQuery += " OFFSET 0 LIMIT @limit";
            queryParams.Add(new SqlParameter("limit", limit.Value));
        }

        var orders = await _handelhjelpContext.Orders
                            .FromSqlRaw(sqlQuery, queryParams.ToArray())
                            //.FromSqlRaw(sqlQuery
                            //, new SqlParameter("latitude", latitude) { DbType = System.Data.DbType.Decimal }
                            //, new SqlParameter("longitude", longitude) { DbType = System.Data.DbType.Decimal }
                            //, new SqlParameter("rangeInKm", rangeInKm) { DbType = System.Data.DbType.Decimal })
                            .ToListAsync();

        var ordersWithDistance = new List<OrderWithDistance>();

        foreach (var order in orders)
        {
            var newOrderWithDistance = new OrderWithDistance(order);

            if (longitude.HasValue && latitude.HasValue)
            {
                newOrderWithDistance.DistanceInKm = GeospatialHelper.CalculateDistanceInKm((double)latitude, (double)longitude, (double)order.Latitude, (double)order.Longitude);
            }

            ordersWithDistance.Add(newOrderWithDistance);
        };

        return ordersWithDistance;
    }

    //public async Task<List<StoreWithDistance>> GetStoresBySearchAsync(string query, int? limit, string countryCode, string tags, double? latitude, double? longitude, double? rangeInKm)
    //{
    //    var sqlQuery = "SELECT * FROM s WHERE ";
    //    var queryParams = new List<(string, object)>();
    //    var parameterCount = 0;

    //    if (!string.IsNullOrEmpty(query))
    //    {
    //        parameterCount++;

    //        var searchText = query.ToLower();
    //        sqlQuery += "LOWER(s.name) LIKE @name OR LOWER(s.address.city) LIKE @city";
    //        queryParams.Add(("@name", $"%{searchText}%"));
    //        queryParams.Add(("@city", $"{searchText}%"));
    //    }

    //    if (!string.IsNullOrEmpty(countryCode))
    //    {
    //        parameterCount++;

    //        if (queryParams.Any())
    //            sqlQuery += " AND ";

    //        sqlQuery += "LOWER(s.address.countryCode) = @country";
    //        queryParams.Add(("@country", countryCode.ToLower()));
    //    }

    //    if (!string.IsNullOrEmpty(tags))
    //    {
    //        parameterCount++;

    //        var tagCount = 0;
    //        var tagsArray = tags.Split(',');

    //        foreach (var tag in tagsArray)
    //        {
    //            if (queryParams.Any())
    //                sqlQuery += " AND ";

    //            tagCount++;

    //            sqlQuery += $"ARRAY_CONTAINS(s.features, @tag{tagCount})";
    //            queryParams.Add(($"@tag{tagCount}", tag.ToLower()));
    //        }
    //    }

    //    if (longitude.HasValue && latitude.HasValue && rangeInKm.HasValue)
    //    {
    //        parameterCount++;

    //        if (queryParams.Any())
    //            sqlQuery += " AND ";

    //        sqlQuery += "ST_DISTANCE(s.location, { 'type': 'Point', 'coordinates': [@longitude, @latitude] }) <= @rangeInKm * 1000";

    //        queryParams.Add(("@latitude", latitude));
    //        queryParams.Add(("@longitude", longitude));
    //        queryParams.Add(("@rangeInKm", rangeInKm));
    //    }

    //    // If no parameters are specified, return all stores
    //    if (parameterCount == 0)
    //    {
    //        sqlQuery += " 1 = 1";
    //    }

    //    // Do not use limit if you are using a range search
    //    if (limit.HasValue && !rangeInKm.HasValue)
    //    {
    //        sqlQuery += " OFFSET 0 LIMIT @limit";
    //        queryParams.Add(("@limit", limit.Value));
    //    }

    //    var queryDefinition = new QueryDefinition(sqlQuery);

    //    foreach (var (paramName, paramValue) in queryParams)
    //    {
    //        queryDefinition.WithParameter(paramName, paramValue);
    //    }

    //    var stores = await QueryStoresAsync<StoreWithDistance>(queryDefinition);

    //    if (longitude.HasValue && latitude.HasValue)
    //    {
    //        foreach (var store in stores)
    //        {
    //            store.DistanceInKm = GeospatialHelper.CalculateDistanceInKm((double)latitude, (double)longitude, store.Location.Coordinates[1], store.Location.Coordinates[0]);
    //        }
    //    }

    //    return stores;
    //}

    //public async Task DeleteStoreAsync(Store store)
    //{
    //var response = await _storesContainer.DeleteItemAsync<Store>(store.Id, new PartitionKey(store.Address.CountryCode));

    //if (response.StatusCode != HttpStatusCode.NoContent)
    //{
    //    throw new Exception($"Failed to delete store. Status code: {response.StatusCode}");
    //}

    //    throw new NotImplementedException();
    //}

    public async Task UpdateOrderAsync(Order order)
    {
        var dbOrder = _handelhjelpContext.Orders.FirstOrDefault(self => self.OrderId == order.OrderId);

        if (dbOrder != null)
        {
            dbOrder.Image = order.Image;
            dbOrder.City = order.City;
            dbOrder.TotalAmount = order.TotalAmount;
            dbOrder.StreetAddressLine1 = order.StreetAddressLine1;
            dbOrder.PostalCode = order.PostalCode;
            dbOrder.CountryCode = order.CountryCode;
            dbOrder.CountryName = order.CountryName;
            dbOrder.Latitude = order.Latitude;
            dbOrder.Longitude = order.Longitude;
            dbOrder.PhoneNumber = order.PhoneNumber;
            dbOrder.ReceivedOn = order.ReceivedOn;
            dbOrder.Note = order.Note;
            dbOrder.UpdatedBy = order.CreatedBy;
            dbOrder.UpdatedDate = DateTime.Now;

            _handelhjelpContext.Update(dbOrder);
        }
        else
        {
            order.OrderDate = order.CreatedDate = DateTime.Now;
            await _handelhjelpContext.AddAsync(order);
        }

        await _handelhjelpContext.SaveChangesAsync();
    }

    //public async Task UpdateStoreAsync(Store store)
    //{
    //var response = await _storesContainer.ReplaceItemAsync<Store>(store, store.Id, new PartitionKey(store.Address.CountryCode));

    //if (response.StatusCode != HttpStatusCode.OK)
    //{
    //    throw new Exception($"Failed to update store details. Status code: {response.StatusCode}");
    //}

    //    throw new NotImplementedException();
    //}

    //private async Task<List<T>> QueryStoresAsync<T>(QueryDefinition queryDefinition)
    //{
    //    var queryResult = _storesContainer.GetItemQueryIterator<T>(queryDefinition);
    //    var items = new List<T>();

    //    while (queryResult.HasMoreResults)
    //    {
    //        var response = await queryResult.ReadNextAsync();
    //        items.AddRange(response);
    //    }

    //    return items;
    //}
}