using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace YLSocket
{
    public class SocketClient
    {
        /// <summary>
        /// 最大收包缓存大小
        /// </summary>
        public static int BUFFER_SIZE = 65000;
        public Socket scoket; 
        public event EventHandler<RecvEvent> RecvEventHandler;
        public event EventHandler<ConnectEvent> ConnectHandler;
        public event EventHandler CloseHandler;
        public event EventHandler ConnectErrorHandler;
        public event EventHandler ReconnectHandler;

        /// <summary>
        /// 新建Socket连接
        /// </summary>
        /// <param name="serverAddress">server地址</param>
        /// <param name="port">端口号</param>
        public void CreateConnection(string serverAddress, int port)
        {
            if (scoket != null && scoket.Connected)
            {
                Debug.Log("tcp服务器已链接，请勿再次链接");
                return;
            }
            Debug.Log("链接tcp服务器 ：" + GameManager.GetInstance.CurHostIP + "||" + 7772);
            scoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.scoket.NoDelay = true;
            IPEndPoint end_point = new IPEndPoint(IPAddress.Parse(serverAddress), port);
            this.scoket.BeginConnect(end_point, new AsyncCallback(this.Connected), this.scoket);
        }
       
        /// <summary>
        /// 销毁socket连接
        /// </summary>
        public void DisConnect()
        {
            if (scoket != null)
            {
                try
                {
                    scoket.Shutdown(SocketShutdown.Both);
                    scoket.Close();
                    scoket = null;
                }
                catch
                {
                    scoket = null;
                }
            }
        }

        /// <summary>
        /// 发送消息给server
        /// </summary>
        /// <param name="data"></param>
        public void SendMessageToServer(byte[] data)
        {
            if (this.scoket == null)
            {
                if (this.ReconnectHandler != null)
                {
                    this.ReconnectHandler(this, new EventArgs());
                }
                return;
            }
            if (!this.scoket.Connected)
            {
                if (this.ReconnectHandler != null)
                {
                    this.ReconnectHandler(this, new EventArgs());
                }
                return;
            }
            this.scoket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.SendMessageToServerComplete), this.scoket);
        }

        /// <summary>
        /// 信息发送到server完成回调
        /// </summary>
        /// <param name="iar"></param>
        private void SendMessageToServerComplete(IAsyncResult iar)
        {
            //this.scoket.EndSend(iar);
            //Debug.Log("消息发送完成");
        }

        /// <summary>
        /// socket连接创建成功回调
        /// </summary>
        /// <param name="iar"></param>
        private void Connected(IAsyncResult iar)
        {
            try
            {
                this.scoket.EndConnect(iar);
                if (this.ConnectHandler != null)
                {
                    this.ConnectHandler(this, new ConnectEvent(scoket.RemoteEndPoint));
                }
                byte[] array = new byte[BUFFER_SIZE];
                this.scoket.BeginReceive(array, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(this.KeepConnect), array);
            }
            catch (SocketException e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.SocketErrorCode);
                if (this.ConnectErrorHandler != null)
                {
                    this.ConnectErrorHandler(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 保持socket连接, 一直监听server发过来的消息
        /// </summary>
        /// <param name="iar"></param>
        private void KeepConnect(IAsyncResult iar)
        {
            try
            {
                int num = this.scoket.EndReceive(iar);
                if (num > 0)
                {
                    byte[] array = (byte[])iar.AsyncState;
                    if (this.RecvEventHandler != null && array != null)
                    {
                        this.RecvEventHandler(this, new RecvEvent(array, num));
                    }

                    array = new byte[BUFFER_SIZE];
                    this.scoket.BeginReceive(array, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(this.KeepConnect), array);
                }
                else
                {
                    if (this.CloseHandler != null)
                    {
                        this.CloseHandler(this, new EventArgs());
                    }
                }
            }
            catch (SocketException)
            {

            }
        }
    }
}
