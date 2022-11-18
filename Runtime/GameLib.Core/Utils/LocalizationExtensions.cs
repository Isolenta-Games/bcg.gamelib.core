namespace GameLib.Core.Utils
{
	public static class LocalizationExtensions
	{
		public static void ForceIncluded()
		{
			_ = new System.Globalization.ChineseLunisolarCalendar();
			_ = new System.Globalization.HebrewCalendar();
			_ = new System.Globalization.HijriCalendar();
			_ = new System.Globalization.JapaneseCalendar();
			_ = new System.Globalization.JapaneseLunisolarCalendar();
			_ = new System.Globalization.KoreanCalendar();
			_ = new System.Globalization.KoreanLunisolarCalendar();
			_ = new System.Globalization.PersianCalendar();
			_ = new System.Globalization.TaiwanCalendar();
			_ = new System.Globalization.TaiwanLunisolarCalendar();
			_ = new System.Globalization.ThaiBuddhistCalendar();
			_ = new System.Globalization.UmAlQuraCalendar();
		}
	}
}