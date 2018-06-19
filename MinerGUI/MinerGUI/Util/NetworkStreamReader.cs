using MinerGUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinerGUI.Util
{

    class NetworkStreamReader : CryptoElectronNetworkStream
    {
        List<byte[]> receivedPacketsQueue = new List<byte[]>();
        private NetworkStream networkStream;
        private byte[] alreadyReceived;

        private long lastLaunchTime = 0;

        public NetworkStreamReader(NetworkStream networkStream)
        {
            this.networkStream = networkStream;
            //readingQueue.Enqueue(Read);
        }


        /*public virtual async Task<byte[]> ReadPacketAsync()
        {
            if (receivedPacketsQueue.Count() > 0)
            {
                return receivedPacketsQueue.Slice<byte[]>(0, 1).First<byte[]>();
            } else
            {
                long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                Task<byte[]> t = readingQueue.Enqueue<byte[]>(ReadPacketAsync);
                return t;
            }
        }
        private async void Read()
        {
            byte[] buffer = new byte[1024];
            Task<int> t = this.networkStream.ReadAsync(buffer, 0, buffer.Length);
            await t;
            byte[] received = new byte[t.Result];

            System.Buffer.BlockCopy(buffer, 0, received, 0, t.Result);
            byte[] finalBytes = MergeByteArrays(alreadyReceived, received);
            int lastEOPPosition = 0;
            foreach (int i in finalBytes.StartingIndex(EOP))
            {

            }

            readingQueue.Enqueue(Read);
        }

        internal async Task<byte[]> ReadAsync()
        {
            if(this.networkStream.DataAvailable)
            {
            }
        }*/
    }
}

