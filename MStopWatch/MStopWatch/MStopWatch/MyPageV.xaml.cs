using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MStopWatch
{
	public partial class MyPageV : ContentPage, INotifyPropertyChanged
    {
        public MyPageV()
        {
            InitializeComponent();
            this.LayoutChanged += MyPage_LayoutChanged;
        }

        public class LapTime
        {
            public TimeSpan Span { get; set; }
            public DateTime Time { get; set; }
            public LapTime(DateTime time, TimeSpan span)
            {
                this.Time = time;
                this.Span = span;
            }
        }
        private int _mode = 0;
        private DateTime _startTime;
        private DateTime _now;
        private TimeSpan _nowSpan = new TimeSpan(0);
        private ObservableCollection<LapTime> _items = new ObservableCollection<LapTime>();
        private bool _loop = false; // タイマーループ
        private Task _task;

        public string StartButtonText
        {
            get
            {
                switch (_mode)
                {
                    case 0: return "Start";
                    case 1: return "Stop";
                    case 2: return "Restart";
                    default: return "";
                }
            }
        }
        public int Mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    this.OnPropertyChanged("StartButtonText");
                }
            }
        }
        public ObservableCollection<LapTime> Items
        {
            get { return _items; }
        }
        public TimeSpan NowSpan
        {
            get { return _nowSpan; }
            set {
                _nowSpan = value;
                this.OnPropertyChanged("NowSpan");
            }
        }

        private void MyPage_LayoutChanged(object sender, EventArgs e)
        {
            this.BindingContext = this;
            this.Reset();
        }

        private void clickStart(object sender, EventArgs e)
        {
            switch (_mode)
            {
                case 0:
                    Start();
                    break;
                case 1:
                    Stop();
                    break;
                case 2:
                    Reset();
                    break;
            }
        }
        public void Start()
        {
            _now = DateTime.Now;
            _startTime = _now;
            _items.Clear();
            _loop = true;
            Mode = 1;
            _task = new Task(async () => {
                while (_loop)
                {
                    await Task.Delay(100);
                    _now = DateTime.Now;
                    NowSpan = _now - _startTime;   // 画面を更新
                }
            });
            _task.Start();
        }
        public void Stop()
        {
            _now = DateTime.Now;
            this.Items.Add(new LapTime(_now, _now - _startTime));
            _loop = false;
            Mode = 2;
        }
        public void Reset()
        {
            _now = DateTime.Now;
            _startTime = _now;
            NowSpan = new TimeSpan(0);
            _items.Clear();
            Mode = 0;
        }

        private void clickLap(object sender, EventArgs e)
        {
            _now = DateTime.Now;
            this.Items.Add(new LapTime(_now, _now - _startTime));
        }

    }
}

