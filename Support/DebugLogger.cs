﻿using System.Diagnostics;

public static class DebugLogger
{
    private static readonly CircularBuffer<string> circularBuffer = new(Int32.MaxValue / 2);

    public static event Action<string[]> OnLogChanged;

    public static void Log(string message)
    {
        //Current Date Time - hh:mm:ss dd/MM/yyyy
        var log = DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy") + " - " + message;
        Trace.WriteLine(log);
        Console.WriteLine(log);
        circularBuffer.Add(log);
        OnLogChanged?.Invoke(circularBuffer.GetAll().ToArray());
    }
}