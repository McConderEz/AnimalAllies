using System.Net;
using System.Net.Http.Json;
using FileService.Contract;
using FileService.Contract.Requests;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient)
{
    public async Task<IEnumerable<FileWebResponse>> GetFilePresignedUrlAsync(
        DownloadPresignedUrlRequest request, CancellationToken cancellationToken = default)
    {
        //await httpClient.PostAsJsonAsync($"files/{key:guid}/presigned-for-downloading")
        return null;
    }
}