using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BeekeepingWpfClient.Command;
using BeekeepingWpfClient.Core;
using BeekeepingWpfClient.Model.Request;
using BeekeepingWpfClient.Model.Response;
using BeekeepingWpfClient.View;

namespace BeekeepingWpfClient.ViewModel;

public class DashboardViewModel : BaseViewModel
{
    private readonly HttpService _httpService;
    private ObservableCollection<GetAllRequestsResponse>? _requests;
    private ObservableCollection<GetAllProductsResponse>? _products;
    private GetAllProductsResponse _selectedProduct;
    private short _amount;
    private DateTime _selectedDate;

    public ObservableCollection<GetAllRequestsResponse>? Requests
    {
        get => _requests;
        private set => SetProperty(ref _requests, value);
    }

    public ObservableCollection<GetAllProductsResponse>? Products
    {
        get => _products;
        set => SetProperty(ref _products, value);
    }

    public GetAllProductsResponse SelectedProduct
    {
        get => _selectedProduct;
        set => SetProperty(ref _selectedProduct, value);
    }

    public short Amount { get => _amount; set => SetProperty(ref _amount, value); }
    public DateTime SelectedDate { get => _selectedDate; set => SetProperty(ref _selectedDate, value); }

    public ICommand OpenProductWindowCommand { get; private set; }
    public ICommand OpenNewEmployeeWindowCommand { get; private set; }
    public ICommand SignOutCommand { get; private set; }
    public ICommand AddRequestCommand { get; private set; }

    public DashboardViewModel()
    {
        _httpService = new HttpService(AuthTokens.Access);

        SelectedDate = DateTime.Now;

        SignOutCommand = new DelegateCommand(SignOut);
        OpenProductWindowCommand = new DelegateCommand(OpenProductWindow);
        OpenNewEmployeeWindowCommand = new DelegateCommand(OpenNewEmployeeWindow);
        AddRequestCommand = new DelegateCommand(AddRequest);

        Task.Run(async () =>
        {
            await LoadRequestsAsync();

            await LoadProductsAsync();
        });
    }

    private async void AddRequest(object obj)
    {
        await _httpService.CreateRequestAsync(
            new CreateRequestRequest(SelectedProduct.Id, Amount, ((DateTimeOffset)SelectedDate).ToUnixTimeMilliseconds()));

        await LoadRequestsAsync();
    }

    private async Task LoadProductsAsync() =>
        Products = new ObservableCollection<GetAllProductsResponse>(await _httpService.GetProductsAsync());

    private async Task LoadRequestsAsync() =>
        Requests = new ObservableCollection<GetAllRequestsResponse>(await _httpService.GetRequestsAsync());

    private static void OpenNewEmployeeWindow(object obj) =>
        WindowManager.Open<EmployeeWindow, DashboardWindow>();

    private static void OpenProductWindow(object obj) =>
        WindowManager.Open<ProductWindow, DashboardWindow>();

    private static void SignOut(object obj)
    {
        AuthTokens.SetTokens(null, null);
        WindowManager.Open<AuthorizationWindow, DashboardWindow>();
    }
}
