using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BeekeepingWpfClient.Command;
using BeekeepingWpfClient.Core;
using BeekeepingWpfClient.Model.Request;
using BeekeepingWpfClient.Model.Response;
using BeekeepingWpfClient.View;
using Microsoft.Win32;

namespace BeekeepingWpfClient.ViewModel;

public class ProductViewModel : BaseViewModel
{
    private readonly HttpService _httpService;
    private string _name;
    private decimal _price;
    private GetAllProductTypeResponse _selectedGetAllProductType;
    private ObservableCollection<GetAllProductTypeResponse> _productTypes;
    private ObservableCollection<GetAllProductsResponse>? _products;
    private string _filePath;
    private BitmapImage? _image;

    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public decimal Price { get => _price; set => SetProperty(ref _price, value); }
    public string FilePath { get => _filePath; set => SetProperty(ref _filePath, value); }


    public GetAllProductTypeResponse SelectedGetAllProductType
    {
        get => _selectedGetAllProductType;
        set => SetProperty(ref _selectedGetAllProductType, value);
    }

    public ObservableCollection<GetAllProductTypeResponse> ProductTypes
    {
        get => _productTypes;
        set => SetProperty(ref _productTypes, value);
    }

    public ObservableCollection<GetAllProductsResponse>? Products
    {
        get => _products;
        private set => SetProperty(ref _products, value);
    }

    public ICommand BackCommand { get; private set; }
    public ICommand ChooseImageCommand { get; private set; }
    public ICommand AddProductCommand { get; private set; }
    public ICommand DeleteProductCommand { get; private set; }

    public ProductViewModel()
    {
        _httpService = new HttpService(AuthTokens.Access);

        BackCommand = new DelegateCommand(Back);
        ChooseImageCommand = new DelegateCommand(ChooseImage);
        AddProductCommand = new DelegateCommand(AddProduct);
        DeleteProductCommand = new DelegateCommand(DeleteProduct);

        FilePath = "Выберите файл";

        Task.Run(async () =>
        {
            await LoadProductTypesAsync();
            await LoadProductsAsync();
        });
    }

    private async void DeleteProduct(object id)
    {
        if (!MessageBoxService.Delete("Вы уверены что хотите удалить товар?")) return;

        await _httpService.DeleteProductAsync((int)id);
        await LoadProductsAsync();
    }

    private async void AddProduct(object obj)
    {
        await _httpService.CreateProductAsync(
            new CreateProductRequest(Name, Price, SelectedGetAllProductType.Id), _image);

        await LoadProductsAsync();
    }

    private void ChooseImage(object obj)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files|*.bmp;*.jpg;*.png",
            FilterIndex = 1
        };

        if (openFileDialog.ShowDialog() is not true) return;

        FilePath = openFileDialog.FileName;
        _image = new BitmapImage(new Uri(openFileDialog.FileName));
    }

    private async Task LoadProductTypesAsync() =>
        ProductTypes = new ObservableCollection<GetAllProductTypeResponse>(await _httpService.GetProductTypesAsync());

    private async Task LoadProductsAsync() =>
        Products = new ObservableCollection<GetAllProductsResponse>(await _httpService.GetProductsAsync());

    private static void Back(object obj) => WindowManager.Open<DashboardWindow, ProductWindow>();
}
