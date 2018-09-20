using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MessageHandler(object data);
public class MessageComponent : MonoBehaviour
{
    static Dictionary<int, Dictionary<string, MessageHandler>> TargetsEventsAndHandlers = new Dictionary<int, Dictionary<string, MessageHandler>>();
    static Dictionary<string, MessageHandler> EventsAndHandlers = new Dictionary<string, MessageHandler>();

    Dictionary<int, Dictionary<string, MessageHandler>> TargetsEventsAndHandlers_Self = new Dictionary<int, Dictionary<string, MessageHandler>>();
    Dictionary<string, MessageHandler> EventsAndHandlers_Self = new Dictionary<string, MessageHandler>();

    public void Monitor(GameObject target, string eventName, MessageHandler handler)
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }

        var insID = target.GetInstanceID();

        Record(TargetsEventsAndHandlers, insID, eventName, handler);
        Record(TargetsEventsAndHandlers_Self, insID, eventName, handler);
    }
    public void MonitorAll(string eventName, MessageHandler handler)
    {
        Record(EventsAndHandlers, eventName, handler);
        Record(EventsAndHandlers_Self, eventName, handler);
    }

    public void StopMonitor(GameObject target, string eventName, MessageHandler handler)
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }

        var insID = target.GetInstanceID();

        RemoveHelper(TargetsEventsAndHandlers, insID, eventName, handler);
        RemoveHelper(TargetsEventsAndHandlers_Self, insID, eventName, handler);
    }
    public void StopMonitorAll(string eventName, MessageHandler handler)
    {
        RemoveHelper(EventsAndHandlers, eventName, handler);
        RemoveHelper(EventsAndHandlers_Self, eventName, handler);
    }

    public void TriggerSelf(string eventName, object data)
    {
        Trigger(gameObject, eventName, data);
    }

    static void Trigger(GameObject target, string eventName, object data)
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }

        if (EventsAndHandlers.ContainsKey(eventName))
        {
            if (EventsAndHandlers[eventName] != null)
                EventsAndHandlers[eventName](data);
        }

        var insID = target.GetInstanceID();
        if (TargetsEventsAndHandlers.ContainsKey(insID))
        {
            if (TargetsEventsAndHandlers[insID].ContainsKey(eventName))
                if (TargetsEventsAndHandlers[insID][eventName] != null)
                    TargetsEventsAndHandlers[insID][eventName](data);
        }
    }

    void OnDestroy()
    {
        var insID = gameObject.GetInstanceID();
        if (TargetsEventsAndHandlers.ContainsKey(insID))
            TargetsEventsAndHandlers.Remove(insID);

        foreach (var item in TargetsEventsAndHandlers_Self)
        {
            foreach (var itemc in item.Value)
                RemoveHelper(TargetsEventsAndHandlers, item.Key, itemc.Key, itemc.Value);
        }

        foreach (var item in EventsAndHandlers_Self)
            RemoveHelper(EventsAndHandlers, item.Key, item.Value);

        TargetsEventsAndHandlers_Self.Clear();
        EventsAndHandlers_Self.Clear();
    }

    #region  helper
    static void Record(Dictionary<int, Dictionary<string, MessageHandler>> dic, int insID, string eventName, MessageHandler handler)
    {

        if (dic.ContainsKey(insID))
        {
            if (dic[insID].ContainsKey(eventName))
                dic[insID][eventName] += handler;
            else
                dic[insID].Add(eventName, handler);
        }
        else
        {
            dic.Add(insID, new Dictionary<string, MessageHandler>());
            dic[insID].Add(eventName, handler);
        }
    }
    static void Record(Dictionary<string, MessageHandler> dic, string eventName, MessageHandler handler)
    {
        if (dic.ContainsKey(eventName))
        {
            dic[eventName] += handler;
        }
        else
            dic.Add(eventName, handler);
    }

    static void RemoveHelper(Dictionary<int, Dictionary<string, MessageHandler>> dic, int insID, string eventName, MessageHandler handler)
    {

        if (dic.ContainsKey(insID))
        {
            if (dic[insID].ContainsKey(eventName))
            {
                dic[insID][eventName] -= handler;
                if (dic[insID][eventName] == null)
                {
                    dic[insID].Remove(eventName);
                    if (dic[insID].Count == 0)
                        dic.Remove(insID);
                }
            }
        }
    }

    static void RemoveHelper(Dictionary<string, MessageHandler> dic, string eventName, MessageHandler handler)
    {
        if (dic.ContainsKey(eventName))
        {
            dic[eventName] -= handler;
            if (dic[eventName] == null)
                dic.Remove(eventName);
        }
    }

    #endregion

}
