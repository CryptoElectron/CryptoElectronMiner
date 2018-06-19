using MinerGUI.Bundles;
using MinerGUI.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MinerGUI
{
    class CryptoElectronMaster
    {

        private static int userId = 1;
        private static String rigName = "local";
        private static String hashedRig = "asdzx";
        private static String bundles = "zdxfklsdjk";
        private static String pingedRegions;
        private static String regionsPings;

        private static NetworkStreamWriter streamWriter;
        private static NetworkStreamReader streamReader;
        internal async static void Auth(Form1 form1)
        {
            /*String t = RequestRegions();
            if (t == null)
            {
                //TODO: Cause internal
            }
            else
            {
                //handle region. choose miner
            }
            List<String> regions = t.Split('|').ToList(); ;

            List<MarkedPingReply> taskedPingReplies = Ping(regions);
            List<MarkedPingReply> pingReplies = new List<MarkedPingReply>();

            for(int i = 0; i < taskedPingReplies.Count(); i++)
            {
                try
                {
                    long a = taskedPingReplies[i].pingReply.Result.RoundtripTime;
                    pingReplies.Add(taskedPingReplies[i]);
                } catch(Exception e)
                {

                }
            }
            if(pingReplies.Count()==0)
            {
                //cause internet issues
            }
            pingReplies.Sort((x, y) =>
                x.pingReply.Result.RoundtripTime.CompareTo(y.pingReply.Result.RoundtripTime));
                
            List<String> pingedRegions = new List<string>();
            List<String> regionsPings = new List<string>();
            String finalStr = "";
            for (int i = 0; i < pingReplies.Count(); i++)
            {
                pingedRegions.Add(pingReplies[i].region);
                regionsPings.Add(pingReplies[i].pingReply.Result.RoundtripTime + "");
                finalStr += "Ping to " + pingReplies[i].region + ": " + pingReplies[i].pingReply.Result.RoundtripTime + "ms\n";
            }
            MessageBox.Show(finalStr);
            CryptoElectronMaster.pingedRegions = String.Join(",", pingedRegions);
            CryptoElectronMaster.regionsPings = String.Join(",", regionsPings);

            String returnedRegions = RequestRegion();
            if (returnedRegions == null)
            {
                //TODO: Cause internal
            }
            else
            if (returnedRegions.Equals("waitpls"))
            {
                //TODO: Cause waitpls
            }
            else
            {
                //handle region. choose miner
            }*/
        }


        internal static void Auth(Form1 form1, List<Bundle> bundles)
        {
            EstablishConnectionToMaster("127.0.0.1", 11111, false, bundles);
        }
        private static String RequestRegions()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://local.cryptoelectron.com/master/regions/");

            var postData = "userId=" + userId;
            postData += "&rig=" + rigName;
            postData += "&hashedrig=" + hashedRig;
            postData += "&bundles=" + bundles;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException e)
            {
                return null;
            }

        }

        private static String RequestRegion()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://local.cryptoelectron.com/master/region/");

            var postData = "userId=" + userId;
            postData += "&rig=" + rigName;
            postData += "&hashedrig=" + hashedRig;
            postData += "&regionslist=" + pingedRegions;
            postData += "&latencieslist=" + regionsPings;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                String r = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return r;
            }
            catch (WebException ex)
            {

                return null;

            }


        }

        private static List<MarkedPingReply> Ping(List<String> listOfAddresses)
        {
            Ping pingSender = new Ping();
            List<String> pings = new List<String>();
            List<MarkedPingReply> markedPingReplies = new List<MarkedPingReply>();
            List<Task> t = new List<Task>();
            for (int i = 0; i < listOfAddresses.Count(); i++)
            {
                String[] p = listOfAddresses[i].Split(':');
                Ping ping = new Ping();
                Task<PingReply> tpr = ping.SendPingAsync(p[1], 2500);
                markedPingReplies.Add(new MarkedPingReply()
                {
                    pingReply = tpr,
                    region = p[0]
                });
                t.Add(tpr);
            }
            Task.WhenAll(t);
            return markedPingReplies;
        }
        private class MarkedPingReply
        {
            internal Task<PingReply> pingReply;
            internal String region;
        }




        internal static void TestSsl()
        {

            /*var RSA = new RSACryptoServiceProvider(4096);
            //RSAParameters rsaParameters = RSA.ExportParameters(true);
            var publicKey = RSA.ToXmlString(false);
            File.WriteAllText("./publicxml", publicKey);
            var privateKey = RSA.ToXmlString(true);
            File.WriteAllText("./privatexml", privateKey);*/
            /*File.WriteAllText("./publicmodulus", Convert.ToBase64String(rsaParameters.Modulus));
            File.WriteAllText("./publicexponent", Convert.ToBase64String(rsaParameters.Exponent));
            File.WriteAllText("./publicdelement", Convert.ToBase64String(rsaParameters.D));
            File.WriteAllText("./encryptedstring", Convert.ToBase64String(RSA.Encrypt(Encoding.UTF8.GetBytes("encryptedcontent"), false)));*/
            
            /*string de = "tqVg4IYSIWJFsKdektsAMO/gSKlFPD29CRgFfdgtSr+62eazyjLkxGuYcgZVflH6n9Abi5ATDLpfvEo/4/qL9zR+BRZQXIWsUfHACd7Bt0LjIF48jDsqnzOBKzAg+9I9nNlOcMyTxxv2xaZxI9d7vLkICBdTjcEpZutgV3PbINY=";
            //string encrypt = RSAEncrypt("", "chenhailong"); 
            byte[] encrypt = RSAEncrypt("chenhailong");
            //string name = RSADecrypt(encrypt); 
            string name = RSADecrypt(Convert.FromBase64String(de));
            MessageBox.Show(encrypt.Length+"");
            MessageBox.Show(Convert.ToBase64String(encrypt));
            MessageBox.Show(name);*/
        }
        /// <summary> 
        /// RSA encrypt 
        /// </summary> 
        /// <param name="publickey"></param> 
        /// <param name="content"></param> 
        /// <returns></returns> 
        public static byte[] RSAEncrypt(string content)
        {
            string publickey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            //return Convert.ToBase64String(cipherbytes); 
            return cipherbytes;
        }

        /// <summary> 
        /// RSA decrypt 
        /// </summary> 
        /// <param name="privatekey"></param> 
        /// <param name="content"></param> 
        /// <returns></returns> 
        public static string RSADecrypt(byte[] content)
        {
            string privatekey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent><P>/hf2dnK7rNfl3lbqghWcpFdu778hUpIEBixCDL5WiBtpkZdpSw90aERmHJYaW2RGvGRi6zSftLh00KHsPcNUMw==</P><Q>6Cn/jOLrPapDTEp1Fkq+uz++1Do0eeX7HYqi9rY29CqShzCeI7LEYOoSwYuAJ3xA/DuCdQENPSoJ9KFbO4Wsow==</Q><DP>ga1rHIJro8e/yhxjrKYo/nqc5ICQGhrpMNlPkD9n3CjZVPOISkWF7FzUHEzDANeJfkZhcZa21z24aG3rKo5Qnw==</DP><DQ>MNGsCB8rYlMsRZ2ek2pyQwO7h/sZT8y5ilO9wu08Dwnot/7UMiOEQfDWstY3w5XQQHnvC9WFyCfP4h4QBissyw==</DQ><InverseQ>EG02S7SADhH1EVT9DD0Z62Y0uY7gIYvxX/uq+IzKSCwB8M2G7Qv9xgZQaQlLpCaeKbux3Y59hHM+KpamGL19Kg==</InverseQ><D>vmaYHEbPAgOJvaEXQl+t8DQKFT1fudEysTy31LTyXjGu6XiltXXHUuZaa2IPyHgBz0Nd7znwsW/S44iql0Fen1kzKioEL3svANui63O3o5xdDeExVM6zOf1wUUh/oldovPweChyoAdMtUzgvCbJk1sYDJf++Nr0FeNW1RB1XG30=</D></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(content, false);

            return Encoding.UTF8.GetString(cipherbytes);
        }


    
    /*RSACryptoServiceProvider rSA = new RSACryptoServiceProvider(4096);
    var privateKey = rSA.ToXmlString(true);
    var publicKey = rSA.ToXmlString(false);
    *
    //new RSACryptoServiceProvider(4096);
    
    var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            byte[] publicKeyBytes = ReadToEnd(currentAssembly.GetManifestResourceStream("MinerGUI.Resources.RSA.pub"));

            var RSAKeyInfo = new RSAParameters();
            var RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Encoding.UTF8.GetString(publicKeyBytes));
            byte[] encrypted = RSA.Encrypt(Encoding.UTF8.GetBytes("encrypted penis"), false);

            File.WriteAllBytes("./enc", encrypted);
            /*
            using (var stream = assembly.GetManifestResourceStream("MinerGUI.Resources.RSA.pub"))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }*/
        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        
        private static void EstablishConnectionToMaster(String ip, int port, bool tls, List<Bundle> bundles)
        {
            int protocolVersion = 1;
            TcpClient tcpClient = new TcpClient(ip, port);
            streamWriter = new RSANetworkStreamWriter(tcpClient.GetStream());
            streamReader = new RSANetworkStreamReader(tcpClient.GetStream());
            ((RSANetworkStreamWriter)streamWriter).RSASubmit();
            /*if (!tls)
            {
                streamWriter = new NetworkStreamWriter(tcpClient.GetStream());
                streamReader = new NetworkStreamReader(tcpClient.GetStream());
            }*/
            streamWriter.Write(BitConverter.GetBytes(protocolVersion));
            streamWriter.Write(Encoding.UTF8.GetBytes("cryptoelectronpr@gmail.com"));
            streamWriter.Write(Encoding.UTF8.GetBytes("1231231"));
            streamWriter.Write(Encoding.UTF8.GetBytes("rig1"));
            streamWriter.Write(Encoding.UTF8.GetBytes("hashed1"));
            List<String> bundleNames = new List<string>();
            foreach(Bundle bundle in bundles) {
                bundleNames.Add(bundle.Name);
            }
            streamWriter.Write(Encoding.UTF8.GetBytes(String.Join(",",bundleNames)));
            streamWriter.Flush();
        }
    }
}