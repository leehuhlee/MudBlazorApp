using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudBlazorApp.Client.Shared.Dialogs
{
    public partial class DeleteConfirmation
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public string ContentText { get; set; }

        void Submit()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
        void Cancel() => MudDialog.Cancel();
    }
}
