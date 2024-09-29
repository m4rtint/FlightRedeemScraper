namespace CathayDomain;

public class SimpleStorage
{
    private readonly string _prefix;
    private readonly Dictionary<string, object> _storage = new();

    public SimpleStorage(string prefix = "default-")
    {
        _prefix = prefix;
    }

    public void SetInt(string key, int value)
    {
        _storage[_prefix + key] = value;
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        if (_storage.TryGetValue(_prefix + key, out var value)) return (int)value;
        return defaultValue;
    }

    public void SetFloat(string key, float value)
    {
        _storage[_prefix + key] = value;
    }

    public float GetFloat(string key, float defaultValue = 0.0f)
    {
        if (_storage.TryGetValue(_prefix + key, out var value)) return (float)value;
        return defaultValue;
    }
}