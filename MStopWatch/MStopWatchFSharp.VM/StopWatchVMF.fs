namespace MStopWatch.VMF
open System
open System.ComponentModel
open System.Collections.ObjectModel
open System.Threading.Tasks

type LapTime(num, span, time ) = 
    let mutable _num = num
    let mutable _span = span
    let mutable _time = time
    member this.Num 
        with get() = _num
        and  set(value) = _num <- value
    member this.Span 
        with get() = _span
        and  set(value) = _span <- value
    member this.Time 
        with get() = _time
        and  set(value) = _time <- value

type Async() =
    // await を自作する
    static member await (t:Task) =
        t.Start()
        t.Wait()
    static member await (t:Task<'a>) =
        t.Start()
        t.Wait()
        t.Result
    static member await (t:Async<'a>) =
        ( t |> Async.StartAsTask ).Wait()

type StopWatchVM() = 
    let ev = new Event<_,_>()
    let mutable _mode = 0
    let mutable _startTime = DateTime()
    let mutable _now = DateTime()
    let mutable _nowSpan = TimeSpan()
    let mutable _items = new ObservableCollection<LapTime>()
    let mutable _loop = false
    let mutable _task:Task = null

    member this.StartButtonText 
        with get() = 
            match _mode with
            | 0 -> "Start"
            | 1 -> "Stop"
            | 2 -> "Restart"
            | _ -> ""
    member this.Mode 
        with get() = _mode
        and set(value) = 
            if ( _mode <> value ) then
                _mode <- value
                ev.Trigger(this, PropertyChangedEventArgs("StartButtonText"))
                ev.Trigger(this, PropertyChangedEventArgs("Mode"))
    member this.Items 
        with get() = _items
        and set(value) = 
            _items <- value
            ev.Trigger(this, PropertyChangedEventArgs("Items"))
    member this.NowSpan
        with get() = _nowSpan
        and set(value) = 
            _nowSpan <- value
            ev.Trigger(this, PropertyChangedEventArgs("NowSpan"))


    member this.ClickStart() =
        match _mode with
        | 0 -> this.Start()
        | 1 -> this.Stop()
        | 2 -> this.Reset()
        | _ -> ()
    member this.ClickLap() = this.Lap()

    member this.Start() = 
        _now <- DateTime.Now
        _startTime <- _now
        _items.Clear()
        _loop <- true
        this.Mode <- 1
        _task <- new Task( fun () ->
                while ( _loop ) do
                    ( Async.Sleep(100) |> Async.StartAsTask ).Wait()
                    _now <- DateTime.Now
                    this.NowSpan <- _now - _startTime
            )
        _task.Start()

    member this.Stop() = 
        _now <- DateTime.Now
        this.Items.Add( LapTime( this.Items.Count+1, _now, _now-_startTime))
        _loop <- false
        this.Mode <- 2

    member this.Reset() = 
        _now <- DateTime.Now
        _startTime <- _now
        this.NowSpan <- TimeSpan(0,0,0)
        this.Items.Clear()
        this.Mode <- 0

    member this.Lap() =
        _now <- DateTime.Now
        this.Items.Add( LapTime( this.Items.Count+1, _now, _now-_startTime))

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = ev.Publish





