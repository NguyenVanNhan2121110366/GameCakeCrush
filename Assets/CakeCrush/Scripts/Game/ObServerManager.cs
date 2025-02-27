using System;
using System.Collections.Generic;
public class ObServerManager
{
    public static Dictionary<string, Action> listener = new();
    public static void AddObServer(string nameKey, Action action)
    {
        if (!listener.ContainsKey(nameKey))
        {
            listener[nameKey] = action;
        }
        else
        {
            listener[nameKey] += action;
        }
    }

    public static void RemoveObServer(string nameKey, Action action)
    {
        if (!listener.ContainsKey(nameKey))
            return;
        listener[nameKey] -= action;
    }

    public static void Notifine(string nameKey)
    {
        if (!listener.ContainsKey(nameKey))
            return;
        listener[nameKey]?.Invoke();
    }
}
