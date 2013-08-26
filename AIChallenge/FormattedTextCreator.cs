using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace AIChallenge
{
	internal sealed class FormattedTextCreator
	{
		private static readonly Typeface _typeface = new Typeface("Arial");

		public static FormattedText Create(string text, int size)
		{
			return Create(text, size, Brushes.White);
		}

		public static FormattedText Create(string text, int size, Brush brush)
		{
			return new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, _typeface, size, brush);
		}
	}
}
