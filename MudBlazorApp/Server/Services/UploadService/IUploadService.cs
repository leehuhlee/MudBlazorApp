using MudBlazorApp.Shared.Requests;

namespace MudBlazorApp.Server.Services.UploadService
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}
