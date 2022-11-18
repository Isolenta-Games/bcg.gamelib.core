using System;
using GameLib.Core.Json;
using GameLib.Core.Utils;
using Newtonsoft.Json;

namespace GameLib.Core.CommonTypes
{
	[JsonConverter(typeof(RangeFConverter)), Serializable]
	public struct RangeF
	{
		public static readonly RangeF Zero = new RangeF(0, 0);

		// ReSharper disable InconsistentNaming
		public float min;

		public float max;
		// ReSharper restore InconsistentNaming

		[JsonIgnore]
		public float Length
		{
			get
			{
				Normalize();
				return Math.Abs(max - min);
			}
		}

		public RangeF(float constant)
		{
			min = constant;
			max = constant;
		}

		public RangeF(int from, int to)
		{
			min = from;
			max = to;

			Normalize();
		}

		public RangeF(float from, float to)
		{
			min = from;
			max = to;

			Normalize();
		}

		[JsonIgnore] public bool IsZero => min == 0 && min == max;

		public void Normalize()
		{
			if (min > max)
			{
				var x = min;
				min = max;
				max = x;
			}
		}

		public bool IsInRange(float x)
		{
			Normalize();
			return x >= min && x <= max;
		}

		public bool IsInRange(int x)
		{
			Normalize();
			return x >= min && x <= max;
		}

		public float Clamp(float x)
		{
			Normalize();
			return MathEx.Clamp(x, min, max);
		}

		public float GetK(float x)
		{
			Normalize();
			return Length != 0 ? MathEx.Clamp01((x - min) / Length) : 0;
		}

		public float ClampDelta(float x, float delta)
		{
			Normalize();

			var val = x + delta;

			if (val < min)
			{
				delta = min - x;
			}

			if (val > max)
			{
				delta = max - x;
			}

			return delta;
		}

		public float GetMiddle()
		{
			Normalize();

			return (min + max) * 0.5f;
		}

#if UNITY_2017_1_OR_NEWER
		public float GetRandom()
		{
			Normalize();
			return UnityEngine.Random.Range(min, max);
		}
#endif

		public float GetRandom(Random random)
		{
			Normalize();
			return random.RandomRange(min, max);
		}

		public float Lerp(float k)
		{
			Normalize();
			return MathEx.Lerp(min, max, MathEx.Clamp01(k));
		}

		public override string ToString()
		{
			return "[" + min.ToString("0.000") + ", " + max.ToString("0.000") + "]";
		}

		public static RangeF operator *(RangeF r, float k)
		{
			return new RangeF(r.min * k, r.max * k);
		}

		public static RangeF operator /(RangeF r, float k)
		{
			k = 1.0f / k;
			return new RangeF(r.min * k, r.max * k);
		}

		public static RangeF operator *(RangeF r, RangeF r2)
		{
			return new RangeF(r.min * r2.min, r.max * r2.max);
		}

		public static RangeF operator /(RangeF r, RangeF r2)
		{
			return new RangeF(r.min / r2.min, r.max / r2.max);
		}
	}
}