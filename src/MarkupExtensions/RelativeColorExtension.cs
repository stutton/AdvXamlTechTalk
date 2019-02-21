using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(Brush))]
    public class RelativeColorExtension : MarkupExtension
    {
        public Color StartColor { get; set; }
        public ColorModifier Modifier { get; set; }

        public RelativeColorExtension() { }
        
        public RelativeColorExtension(ColorModifier modifier, Color startColor)
        {
            Modifier = modifier;
            StartColor = startColor;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var color = ColorHelpers.ModifyColor(StartColor, Modifier);
            return new SolidColorBrush(color);
        }
    }
}
