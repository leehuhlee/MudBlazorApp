using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Administration
{
    public partial class AddUserModal
    {
		[CascadingParameter] private MudDialogInstance MudDialog { get; set; }
		[Parameter] public UserRegister User { get; set; } = new UserRegister();
		private FluentValidationValidator _fluentValidationValidator;
		private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

		protected async Task Save()
		{
			var result = await AuthService.Register(User);

            if (!result.Success)
            {
                SnackBar.Add(result.Message, Severity.Error);
                return;
            }

            SnackBar.Add(result.Message, Severity.Success);
            MudDialog.Close();
		}

		public void Cancel()
		{
			MudDialog.Cancel();
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
