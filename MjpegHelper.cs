using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Macroscope;

public static class MjpegHelper
{

    private static readonly byte[] JpegStart = new byte[] { 0xFF, 0xD8 };
    private static readonly byte[] JpegEnd = new byte[] { 0xFF, 0xD9 };

    public static async Task<byte[]> TryGetMjpegFrameAsync(this MemoryStream stream)
    {
        int startPos = await IndexOfAsync(stream, JpegStart);
        int endPos = await IndexOfAsync(stream, JpegEnd, startPos);

        if (startPos >= 0 && endPos >= 0)
        {
            byte[] data = stream.ToArray();
            if (data.Length >= endPos)
            {
                return data.Skip(startPos).Take(endPos - startPos + 2).ToArray();
            }
        }

        return null;
    }

    private static async Task<int> IndexOfAsync(MemoryStream stream, byte[] pattern, int startIndex = 0)
    {
        int patternIndex = 0;
        for (int i = startIndex; i < stream.Length; i++)
        {
            if (stream.GetBuffer()[i] == pattern[patternIndex])
            {
                patternIndex++;
                if (patternIndex == pattern.Length)
                    return i - pattern.Length + 1;
            }
            else
            {
                patternIndex = 0;
            }
        }
        return -1;
    }

}