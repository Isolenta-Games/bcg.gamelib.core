using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "InconsistentNaming"), SuppressMessage("ReSharper", "CheckNamespace")]
public class LambdaComparer<T> : IComparer<T>
{
	private readonly Comparison<T> _comparision;

	public LambdaComparer(Comparison<T> comparision)
	{
		_comparision = comparision;
	}

	public int Compare(T x, T y)
	{
		return _comparision(x, y);
	}
}