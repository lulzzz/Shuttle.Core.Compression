using System.IO;
using System.IO.Compression;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Compression
{
    public class GZipCompressionAlgorithm : ICompressionAlgorithm
    {
        public string Name => "GZip";

        public byte[] Compress(byte[] bytes)
        {
            Guard.AgainstNull(bytes, nameof(bytes));

            using (var compressed = new MemoryStream())
            {
                using (var gzip = new GZipStream(compressed, CompressionMode.Compress, true))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                }

                return compressed.ToArray();
            }
        }

        public byte[] Decompress(byte[] bytes)
        {
            Guard.AgainstNull(bytes, nameof(bytes));

            using (var gzip = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var decompressed = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = gzip.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            decompressed.Write(buffer, 0, count);
                        }
                    } while (count > 0);
                    return decompressed.ToArray();
                }
            }
        }
    }
}