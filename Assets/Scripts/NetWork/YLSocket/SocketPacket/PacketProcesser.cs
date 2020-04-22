using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SocketPacket {

    public delegate void MessageListener(string obj);

    public class PacketProcesser {
        /// <summary>
        /// 保存消息和对应的消息处理对象
        /// </summary>
        private Dictionary<int, MessageListener> eventListerners = null;
        Dictionary<int, MessageListener> needChange = new Dictionary<int, MessageListener>();
        public PacketProcesser() {
            eventListerners =new Dictionary<int, MessageListener> ();
        }

        public void addEventListener(int MdmNum,  MessageListener listener)
        {
            if (!eventListerners.ContainsKey(MdmNum))
            {
                eventListerners[MdmNum] = listener;
            }
        }

        public void removeEventListener(int MdmNum, MessageListener listener)
        {

            Dictionary<int, MessageListener> DirData = new Dictionary<int, MessageListener>();
           
            foreach (var item in eventListerners)
            {
                if (item.Key == MdmNum)
                {
                    DirData.Add(item.Key, listener); 
                }
            } 
            foreach (var item in DirData)
            {
                if (item.Key == MdmNum)
                {
                    eventListerners.Remove(item.Key);
                } 
            } 
            DirData.Clear();
        }

        public void dispatchEvent(int MdmNum, string str)
        {
            foreach (var item in eventListerners)
            {
                if (item.Key == MdmNum)
                {
                    needChange.Add(item.Key, item.Value);
                    break;
                }
                
            }
            foreach (var item in needChange)
            {
                if (item.Key == MdmNum)
                {
                    if (str != null)
                    {
                        eventListerners[item.Key](str);
                        break;
                    }
                }
            }

            needChange.Clear();
        }

        public bool hasEventListener(int MdmNum)
        {
            return eventListerners.ContainsKey(MdmNum);
        }
    }
}


