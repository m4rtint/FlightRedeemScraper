using System.Windows;

namespace CathayScraperApp.Assets.Presentation;

public partial class APIKeyVerificationWindow : Window
{
    public APIKeyVerificationWindow()
    {
        InitializeComponent();
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
            bool isValid = await VerifyApiKeyAsync(apiKey);

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

    // Simulate an async API key verification call
    private async Task<bool> VerifyApiKeyAsync(string apiKey)
    {
        // Simulate a delay to mimic a real async operation (like a network request)
        await Task.Delay(2000);

        // Simulate checking if the API key is valid
        // You can replace this with an actual call to your API
        return !string.IsNullOrWhiteSpace(apiKey) && apiKey == "correct-api-key";
    }
}