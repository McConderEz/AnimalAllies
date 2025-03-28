using System.Net;
using System.Net.Http.Json;
using FileService.Contract;
using FileService.Contract.Requests;
using FileService.Contract.Responses;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient)
{
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
}