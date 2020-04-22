using System;
using System.Net;

namespace YLSocket
{
    public class ConnectEvent : EventArgs
    {
        public EndPoint EndPoint { get; private set; }

        public ConnectEvent(EndPoint endPoint)
        {
            this.EndPoint = endPoint;
        }
    }
}