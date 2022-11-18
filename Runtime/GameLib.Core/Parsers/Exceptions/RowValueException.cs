using System;

namespace GameLib.Core.Parsers.Exceptions
{
	public class RowValueException : Exception
	{
		public override string StackTrace => "";

		public RowValueException()
		{
		}

		public RowValueException(string message) : base(message)
		{
		}
	}
}