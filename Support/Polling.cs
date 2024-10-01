using System.Timers;
using Timer = System.Timers.Timer;

public class Polling
{
    private Timer _timer;
    private readonly double _intervalInMilliseconds;
    private bool _isPolling;

    // Constructor takes polling interval in minutes
    public Polling(double intervalInMinutes)
    {
        _intervalInMilliseconds = intervalInMinutes * 60 * 1000; // Convert minutes to milliseconds
        _isPolling = false;
    }

    // Start polling and execute the provided async action
    public void StartPolling(Func<Task> asyncAction)
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