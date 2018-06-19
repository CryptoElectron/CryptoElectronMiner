using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinerGUI.Util
{
    class RSANetworkStreamWriter : NetworkStreamWriter
    {
        private RSACryptoServiceProvider remotePublicRSAKey;

        public RSANetworkStreamWriter(NetworkStream networkStream) : base(networkStream)
        {
            this.InitializeRemotePublicKey();
        }

        private void InitializeRemotePublicKey()
        {
            if (this.remotePublicRSAKey==null)
            {
                var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                StreamReader streamReader = new StreamReader(currentAssembly.GetManifestResourceStream("MinerGUI.Resources.RSA.pub"));
                this.remotePublicRSAKey = new RSACryptoServiceProvider();
                this.remotePublicRSAKey.FromXmlString(streamReader.ReadToEnd());
            }
        }
        public override void Write(byte[] data)
        {
            base.Write(EncryptBytes(data));
        }
        private byte[] EncryptBytes(byte[] bytes)
        {
            byte[] bytes1 = this.remotePublicRSAKey.Encrypt(bytes, false);
            //MessageBox.Show(String.Join(",", this.toStrings(bytes1)) + "\n\n" + String.Join(",", this.toStrings(bytes)));
            return bytes1;
        }
        public void RSASubmit()
        {
            RSAParameters rsaParameters = this.remotePublicRSAKey.ExportParameters(false);
            byte[] modulus = rsaParameters.Modulus;
            int sep = modulus.Length / 2;
            byte[] modulusPart1 = Slice(modulus, 0, sep);
            byte[] modulusPart2 = Slice(modulus, sep, modulus.Length - sep);
            this.Write(modulusPart1);
            this.Write(modulusPart2);
            this.Write(rsaParameters.Exponent);
            this.Flush();
        }

        public static byte[] Slice(byte[] source, int index, int length)
        {
            byte[] slice = new byte[length];
            Array.Copy(source, index, slice, 0, length);
            return slice;
        }
    }
}
