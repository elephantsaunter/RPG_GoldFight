using System;

public class TimerSvc: SystemRoot {
    public static TimerSvc Instance = null;
    private PETimer pt;

    public void InitSvc () {
        Instance = this;
        pt = new PETimer();
        // SET LOG OUTPUT
        pt.SetLog((string info) => {
            PECommon.Log(info);
        });
        PECommon.Log("Init TimeSvc...");
    }
    
    private void Update() {
        pt.Update();
    }
    public int AddTimerTask(Action<int> callback,double delay,PETimeUnit timeUnit=PETimeUnit.Millisecond,int count = 1) {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }
    public double GetNowTime() {
        return pt.GetMillisecondsTime();
    }
    public void DelTask(int tid) {
        pt.DeleteTimeTask(tid);
    }
}
