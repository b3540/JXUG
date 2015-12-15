using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MStopWatch.VM
{
    public class LapTime
    {
        public TimeSpan Span { get; set; }
        public DateTime Time { get; set; }
        public LapTime( DateTime time, TimeSpan span )
        {
            this.Time = time;
            this.Span = span;
        }

    }
    public class StopWatchVM : BindableBase
    {
        private int _mode = 0;
        private DateTime _startTime;
        private DateTime _now;
        private TimeSpan _nowSpan = new TimeSpan(0);
        private ObservableCollection<LapTime> _items = new ObservableCollection<LapTime>();
        private bool _loop = false; // タイマーループ
        private Task _task;
        public event Action OnTimer;    // UI用コールバック

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
        public ObservableCollection<LapTime> Items
        {
            get { return _items; }
            set { this.SetProperty(ref _items, value); }
        }
        public TimeSpan NowSpan
        {
            get { return _nowSpan; }
            set { this.SetProperty(ref _nowSpan, value); }
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
                    if (OnTimer != null)
                        OnTimer();
                    else
                    {
                        this.NowSpan = _now - _startTime;   // 画面を更新
                        // ただし、Android の場合はスレッド越えで落ちるため
                        // 別途コールバックを使う
                    }
                }
            });
            _task.Start();
        }
        public void Stop()
        {
            _now = DateTime.Now;
            this.Items.Add( new LapTime( _now,  _now - _startTime ));
            _loop = false;
            Mode = 2;
        }
        public void Reset()
        {
            _now = DateTime.Now;
            _startTime = _now;
            this.NowSpan = new TimeSpan(0);
            _items.Clear();
            Mode = 0;
        }
        public void Lap()
        {
            _now = DateTime.Now;
            this.Items.Add(new LapTime(_now, _now - _startTime));
        }
        public void ClickLap()
        {
            Lap();
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
