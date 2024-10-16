using System.ComponentModel;
using System.Windows;
using CathayScraperApp.Assets.Domain.UseCases;

namespace CathayScraperApp.Assets.Presentation;

public partial class APIKeyVerificationWindow : Window
{
    private readonly APIKeyVerificationViewModel _viewModel;
    public APIKeyVerificationWindow()
    {
        InitializeComponent();
        var storeApiKeyUseCase = new StoreApiKeyUseCase();
        _viewModel = new APIKeyVerificationViewModel(
                        new VerifyAPIKeyUseCase(
                            new MailRepository(
                                new MailAPI())),
                        storeApiKeyUseCase);
        DoneButton.IsEnabled = false;
        StatusLabel.Visibility = Visibility.Collapsed;
        this.Closing += APIKeyVerificationWindow_Closing;
    }

    private async void VerifyButton_Click(object sender, RoutedEventArgs e)
    {
        StatusLabel.Visibility = Visibility.Visible;
        string apiKey = InputTextBox.Text;

        StatusLabel.Content = "Verifying API Key...";

        try
        {
            // Simulate an async API key verification call
            bool isValid = await _viewModel.Verify(apiKey);

            // Update the status label depending on the result
            if (isValid)
            {
                StatusLabel.Content = "API Key is valid!";
                _viewModel.Store(apiKey);
                DoneButton.IsEnabled = true;
            }
            else
            {
                StatusLabel.Content = "Invalid API Key.";
                DoneButton.IsEnabled = false;
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Content = $"Error: {ex.Message}";
        }
    }

    private void DoneButton_Click(object sender, RoutedEventArgs e)
    {
        if (DoneButton.IsEnabled)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        else
        {
            Close();
        }
    }
    
    private void APIKeyVerificationWindow_Closing(object sender, CancelEventArgs e)
    {
        // Shut down the entire application when the window is closed
        Application.Current.Shutdown();
    }
}