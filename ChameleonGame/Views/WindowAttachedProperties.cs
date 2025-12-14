using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameleonGame.Views
{
    public class WindowAttachedProperties
    {
        public static readonly AttachedProperty<bool?> DialogResultProperty = AvaloniaProperty.RegisterAttached<WindowAttachedProperties, Window, bool?>("DialogResult");

        public static bool? GetDialogResult(Window element)
        {
            return element.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(Window element, bool? value)
        {
            element.SetValue(DialogResultProperty, value);
        }

        static WindowAttachedProperties()
        {
            DialogResultProperty.Changed.AddClassHandler<Window>((window, args) =>
            {
                if (args.NewValue is bool result)
                {
                    window.Close(result);
                }
            });
        }
    }
}
