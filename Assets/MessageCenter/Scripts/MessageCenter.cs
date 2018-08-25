using System;
using System.Collections;
using System.Collections.Generic;

namespace CZM
{
    public struct Message
    {
        public object Sender;
        public object Receiver;
        public object Data;
    }

    public delegate void MessageDelegate(Message mes);
    public class MessageCenter
    {
        static void AddListener()
        {
        }

        static void RemoveListener(Message mes)
        {

        }

        static void RemoveAllListener()
        {

        }

        //immedate, next frame, some seconds
        static void SendEvent()
        {

        }

        public static void Update()
        {

        }
    }
}

