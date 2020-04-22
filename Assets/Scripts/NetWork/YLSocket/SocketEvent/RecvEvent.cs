using System;

namespace YLSocket
{
    public class RecvEvent : EventArgs
    {
        private byte[] _Message = null;
        public byte[] Message
        {
            get
            {
                return _Message;
            }
        }

        private int _BytesTransferred = 0;
        public int BytesTransferred
        {
            get
            {
                return _BytesTransferred;
            }
            set
            {
                _BytesTransferred = value;
            }
        }

        public RecvEvent(byte[] data, int dataSize)
        {
            _Message = data;
            _BytesTransferred = dataSize;
        }
    }
}
