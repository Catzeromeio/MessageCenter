using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CZM;

public class MessageInfo
{
    public GameObject Target;
    public string EventName;
    public MessageDelegate Handler;

    public MessageInfo(GameObject target, string eventName, MessageDelegate handler)
    {
        Target = target;
        EventName = eventName;
        Handler = handler;
    }
}

public class MCMonoBehaviour : MonoBehaviour
{
    List<MessageInfo> ListMessageInfos = new List<MessageInfo>();
    List<MessageInfo> ListMessageInfosAll = new List<MessageInfo>();
    List<MessageInfo> ListMessageInfosUnowned = new List<MessageInfo>();
    void Monitor(GameObject target, string eventName, MessageDelegate handler)
    {
        if (MessageCenter.Monitor(target, eventName, handler))
            ListMessageInfos.Add(new MessageInfo(target, eventName, handler));
    }

    void StopMonitor(GameObject target, string eventName, MessageDelegate handler)
    {
        MessageCenter.StopMonitor(target, eventName, handler);
        ListMessageInfos.RemoveAll(c => c.Target == target && c.EventName == eventName && c.Handler.Equals(handler));
    }

    void MonitorAll(string eventName, MessageDelegate handler)
    {
        MessageCenter.MonitorAll(eventName, handler);
        ListMessageInfosAll.Add(new MessageInfo(null, eventName, handler));
    }

    void StopMonitorAll(string eventName, MessageDelegate handler)
    {
        MessageCenter.StopMonitorAll(eventName, handler);
        ListMessageInfosAll.RemoveAll(c => c.EventName == eventName && c.Handler.Equals(handler));
    }

    void MonitorUnownedEvent(string eventName, MessageDelegate handler)
    {
        MessageCenter.MonitorUnownedEvent(eventName, handler);
        ListMessageInfosUnowned.Add(new MessageInfo(null, eventName, handler));
    }

    void StopMonitorUnownedEvent(string eventName, MessageDelegate handler)
    {
        MessageCenter.StopMonitorUnownedEvent(eventName, handler);
        ListMessageInfosUnowned.RemoveAll(c => c.EventName == eventName && c.Handler.Equals(handler));
    }

    //immedate, next frame, some seconds
    static void TriggerEvent(GameObject target, string eventName, object data)
    {
        MessageCenter.TriggerEvent(target, eventName, data);
    }
    static void TriggerUnownedEvent(string eventName, object data)
    {
        MessageCenter.TriggerUnownedEvent(eventName, data);
    }

    void OnDestroy()
    {
        foreach (var item in ListMessageInfos)
            MessageCenter.StopMonitor(item.Target, item.EventName, item.Handler);
        foreach (var item in ListMessageInfosAll)
            MessageCenter.StopMonitorAll(item.EventName, item.Handler);
        foreach (var item in ListMessageInfosUnowned)
            MessageCenter.StopMonitorUnownedEvent(item.EventName, item.Handler);

        ListMessageInfos.Clear();
        ListMessageInfosAll.Clear();
        ListMessageInfosUnowned.Clear();
    }
}
