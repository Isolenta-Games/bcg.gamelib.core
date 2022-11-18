using System;
using GameLib.Core.Json;
using GameLib.Core.Utils;
using Newtonsoft.Json;

namespace GameLib.Core.CommonTypes
{
	[JsonConverter(typeof(RangeConverter)), Serializable]
	public struct Range
	{
		public static readonly Range Zero = new Range(0, 0);

		// ReSharper disable InconsistentNaming
		public int min;
		public int max;
		// ReSharper restore InconsistentNaming

		[JsonIgnore]
		public int Length
		{
			get
			{
				Normalize();
				return Math.Abs(max - min + 1);
			}
		}

		public Range(int constant)
		{
			min = constant;
			max = constant;
			Normalize();
		}

		public Range(int from, int to)
		{
			min = from;
			max = to;

			Normalize();
		}
		[JsonIgnore] public bool IsZero => min == 0 && min == max;

		public static int Clamp(int min, int max, int v)
		{
			return Math.Max(min, Math.Min(v, max));
		}

		public void Normalize()
		{
			if (min > max)
			{
				var x = min;
				min = max;
				max = x;
			}
		}

		public int GetMiddle()
		{
			Normalize();
			return MathEx.RoundToInt((min + max) * 0.5f);
		}

		public bool IsInRange(int x)
		{
			Normalize();
			return x >= min && x <= max;
		}

		public float Lerp(float k)
		{
			Normalize();
			return MathEx.Lerp(min, max, MathEx.Clamp01(k));
		}

		public int Clamp(int x)
		{
			Normalize();
			return MathEx.Clamp(x, min, max);
		}

		public float Clamp(float x)
		{
			Normalize();
			if (min > x)
			{
				return min;
			}

			if (max < x)
			{
				return max;
			}

			return x;
		}

#if UNITY_2017_1_OR_NEWER
		public int GetRandom()
		{
			Normalize();
			return UnityEngine.Random.Range(min, max + 1);
		}
#endif

		public int GetRandom(Random random)
		{
			Normalize();
			return random.RandomRange(min, max);
		}

		public int GetRandom(Random random, int step)
		{
			if (step <= 0)
				return 0;

			Normalize();

			var minS = min / step;
			var maxS = max / step;

			return random.Next(minS, maxS + 1) * step;
		}

		public override string ToString()
		{
			return (min == max) ? "[" + min + "]" : "[" + min + ", " + max + "]";
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Range))
			{
				return false;
			}

			var range = (Range)obj;
			return min == range.min &&
				max == range.max;
		}

		public override int GetHashCode()
		{
			var hashCode = -897720056;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + min.GetHashCode();
			hashCode = hashCode * -1521134295 + max.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(Range range1, Range range2)
		{
			return range1.Equals(range2);
		}

		public static bool operator !=(Range range1, Range range2)
		{
			return !(range1 == range2);
		}
	}
}