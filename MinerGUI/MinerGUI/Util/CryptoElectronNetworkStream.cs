using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinerGUI.Util
{
    class CryptoElectronNetworkStream
    {

        internal static SerialQueue writingQueue = new SerialQueue();
        internal static SerialQueue readingQueue = new SerialQueue();

        public long readingDelay = 10;

        public static byte[] EOP { get => Encoding.UTF8.GetBytes("CEEOP%!@#"); }
        public static byte[] MergeByteArrays(byte[] data, byte[] data1)
        {
            byte[] rv = new byte[data.Length + data1.Length];
            System.Buffer.BlockCopy(data, 0, rv, 0, data.Length);
            System.Buffer.BlockCopy(data1, 0, rv, data.Length, data1.Length);
            return rv;
        }

        internal String[] toStrings(byte[] data)
        {
            return data.Select(byteValue => byteValue.ToString()).ToArray();
        }
    }


    static class ArrayExtensions
    {

        public static IEnumerable<int> StartingIndex(this byte[] x, byte[] y)
        {
            IEnumerable<int> index = Enumerable.Range(0, x.Length - y.Length + 1);
            for (int i = 0; i < y.Length; i++)
            {
                index = index.Where(n => x[n + i] == y[i]).ToArray();
            }
            return index;
        }

    }

    public static class Extensions
    {
        /// <summary>
        /// Slices the specified collection like Python does
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me">the collection</param>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        /// <returns></returns>
        /// <example><code>
        /// var numbers=new[]{1,2,3,4,5};
        /// var eg = numbers.Slice(-3,-1);
        /// foreach (var i in eg)
        ///	{
        ///		Console.Write("{0}, ", i);
        ///	}
        ///	Console.Writeline();	// Output: 3, 4,</code></example>
        /// <exception cref="System.ArgumentException">starting point must be less than or equal to ending point;start</exception>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> me, int? start = null, int? end = null)
        {
            if (start.HasValue && end.HasValue && start.Value > end.Value)
            {
                throw new ArgumentException("starting point must be less than or equal to ending point", "start");
            }

            if (start.HasValue && start < 0)
            {
                start = me.Count() + start;
            }

            if (end.HasValue && end < 0)
            {
                end = me.Count() + end;
            }

            if (!start.HasValue)
            {
                start = 0;
            }

            if (!end.HasValue)
            {
                end = me.Count();
            }

            return me.Skip(start.Value).Take(end.Value - start.Value);
        }

    }
}
