namespace GameLib.Core.Utils
{
	/// <summary>
	/// convert types
	/// </summary>
	public static class ConvertUtils
	{
		public static long ToLong(long s0, long s1, long s2, long s3)
		{
			return ((long)(short)s0 << 0x30) + ((long)(short)s1 << 0x20) + ((long)(short)s2 << 0x10) + (long)(short)s3;
		}

		public static long ToLong(int s0, int s1, int s2, int s3)
		{
			return ((long)(short)s0 << 0x30) + ((long)(short)s1 << 0x20) + ((long)(short)s2 << 0x10) + (long)(short)s3;
		}

		public static long ToLong(short s0, short s1, short s2, short s3)
		{
			return ((long)s0 << 0x30) + ((long)s1 << 0x20) + ((long)s2 << 0x10) + (long)s3;
		}

		public static short[] ToShortArray(long src)
		{
			return new short[]
			{
				(short)((src >> 0x30) & 0xFFFF),
				(short)((src >> 0x20) & 0xFFFF),
				(short)((src >> 0x10) & 0xFFFF),
				(short)(src & 0xFFFF),
			};
		}
	}
}
