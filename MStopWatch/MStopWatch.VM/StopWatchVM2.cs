using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MStopWatch.VM2
{
    public class LapTime
    {
        public int Num { get; set; }
        public TimeSpan Span { get; set; }
        public DateTime Time { get; set; }
        public LapTime(int num, DateTime time, TimeSpan span)
        {
            this.Num = num;
            this.Time = time;
            this.Span = span;
        }
    }

    public class StopWatchModel
    {
        public DateTime StartTime { get; set; }
        public DateTime Now { get; set;  }
        public ObservableCollection<LapTime> Items = new ObservableCollection<LapTime>();
        private Task _task;
        private bool _loop = false; // タイマーループ
        public event Action OnTimer;    // UI用コールバック

        public void Start()
        {
            StartTime = Now = DateTime.Now;
            Items.Clear();
            _loop = true;
            _task = new Task(async () => {
                while (_loop)
                {
                    await Task.Delay(100);
                    Now = DateTime.Now;
                    if (OnTimer != null)
                        OnTimer();
                }
            });
            _task.Start();
        }
        public void Stop()
        {
            Now = DateTime.Now;
            Items.Add(new LapTime(this.Items.Count + 1, Now, Now - StartTime));
            _loop = false;
        }
        public void Reset()
        {
            StartTime = Now = DateTime.Now;
            Items.Clear();
        }
        public void Lap()
        {
            Now = DateTime.Now;
            this.Items.Add(new LapTime(this.Items.Count+1, Now, Now - StartTime));
        }

    }
    public class StopWatchVM : BindableBase
    {
        private int _mode = 0;
        private TimeSpan _nowSpan = new TimeSpan(0);

        StopWatchModel _model;
        public StopWatchVM()
        {
            _model = new StopWatchModel();
            _model.OnTimer += () => {
                this.NowSpan = _model.Now - _model.StartTime;
            };
        }

        public string StartButtonText
        {
            get
            {
                switch ( _mode )
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
                if ( _mode != value )
                {
                    _mode = value;
                    this.OnPropertyChanged("StartButtonText");
                }
            }
        }
        public void ClickStart()
        {
            switch ( _mode )
            {
                case 0: _model.Start(); Mode = 1; break;
                case 1: _model.Stop(); Mode = 2;  break;
                case 2: _model.Reset(); Mode = 0;
                    this.NowSpan = new TimeSpan(0); break;
            }
        }
        public ObservableCollection<LapTime> Items
        {
            get { return _model.Items; }
        }
        public TimeSpan NowSpan
        {
            get { return _nowSpan; }
            set { this.SetProperty(ref _nowSpan, value); }
        }
        public void ClickLap()
        {
            _model.Lap();
        }
        public void Reset()
        {
            _model.Reset();
            this.NowSpan = new TimeSpan(0);
            Mode = 0;
        }
    }

    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティの変更を通知するためのマルチキャスト イベント。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティが既に目的の値と一致しているかどうかを確認します。必要な場合のみ、
        /// プロパティを設定し、リスナーに通知します。
        /// </summary>
        /// <typeparam name="T">プロパティの型。</typeparam>
        /// <param name="storage">get アクセス操作子と set アクセス操作子両方を使用したプロパティへの参照。</param>
        /// <param name="value">プロパティに必要な値。</param>
        /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
        /// この値は省略可能で、
        /// CallerMemberName をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
        /// <returns>値が変更された場合は true、既存の値が目的の値に一致した場合は
        /// false です。</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// プロパティ値が変更されたことをリスナーに通知します。
        /// </summary>
        /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
        /// この値は省略可能で、
        /// <see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
