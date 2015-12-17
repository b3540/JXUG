using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MStopWatch
{
    public class CustomButton : Button
    {
        public static BindableProperty ModeProperty =
            BindableProperty.Create<CustomButton, int>(
                p => p.Mode,
                0,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var uc = bindable as CustomButton;
                    switch (newValue)
                    {
                        case 0: uc.Text = "開始"; break;
                        case 1: uc.Text = "停止"; break;
                        case 2: uc.Text = "リセット"; break;
                    }
                    ((CustomButton)bindable).Mode = newValue;
                });
        public int Mode
        {
            get { return (int)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
    }
}
