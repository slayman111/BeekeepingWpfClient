using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BeekeepingWpfClient.Core;
using BeekeepingWpfClient.Model.Response;

namespace BeekeepingWpfClient.ViewModel;

public class DashboardViewModel : BaseViewModel
{
    private ObservableCollection<GetAllRequestsResponse>? _requests;

    public ObservableCollection<GetAllRequestsResponse>? Requests
    {
        get => _requests;
        private set => SetProperty(ref _requests, value);
    }

    public ICommand OpenNewProductWindowCommand { get; private set; }
    public ICommand OpenNewEmployeeWindowCommand { get; private set; }
    public ICommand SignOutCommand { get; private set; }

    public DashboardViewModel()
    {
        var httpService = new HttpService(AuthTokens.Access);

        Task.Run(async () =>
        {
            Requests = new ObservableCollection<GetAllRequestsResponse>(
                await httpService.GetRequestsAsync());
        });
    }
}
