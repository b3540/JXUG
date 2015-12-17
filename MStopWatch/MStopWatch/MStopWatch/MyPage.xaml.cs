using System;
using System.Collections.Generic;

using Xamarin.Forms;
using MStopWatch.VM;    // ViewModel にタイマーがあるパターン
//using MStopWatch.VM2;   // Model にタイマーがあるパターン
//using MStopWatch.VMF;    // ViewModel を F# で書いたパターン

namespace MStopWatch
{
	public partial class MyPage : ContentPage
	{
		public MyPage ()
		{
			InitializeComponent ();
            this.LayoutChanged += MyPage_LayoutChanged;
		}

        StopWatchVM _vm;
        private void MyPage_LayoutChanged(object sender, EventArgs e)
        {
            _vm = new StopWatchVM();
            this.BindingContext = _vm;
            _vm.Reset();
        }

        private void clickStart(object sender, EventArgs e)
        {
            _vm.ClickStart();
        }
        private void clickLap(object sender, EventArgs e)
        {
            _vm.ClickLap();
        }
    }
}

