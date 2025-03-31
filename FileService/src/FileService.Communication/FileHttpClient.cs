using System.Net;
using System.Net.Http.Json;
using FileService.Contract;
using FileService.Contract.Requests;
using FileService.Contract.Responses;

namespace FileService.Communication;

/// <summary>
/// Файловый клиент для взаимодействия с файловым сервисом
/// </summary>
/// <param name="httpClient"></param>
public class FileHttpClient(HttpClient httpClient)
{
    /// <summary>
    /// Получить ссылки на загрузку файлов в S3 хранилище
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<GetUploadPresignedUrlResponse?> GetUploadPresignedUrlAsync(
        UploadPresignedUrlRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "files/presigned-for-uploading",
            request, 
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        
        var fileResponse = await response.Content.ReadFromJsonAsync<GetUploadPresignedUrlResponse>(cancellationToken);

        return fileResponse;
    }
    
    /// <summary>
    /// Получение нескольких ссылок на загрузку файлов в S3
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="cancellationToken">Токен отмен</param>
    /// <returns></returns>
    public async Task<IEnumerable<GetUploadPresignedUrlResponse>?> GetManyUploadPresignedUrlsAsync(
        UploadPresignedUrlsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "files/presigned-urls-for-uploading",
            request, 
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        
        var fileResponse = await response.Content
            .ReadFromJsonAsync<IEnumerable<GetUploadPresignedUrlResponse>>(cancellationToken);

        return fileResponse;
    }
    
    /// <summary>
    /// Получение ссылки на скачивание файла из S3
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="cancellationToken">Токен отмен</param>
    /// <returns></returns>
    public async Task<GetDownloadPresignedUrlResponse?> GetDownloadPresignedUrlAsync(
        DownloadPresignedUrlRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "files/presigned-for-downloading",
            request, 
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        
        var fileResponse = await response.Content
            .ReadFromJsonAsync<GetDownloadPresignedUrlResponse>(cancellationToken);

        return fileResponse;
    }
    
    /// <summary>
    /// Получение ссылки на скачивагие файла из S3
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="cancellationToken">Токен отмен</param>
    /// <returns></returns>
    public async Task<GetDeletePresignedUrlsResponse?> GetDeletePresignedUrlAsync(
        DeletePresignedUrlsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "files/presigned-for-deletion",
            request, 
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        
        var fileResponse = await response.Content
            .ReadFromJsonAsync<GetDeletePresignedUrlsResponse>(cancellationToken);

        return fileResponse;
    }
    
    /// <summary>
    /// Получение ссылки на удаление файла из S3
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="cancellationToken">Токен отмен</param>
    /// <returns></returns>
    public async Task<GetFilesByIdsResponse?> GetFilesByIdsAsync(
        GetFilesByIdsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "files/files-by-ids",
            request, 
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        
        var fileResponse = await response.Content
            .ReadFromJsonAsync<GetFilesByIdsResponse>(cancellationToken);

        return fileResponse;
    }
}