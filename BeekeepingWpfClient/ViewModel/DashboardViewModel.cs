using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BeekeepingWpfClient.Command;
using BeekeepingWpfClient.Core;
using BeekeepingWpfClient.Model.Response;
using BeekeepingWpfClient.View;

namespace BeekeepingWpfClient.ViewModel;

public class DashboardViewModel : BaseViewModel
{
    private ObservableCollection<GetAllRequestsResponse>? _requests;

    public ObservableCollection<GetAllRequestsResponse>? Requests
    {
        get => _requests;
        private set => SetProperty(ref _requests, value);
    }

    public ICommand OpenProductWindowCommand { get; private set; }
    public ICommand OpenNewEmployeeWindowCommand { get; private set; }
    public ICommand SignOutCommand { get; private set; }

    public DashboardViewModel()
    {
        var httpService = new HttpService(AuthTokens.Access);

        SignOutCommand = new DelegateCommand(SignOut);
        OpenProductWindowCommand = new DelegateCommand(OpenProductWindow);

        Task.Run(async () =>
        {
            Requests = new ObservableCollection<GetAllRequestsResponse>(
                await httpService.GetRequestsAsync());
        });
    }

    private static void OpenProductWindow(object obj) =>
        WindowManager.Open<ProductWindow, DashboardWindow>();

    private static void SignOut(object obj)
    {
        AuthTokens.SetTokens(null, null);
        WindowManager.Open<AuthorizationWindow, DashboardWindow>();
    }
}
