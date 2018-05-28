using MinerGUI.Gui.Bundles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinerGUI.Bundles
{
    class Bundle
    {
        public String Name { get; }
        public Algo Algo;
        public Double Hashrate;
        public Double Estimates;
        private Boolean Mining;
        public Boolean ToRedraw;
        public int Type = 0;
        public Bundle(String name, Algo algo, Double hashrate, Double est, int type)
        {
            this.Name = name;
            this.Mining = true;
            this.Algo = algo;
            this.Hashrate = hashrate;
            this.Estimates = est;
            this.Type = type;
        }
        public bool IsMining()
        {
            return this.Mining;
        }
        public Double GetHashrate()
        {
            if(Algo.HashrateSizer.Equals("h/s"))
            {
                return (int)Hashrate;
            } else
            {
                return Hashrate;
            }
        }
        private static String[] NvidiaCards = new String[] { "1050", "1050 Ti", "1060", "1070", "1070 Ti", "1080", "1080 Ti" };
        private static String[] AMDCards = new String[] { "470", "480", "570", "580", "Vega 56", "Vega 64"};
        private static Algo[] algos = new Algo[] { new Algo("Equihash", "h/s"), new Algo("CryptoNight", "h/s"), new Algo("Ethash", "Mh/s"), new Algo("Neoscrypt", "kh/s") };
        public static Bundle getRandomBundle(Random r)
        {
            int count = r.Next(6)+1;
            String finalName = "";
            if(count!=1)
            {
                finalName += count + "x ";
            }
            bool cardsType = r.Next(2) == 1;
            string[] cards = cardsType ? AMDCards : NvidiaCards;
            finalName += (cardsType ? "Radeon RX" : "GeForce GTX ") + " " + cards[r.Next(cards.Count())];
            return new Bundle(finalName, algos[r.Next(algos.Count())], r.NextDouble() * 999, r.NextDouble()/100f, 1);
        }

        internal void StopMining()
        {
            if(this.Mining)
            {
                this.ToRedraw = true;
            }
            this.Mining = false;

        }

        internal void StartMining()
        {
            if (!this.Mining)
            {
                this.ToRedraw = true;
            }
            this.Mining = true;
        }

        internal void Redrawed()
        {
            this.ToRedraw = false;
        }
    }
}
