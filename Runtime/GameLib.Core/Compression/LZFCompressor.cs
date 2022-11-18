using GameLib.Core.Compression.Internal;

namespace GameLib.Core.Compression
{
	/// <summary>
	/// compress data using fast LZF compression method
	/// </summary>
	public class LZFCompressor : ICompressor
	{
		private readonly LZF _lzf = new LZF();

		public byte[] Compress(byte[] bytes)
		{
			return _lzf.CompressBytes(bytes);
		}

		public byte[] Compress(byte[] bytes, int bytesCount)
		{
			return _lzf.CompressBytes(bytes, bytesCount);
		}

		public byte[] Decompress(byte[] bytes)
		{
			return _lzf.DecompressBytes(bytes);
		}

		public byte[] Decompress(byte[] bytes, int bytesCount)
		{
			return _lzf.DecompressBytes(bytes, bytesCount);
		}

		public int Compress(byte[] srcBuffer, int srcBytesCount, byte[] dstBuffer, uint dstBytesOffset)
		{
			return _lzf.CompressBytes(srcBuffer, srcBytesCount, dstBuffer, dstBytesOffset);
		}

		public int Decompress(byte[] srcBuffer, int srcBytesCount, uint srcBytesOffset, byte[] dstBuffer)
		{
			return _lzf.DecompressBytes(srcBuffer, srcBytesCount, srcBytesOffset, dstBuffer);
		}
	}
}
