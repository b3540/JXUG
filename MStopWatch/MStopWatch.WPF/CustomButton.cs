using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MStopWatch.WPF
{
    class CustomButton : Button 
    {
        /// <summary>
        /// モードを指定
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                "Mode",     // プロパティ名
                typeof(int),    // プロパティの型
                typeof(CustomButton),　 // コントロールの型
                new FrameworkPropertyMetadata(   // メタデータ                   
                    0,
                    new PropertyChangedCallback((o, e) =>
                    {
                        var uc = o as CustomButton;
                        if (uc != null)
                        {
                            int v = (int)e.NewValue;
                            switch ( v )
                            {
                                case 0: uc.Content = "開始"; break;
                                case 1: uc.Content = "停止"; break;
                                case 2: uc.Content = "リセット"; break;
                            }
                        }
                    })));
        // 依存プロパティのラッパー
        public int Mode
        {
            get { return (int)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
    }
}
