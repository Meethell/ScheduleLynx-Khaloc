using Client.Views;
using System;
using System.Linq;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

public class AppNotificationService
{
    private readonly Notifier _notifier;

    public AppNotificationService()
    {
        _notifier = new Notifier(cfg =>
        {
            // Set MainView to MainWindow
            Application.Current.MainWindow = Application.Current.Windows.OfType<MainView>().FirstOrDefault();
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 50);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3), // Thời gian hiển thị thông báo
                maximumNotificationCount: MaximumNotificationCount.FromCount(5)); // Số thông báo tối đa

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
    }

    public void ShowSuccess(string message)
    {
        _notifier.ShowSuccess(message);
    }

    public void ShowError(string message)
    {
        _notifier.ShowError(message);
    }

    public void ShowInformation(string message)
    {
        _notifier.ShowInformation(message);
    }

    public void ShowWarning(string message)
    {
        _notifier.ShowWarning(message);
    }
}
