using System.Windows.Input;
using BeekeepingWpfClient.Command;
using BeekeepingWpfClient.Core;
using BeekeepingWpfClient.Exception;
using BeekeepingWpfClient.Model.Request;

namespace BeekeepingWpfClient.ViewModel;

public class AuthorizationViewModel : BaseViewModel
{
    private string? _password;
    private string? _login;

    public string? Password { get => _password; set => SetProperty(ref _password, value); }
    public string? Login { get => _login; set => SetProperty(ref _login, value); }

    public ICommand SignInCommand { get; private set; }

    public AuthorizationViewModel()
    {
        SignInCommand = new DelegateCommand(SignIn);
    }

    private async void SignIn(object obj)
    {
        var httpService = new HttpService();

        if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            throw new EmptyCredentialsException();

        var response = await httpService.LoginAsync(new LoginRequest(Login, Password));

        MessageBoxService.Info(response.ToString());
    }
}
