namespace GameLib.Core.Parsers.Base
{
	public class Base62Encoder : BaseEncoder
	{
		public Base62Encoder() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray(), false) { }
	}
}