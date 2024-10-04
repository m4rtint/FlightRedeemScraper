using System.Windows;
using CathayScraperApp.Assets.Domain.UseCases;

namespace CathayScraperApp.Assets.Presentation;

public partial class APIKeyVerificationWindow : Window
{
    private readonly APIKeyVerificationViewModel _viewModel;
    public APIKeyVerificationWindow()
    {
        InitializeComponent();
        _viewModel = new APIKeyVerificationViewModel(
            new VerifyAPIKeyUseCase(
                new MailRepository(
                    new MailAPI())));
    }

    private async void VerifyButton_Click(object sender, RoutedEventArgs e)
    {
        // Get the API key from the input field
        string apiKey = InputTextBox.Text;

        // Update the label to indicate that verification is in progress
        StatusLabel.Content = "Verifying API Key...";

        try
        {
            // Simulate an async API key verification call
            bool isValid = await _viewModel.Verify(apiKey);

            // Update the status label depending on the result
            if (isValid)
            {
                StatusLabel.Content = "API Key is valid!";
            }
            else
            {
                StatusLabel.Content = "Invalid API Key.";
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during the async task
            StatusLabel.Content = $"Error: {ex.Message}";
        }
    }

    private void DoneButton_Click(object sender, RoutedEventArgs e)
    {
        // Close the window when Done is clicked
        this.Close();
    }
}