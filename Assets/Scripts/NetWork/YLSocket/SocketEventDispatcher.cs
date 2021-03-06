﻿using System;
using System.Collections;
using UnityEngine;

namespace YLSocket
{
    /// <summary>
    /// 收到server发过来的数据，并分发数据
    /// </summary>
    public class SocketEventDispatcher
    {
        public struct SocketData
        {
            /// <summary>
            /// 内容
            /// </summary>
            public byte[] data;
            /// <summary>
            /// 包的实际大小
            /// </summary>
            public int actualSize;
        }

        private SocketProcessData mIProcessData = null;
        private Queue mQueue = new Queue();

        public SocketEventDispatcher(SocketProcessData mIProcessData)
        {
            this.mIProcessData = mIProcessData;
        }

        /// <summary>
        /// 检测是否有数据到达，并下发到IProcessCMD类中
        /// <para>注意 ： </para>
        /// <para>请在MonoBehaviour类Update()函数中使用</para>
        /// </summary>
        public void IncomingData()
        {
            while (this.mQueue.Count > 0)
            {
                SocketData socketdata = (SocketData)this.mQueue.Dequeue();
                this.mIProcessData.IncomingData(socketdata.data, socketdata.actualSize);
            }
        }

        /// <summary>
        /// 当有数据到达时
        /// </summary>
        /// <param name="data"></param>
        /// <param name="actualSize"></param>
        public void AddData(byte[] data, int actualSize)
        {
            SocketData socketdata = new SocketData();
            socketdata.data = data;
            socketdata.actualSize = actualSize;
            mQueue.Enqueue(socketdata);
        }
    }
}
