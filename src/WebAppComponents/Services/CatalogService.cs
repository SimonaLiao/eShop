using System.Net.Http.Json;
using System.Web;
using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    private readonly string remoteServiceBaseUrl = "api/catalog/";

    public Task<CatalogItem?> GetCatalogItem(int id)
    {
        var uri = $"{remoteServiceBaseUrl}items/{id}";
        return httpClient.GetFromJsonAsync<CatalogItem>(uri);
    }

    public async Task<CatalogResult> GetCatalogItems(int pageIndex, int pageSize, int? brand, int? type)
    {
        string filterQs = string.Empty;
        if (type.HasValue)
        {
            filterQs += $"type={type.Value}&";
        }
        if (brand.HasValue)
        {
            filterQs += $"brand={brand.Value}&";
        }

        var uri = $"{remoteServiceBaseUrl}items?{filterQs}pageIndex={pageIndex}&pageSize={pageSize}";
        var result = await httpClient.GetFromJsonAsync<CatalogResult>(uri);
        return result!;
    }

    public async Task<List<CatalogItemObject>> GetCatalogItemObjects(int pageIndex, int pageSize, int? brand, int? type)
    {
        string filterQs = string.Empty;
        if (type.HasValue)
        {
            filterQs += $"type={type.Value}&";
        }
        if (brand.HasValue)
        {
            filterQs += $"brand={brand.Value}&";
        }
        var uri = $"{remoteServiceBaseUrl}items?{filterQs}pageIndex={pageIndex}&pageSize={pageSize}";
        var result = await httpClient.GetFromJsonAsync<CatalogResult>(uri);

        var newRes = result?.Data
            .Select(item => new CatalogItemObject(item))
            .ToList() ?? new List<CatalogItemObject>();
        return newRes;
    }

    public async Task<List<CatalogItem>> GetCatalogItems(IEnumerable<int> ids)
    {
        var uri = $"{remoteServiceBaseUrl}items/by?ids={string.Join("&ids=", ids)}";
        var result = await httpClient.GetFromJsonAsync<List<CatalogItem>>(uri);
        return result!;
    }

    public Task<CatalogResult> GetCatalogItemsWithSemanticRelevance(int page, int take, string text)
    {
        var url = $"{remoteServiceBaseUrl}items/withsemanticrelevance?text={HttpUtility.UrlEncode(text)}&pageIndex={page}&pageSize={take}";
        var result = httpClient.GetFromJsonAsync<CatalogResult>(url);
        return result!;
    }

    public async Task<IEnumerable<CatalogBrand>> GetBrands()
    {
        // Todo: Implement in a similar way as GetCatalogItems using GetFromJsonAsync.
        throw new NotImplementedException("This method is not implemented yet.");
    }

    public async Task<IEnumerable<CatalogItemType>> GetTypes()
    {
        // Todo: Implement in a similar way as GetCatalogItems using GetFromJsonAsync.
        throw new NotImplementedException("This method is not implemented yet.");
    }
}
