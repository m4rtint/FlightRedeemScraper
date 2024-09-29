public class CircularBuffer<T>
{
    private readonly T[] buffer;
    private int head;
    private int tail;

    public CircularBuffer(int capacity = 10)
    {
        if (capacity < 1)
            throw new ArgumentException("Capacity must be greater than 0.");
        buffer = new T[capacity];
        head = 0;
        tail = 0;
        Count = 0;
    }

    public int Capacity => buffer.Length;
    public int Count { get; private set; }

    public void Add(T item)
    {
        buffer[tail] = item;
        tail = (tail + 1) % buffer.Length;
        if (Count == buffer.Length)
            head = (head + 1) % buffer.Length; // Move head forward if the buffer is full
        else
            Count++;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        return buffer[(head + index) % buffer.Length];
    }

    public IEnumerable<T> GetAll()
    {
        for (var i = 0; i < Count; i++) yield return Get(i);
    }
}