using System.Collections;

namespace XorFilesMerger
{
    static class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var file1 = @"c:\Users\a\Source\Repos\XorFilesMerger\file1.wmv";
            var file2 = @"c:\Users\a\Source\Repos\XorFilesMerger\file2.wmv";
            var file3 = @"c:\Users\a\Source\Repos\XorFilesMerger\file3.wmv";

            XorFilesMerger.merge(file1, file2, file3);
            return;
#endif
            foreach (var arg in args) System.Console.WriteLine(arg);
            XorFilesMerger.merge(args[0], args[1], args[2], args.Length == 3);
        }

        public static byte[] ToByteArray(this BitArray bits)
        {
            const int BYTE = 8;
            int length = (bits.Count / BYTE) + ((bits.Count % BYTE == 0) ? 0 : 1);
            var bytes = new byte[length];

            for (int i = 0; i < bits.Length; i++)
            {
                int bitIndex = i % BYTE;
                int byteIndex = i / BYTE;

                int mask = (bits[i] ? 1 : 0) << bitIndex;
                bytes[byteIndex] |= (byte)mask;

            }//for

            return bytes;

        }//ToByteArray
    }
}
