using System;

namespace GameLib.Core.Compression.Internal
{
	internal struct LZFPayload
	{
		public static readonly uint PAYLOAD_DATA_SIZE = sizeof(UInt32) + sizeof(byte);

		public uint UncompressedDataSize;
		public bool IsDataCompressed;

		public LZFPayload(int uncompressedDataSize, bool isDataCompressed)
		{
			if (uncompressedDataSize < 0 || uncompressedDataSize >= LZF.MAX_UNCOMPRESSED_DATA_SIZE)
			{
				throw new Exception("Invalid uncompressed data size: " + uncompressedDataSize);
			}

			this.UncompressedDataSize = (uint)uncompressedDataSize;
			this.IsDataCompressed = isDataCompressed;
		}

		public LZFPayload(byte[] compressedData, uint bytesOffset, out uint offset)
		{
			offset = bytesOffset;

			this.UncompressedDataSize = (uint)(compressedData[offset] << 24 | compressedData[offset + 1] << 16 | compressedData[offset + 2] << 8 | compressedData[offset + 3]);
			offset += 4;

			this.IsDataCompressed = compressedData[offset] == 1;
			offset++;
		}

		public int Apply(byte[] buffer, uint bytesOffset)
		{
			if (buffer.Length < PAYLOAD_DATA_SIZE)
			{
				throw new Exception("Invalid buffer size " + buffer.Length);
			}

			int offset = (int)bytesOffset;
			buffer[offset] = (byte)(UncompressedDataSize >> 24);
			offset++;

			buffer[offset] = (byte)(UncompressedDataSize >> 16);
			offset++;

			buffer[offset] = (byte)(UncompressedDataSize >> 8);
			offset++;

			buffer[offset] = (byte)UncompressedDataSize;
			offset++;

			buffer[offset] = (byte)(IsDataCompressed ? 1 : 0);
			offset++;

			return offset;
		}
	}
}
