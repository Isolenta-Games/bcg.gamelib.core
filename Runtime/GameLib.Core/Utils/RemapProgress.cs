using System;
using GameLib.Core.CommonTypes;

namespace GameLib.Core.Utils
{
	public class RemapProgress : IProgress<float>
	{
		private RangeF _range;
		private readonly IProgress<float> _progress;

		public RemapProgress(RangeF range, IProgress<float> progress)
		{
			_range = range;
			_progress = progress;
		}

		public void Report(float value)
		{
			_progress?.Report(_range.Lerp(value));
		}
	}
}