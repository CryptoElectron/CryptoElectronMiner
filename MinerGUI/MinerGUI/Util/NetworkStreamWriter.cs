using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinerGUI.Util
{
    class NetworkStreamWriter : CryptoElectronNetworkStream
    {
        
        private NetworkStream networkStream;
        public NetworkStreamWriter(NetworkStream networkStream)
        {
            this.networkStream = networkStream;
        }

        public virtual void Write(byte[] data)
        {
            data = MergeByteArrays(data, EOP);
            this.networkStream.Write(data, 0, data.Length);
        }

        public virtual void Flush()
        {
            this.networkStream.Flush();
        }

        public virtual void WriteAndFlush(byte[] data)
        {
            this.Write(data);
            this.Flush();
        }
    }
}
