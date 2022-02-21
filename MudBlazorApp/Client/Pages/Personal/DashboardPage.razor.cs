using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazorApp.Shared.Constants;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Personal
{
    public partial class DashboardPage
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private bool _loaded;
        private readonly string[] _dataEnterBarChartXAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private Dashboard dashboard = new Dashboard();

        protected override async Task OnInitializedAsync()
        {
            await GetDashboard();
            _loaded = true;
            HubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
            .Build();
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await GetDashboard();
                StateHasChanged();
            });
            await HubConnection.StartAsync();
        }

        private async Task GetDashboard()
        {
            dashboard.ProductCount = await ProductService.GetProductCountAsync();
            dashboard.BrandCount = await BrandService.GetBrandCountAsync();
            dashboard.UserCount = await UserService.GetUserCountAsync();
            dashboard.RoleCount = await RoleService.GetRoleCountAsync();
            dashboard.DataEnterBarChart.Clear();
            dashboard.DataEnterBarChart.Add(new ChartSeries()
            {
                Name = "Product",
                Data = await ProductService.GetProductMonthlyCountAsync()
            }); 
            dashboard.DataEnterBarChart.Add(new ChartSeries()
            {
                Name = "Brand",
                Data = await BrandService.GetBrandMonthlyCountAsync()
            });
            dashboard.DataEnterBarChart.Add(new ChartSeries()
            {
                Name = "User",
                Data = await UserService.GetUserMonthlyCountAsync()
            });
            dashboard.DataEnterBarChart.Add(new ChartSeries()
            {
                Name = "Role",
                Data = await RoleService.GetRoleMonthlyCountAsync()
            });
        }
    }

    
}
