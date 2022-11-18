namespace GameLib.Core.Parsers.Base
{
	public class Base52Encoder : BaseEncoder
	{
		public Base52Encoder() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray(), false) { }
	}
}