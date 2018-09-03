using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CZM
{
    public delegate void MessageDelegate(object messageData);
    public class MessageCenter
    {
        static Dictionary<int, Dictionary<string, MessageDelegate>> TargtsEventsAndHandlers =
                new Dictionary<int, Dictionary<string, MessageDelegate>>();
        static Dictionary<string, MessageDelegate> MonitorAllEvents =
                new Dictionary<string, MessageDelegate>();
        static Dictionary<string, MessageDelegate> UnownedEventsAndHandlers =
                new Dictionary<string, MessageDelegate>();

        static void Monitor(GameObject target, string eventName, MessageDelegate handler)
        {
            if (target == null)
                return;

            int id = target.GetInstanceID();
            if (!TargtsEventsAndHandlers.ContainsKey(id))
                TargtsEventsAndHandlers.Add(id, new Dictionary<string, MessageDelegate>());

            var eventAndHandlers = TargtsEventsAndHandlers[id];

            if (!eventAndHandlers.ContainsKey(eventName))
                eventAndHandlers.Add(eventName, new MessageDelegate(handler));
            else
                eventAndHandlers[eventName] += handler;
        }

        static void StopMonitor(GameObject target, string eventName, MessageDelegate handler)
        {
            if (target == null)
                return;

            int id = target.GetInstanceID();
            if (!TargtsEventsAndHandlers.ContainsKey(id))
                return;

            var eventAndHandlers = TargtsEventsAndHandlers[id];

            if (!eventAndHandlers.ContainsKey(eventName))
                return;
            else
            {
                eventAndHandlers[eventName] -= handler;

                if (eventAndHandlers[eventName] == null)
                    TargtsEventsAndHandlers[id].Remove(eventName);
            }

            if (TargtsEventsAndHandlers[id].Count == 0)
                TargtsEventsAndHandlers.Remove(id);

        }

        static void MonitorAll(string eventName, MessageDelegate handler)
        {
            if (!MonitorAllEvents.ContainsKey(eventName))
                MonitorAllEvents.Add(eventName, new MessageDelegate(handler));
            else
                MonitorAllEvents[eventName] += handler;
        }

        static void StopMonitorAll(string eventName, MessageDelegate handler)
        {
            if (!MonitorAllEvents.ContainsKey(eventName))
                return;
            else
            {
                MonitorAllEvents[eventName] -= handler;
                if (MonitorAllEvents[eventName] == null)
                    MonitorAllEvents.Remove(eventName);
            }
        }

        static void MonitorUnownedEvent(string eventName, MessageDelegate handler)
        {
            if (!UnownedEventsAndHandlers.ContainsKey(eventName))
                UnownedEventsAndHandlers.Add(eventName, new MessageDelegate(handler));
            else
                UnownedEventsAndHandlers[eventName] += handler;
        }

        static void StopMonitorUnownedEvent(string eventName, MessageDelegate handler)
        {
            if (!UnownedEventsAndHandlers.ContainsKey(eventName))
                return;
            else
            {
                UnownedEventsAndHandlers[eventName] -= handler;
                if (UnownedEventsAndHandlers[eventName] == null)
                    UnownedEventsAndHandlers.Remove(eventName);
            }
        }

        //immedate, next frame, some seconds
        static void TriggerEvent(GameObject target, string eventName, object data)
        {
            if (target == null)
                return;

            TriggerEvent(target.GetInstanceID(), eventName, data);
        }

        static void TriggerEvent(int instanceID, string eventName, object data)
        {
            if (!TargtsEventsAndHandlers.ContainsKey(instanceID))
                return;

            var eventAndHandlers = TargtsEventsAndHandlers[instanceID];

            if (!eventAndHandlers.ContainsKey(eventName))
                return;
            else
                eventAndHandlers[eventName](data);

            if (!MonitorAllEvents.ContainsKey(eventName))
                return;
            else
                MonitorAllEvents[eventName](data);

        }
        
        static void TriggerUnownedEvent(string eventName, object data)
        {
            if (!UnownedEventsAndHandlers.ContainsKey(eventName))
                return;
            else
                UnownedEventsAndHandlers[eventName](data);
        }

        public static void Update()
        {

        }
    }
}

