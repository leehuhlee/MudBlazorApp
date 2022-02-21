using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazorApp.Client.Shared.Dialogs;
using MudBlazorApp.Shared.Constants;
using MudBlazorApp.Shared.Models;
using MudBlazorApp.Shared.Requests;

namespace MudBlazorApp.Client.Pages.Authentication
{
    public partial class AccountPage
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private char _firstLetterOfName;
        private User user = new User();

        public string UserId { get; set; }
        private IBrowserFile _file;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            user = await AuthService.GetUser();
            if (user.FirstName.Length > 0)
            {
                _firstLetterOfName = user.FirstName[0];
            }
        }

        public async void HandleAccount()
        {
            var result = await AuthService.UpdateUserDetails(user);
            await LocalStorage.SetItemAsync("authToken", result.Data);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            SnackBar.Add(result.Message, Severity.Success);
        }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var fileName = $"{UserId}-{Guid.NewGuid()}{extension}";
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                var request = new UpdateProfilePictureRequest { Data = buffer, FileName = fileName, Extension = extension, UploadType = MudBlazorApp.Shared.Enums.UploadType.ProfilePicture };
                var result = await AuthService.UpdateProfilePictureAsync(request);
                if (result.Data)
                {
                    await LocalStorage.SetItemAsync(StorageConstants.Local.UserImageURL, result.Data);
                    SnackBar.Add(result.Message, Severity.Success);
                    NavigationManager.NavigateTo("/profile", true);
                }
                else
                {
                    SnackBar.Add(result.Message, Severity.Error);
                }
            }
        }

        private async Task DeleteAsync()
        {
            var parameters = new DialogParameters
            {
                {nameof(DeleteConfirmation.ContentText), "Do you want to delete the profile picture?" }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var request = new UpdateProfilePictureRequest { Data = null, FileName = string.Empty, UploadType = MudBlazorApp.Shared.Enums.UploadType.ProfilePicture };
                var data = await AuthService.UpdateProfilePictureAsync(request);
                if (data.Success)
                {
                    await LocalStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
                    SnackBar.Add(data.Message, Severity.Success);
                    NavigationManager.NavigateTo("/profile", true);
                }
                else
                {
                    SnackBar.Add(data.Message, Severity.Error);
                }
            }
        }
    }
}
