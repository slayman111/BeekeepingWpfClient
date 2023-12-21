using System.Windows;

namespace BeekeepingWpfClient.Core;

public static class MessageBoxService
{
    public static void Info(string text) =>
        MessageBox.Show(text, "Info", MessageBoxButton.OK, MessageBoxImage.Information);

    public static bool Delete(string text) =>
        MessageBox.Show(text, "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
}
