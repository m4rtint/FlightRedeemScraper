using System.Timers;
using Timer = System.Timers.Timer;

public class Polling
{
    private Timer _timer;
    private readonly double _intervalInMilliseconds;
    private bool _isPolling;

    // Constructor takes polling interval in seconds
    public Polling(double intervalInSeconds)
    {
        _intervalInMilliseconds = intervalInSeconds * 1000; // Convert seconds to milliseconds
        _isPolling = false;
    }

    // Start polling and execute the provided async action
    public async void StartPolling(Func<Task> asyncAction)
    {
        if (_isPolling)
        {
            return;
        }

        _timer = new Timer(_intervalInMilliseconds);
        _timer.Elapsed += async (sender, e) =>
        {
            _timer.Stop(); // Stop timer while task is running to avoid overlapping executions.
            await asyncAction(); // Await the async task.
            _timer.Start(); // Restart the timer after task is done.
        };

        _timer.AutoReset = false; // We manually restart the timer after each task is done.
        _timer.Enabled = true;
        _isPolling = true;

        // Run the asyncAction immediately on start
        await asyncAction();
        _timer.Start();
    }

    // Stop polling
    public void StopPolling()
    {
        if (!_isPolling)
        {
            return;
        }

        _timer.Stop();
        _timer.Dispose();
        _isPolling = false;
    }

    // Destructor to ensure the timer is disposed of
    ~Polling()
    {
        StopPolling();
    }
}