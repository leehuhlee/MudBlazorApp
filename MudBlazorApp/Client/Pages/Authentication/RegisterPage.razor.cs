using Blazored.FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Authentication
{
    public partial class RegisterPage
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        UserRegister user = new UserRegister();

        string message = string.Empty;
        string messageCssClass = string.Empty;
        private string returnUrl = string.Empty;

        protected override void OnInitialized()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
            {
                returnUrl = url;
            }
        }

        async Task HandleRegistration()
        {
            var result = await AuthService.Register(user);
            message = result.Message;
            if (result.Success) 
            { 
                messageCssClass = "text-success";
                UserLogin userLogin = new UserLogin() 
                {
                    Email = user.Email,
                    Password = user.Password
                };
                var resultLogin = await AuthService.Login(userLogin);
                await LocalStorage.SetItemAsync("authToken", resultLogin.Data);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                SnackBar.Add(resultLogin.Message, Severity.Success);
                NavigationManager.NavigateTo(returnUrl);
            }
            else
                messageCssClass = "text-danger";
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }
    }
}
