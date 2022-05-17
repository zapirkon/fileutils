using System.IO;

namespace XorFilesMerger
{
    static class XorFilesMerger
    {
        /// <summary>
        /// merge file1 and file2 into file3, where bytes == 0 should be the ones where the files differ
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="file3"></param>
        public static void merge(string file1, string file2, string file3, bool firstFilePriority = true)
        {
            //no checks, just go for it

            var stream1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
            var stream2 = new FileStream(file2, FileMode.Open, FileAccess.Read);
            var stream3 = new FileStream(file3, FileMode.Create);

            var binary1 = new BinaryReader(stream1);
            var binary2 = new BinaryReader(stream2);
            var binary3 = new BinaryWriter(stream3);

            var bufferLength = 4 << 10; // 4K
            var buffer1 = new byte[bufferLength];
            var buffer2 = new byte[bufferLength];

            var read1 = 0;
            var read2 = 0;

            while (true)
            {
                read1 = binary1.Read(buffer1, 0, bufferLength);
                read2 = binary2.Read(buffer2, 0, bufferLength);

                if (read1 != read2)
                {
                    System.Diagnostics.Debugger.Break();
                }

                bool badBlock = false;
                for (int i = 0; i < bufferLength; i++)
                {
                    if (!badBlock && buffer1[i] != buffer2[i])
                    {
                        if (buffer1[i] > 0 && buffer2[i] > 0)
                        {
                            if (buffer1[i] != buffer2[i])
                            {
                                badBlock = true;
                                System.Console.WriteLine($"different bytes in block at pos {stream1.Position + i}");
#if DEBUG
                                System.Diagnostics.Debugger.Break();
#endif
                                continue;
                            }
                        }

                        if (buffer1[i] == 0)
                        {
                            buffer1[i] = buffer2[i];
                        }

                        if (buffer2[i] == 0)
                        {
                            buffer2[i] = buffer1[i];
                        }
                    }
                }

                if (firstFilePriority)
                {
                    binary3.Write(buffer1, 0, read1);
                }
                else
                {
                    binary3.Write(buffer2, 0, read2);
                }

                if (read1 < bufferLength)
                {
                    break;
                }
            }

            binary1.Close();
            stream1.Close();

            binary2.Close();
            stream2.Close();

            binary3.Flush();
            stream3.Flush();
            binary3.Close();
            stream3.Close();
        }
    }
}
