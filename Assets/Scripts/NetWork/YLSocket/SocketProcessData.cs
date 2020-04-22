using System;
using YLSocket;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SocketProcessData
{
    private SocketCallBack mCallback;
    
    //新的字节数组
    private List <byte> buffer;
    public SocketProcessData(SocketCallBack Callback)
    {
        this.mCallback = Callback;
    }
    //拿到服务器的消息后处理一下
    public void IncomingData(byte[] data, int actualSize)
    {
        if (actualSize >= SocketClient.BUFFER_SIZE)
        {
            Debug.Log("当前大小 actualSize:" + actualSize);
            throw new Exception("Buffer Overflow!");
        }

        List<byte> a = data.ToList();
        buffer = new List<byte>();
        buffer = a.GetRange(0,actualSize);
        this.mCallback.SendMessage(buffer.ToArray<byte>());

        #region
        ////截取第一个消息后剩余的消息，如果剩余不为空说明黏包了，做递归处理
        //List<byte> b = a.GetRange(actualSize, a.Count - actualSize);
        ////消息的123位为消息长度
        //int bSize = b[3] << 16 | b[2] << 8 | b[1];
        ////判断是否黏包
        //if (bSize != 0)
        //{
        //    IncomingData(b.ToArray<byte>(), b.Count);
        //}
        #endregion
    }
}
