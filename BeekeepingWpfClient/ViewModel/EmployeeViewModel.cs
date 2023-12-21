using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BeekeepingWpfClient.Command;
using BeekeepingWpfClient.Core;
using BeekeepingWpfClient.Model.Request;
using BeekeepingWpfClient.Model.Response;
using BeekeepingWpfClient.View;

namespace BeekeepingWpfClient.ViewModel;

public class EmployeeViewModel : BaseViewModel
{
    private readonly HttpService _httpService;
    private string _fullName;
    private string _phone;
    private string _email;
    private string _login;
    private string _password;

    private ObservableCollection<GetAllUsersResponse> _users;

    public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
    public string Phone { get => _phone; set => SetProperty(ref _phone, value); }
    public string Email { get => _email; set => SetProperty(ref _email, value); }
    public string Login { get => _login; set => SetProperty(ref _login, value); }
    public string Password { get => _password; set => SetProperty(ref _password, value); }

    public ObservableCollection<GetAllUsersResponse> Users { get => _users; set => SetProperty(ref _users, value); }

    public ICommand BackCommand { get; private set; }
    public ICommand AddEmployeeCommand { get; private set; }
    public ICommand DeleteEmployeeCommand { get; private set; }

    public EmployeeViewModel()
    {
        _httpService = new HttpService(AuthTokens.Access);

        BackCommand = new DelegateCommand(Back);
        AddEmployeeCommand = new DelegateCommand(AddEmployee);
        DeleteEmployeeCommand = new DelegateCommand(DeleteEmployee);

        Task.Run(async () => { await LoadEmployees(); });
    }

    private async Task LoadEmployees() =>
        Users = new ObservableCollection<GetAllUsersResponse>(await _httpService.GetAllUsersAsync());

    private async void DeleteEmployee(object id)
    {
        if (!MessageBoxService.Delete("Вы уверены что хотите удалить сотрудника?")) return;

        await _httpService.DeleteUserAsync((int)id);
        await LoadEmployees();
    }

    private async void AddEmployee(object obj)
    {
        await _httpService.RegisterAsync(new RegisterRequest(FullName, Phone, Email, Login, Password));
        await LoadEmployees();
    }

    private static void Back(object obj) => WindowManager.Open<DashboardWindow, EmployeeWindow>();
}
