using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MinerGUI.Util
{
    class RSANetworkStreamReader : NetworkStreamReader
    {

        private RSACryptoServiceProvider localRSAKey;
        private byte[] Modulus;
        private byte[] Exponent;
        private NetworkStream networkStream;

        public RSANetworkStreamReader(NetworkStream networkStream) : base(networkStream)
        {
        }

        private void InitializeLocalRSAKey()
        {
            if (this.localRSAKey == null)
            {
                this.localRSAKey = new RSACryptoServiceProvider(4096);
                var ownRSAParams = this.localRSAKey.ExportParameters(false);
                this.Modulus = ownRSAParams.Modulus;
                this.Exponent = ownRSAParams.Exponent;
            }
        }
        /*public override async Task<byte[]> ReadPacketAsync()
        {
            Task<byte[]> t = await base.readingQueue.Enqueue(Read);
            return t.Result;
        }
        private async Task<byte[]> Read()
        {
            byte[] size = new byte[4];
            await base.ReadAsync(size);
            byte[] buffer = new byte[BitConverter.ToInt32(size, 0)];
            await base.ReadAsync(buffer);
            return this.DecryptBytes(buffer);
        }

        private byte[] DecryptBytes(byte[] data)
        {
            return this.localRSAKey.Decrypt(data, false);
        }*/

    }
}
