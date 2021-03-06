﻿using UnityEngine;

using System;
using System.Collections;

public class Timer
{
    internal static readonly object mutex = new object();

    internal static Timer freeRoot;

    internal bool cancelled;

    internal Action callback1;
    internal Action<Timer> callback2;

    internal Timer next;
    internal Timer prev;

    internal Timer helpListNext;

    internal TimerManager manager;

    internal int numRepeats;
    internal int numRepeated;

    internal float timeout;
    internal double fireTime;
    internal double scheduleTime;

    public string name;
    public object userData;

    public void Cancel()
    {
        lock (this)
        {
            if (!cancelled)
            {
                cancelled = true;
                manager.CancelTimer(this);
            }
        }
    }

    internal void Fire()
    {
        lock (this)
        {
            try
            {
                callback2(this);

                if (!cancelled)
                {
                    ++numRepeated;
                    if (numRepeated == numRepeats)
                    {
                        Cancel();
                    }
                    else
                    {
                        fireTime = manager.currentTime + timeout;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception while firing timer: " + e);
                Cancel();
            }
        }
    }

    internal static void DefaultTimerCallback(Timer timer)
    {
        timer.callback1();
    }

    public bool IsRepeated
    {
        get { return numRepeats != 1; }
    }

    public float Timeout
    {
        get { return timeout; }
    }

    public float Elapsed
    {
        get { return (float)(manager.currentTime - scheduleTime); }
    }

    protected static Timer FreeRoot
    {
        get { return freeRoot; }
        set { freeRoot = value; }
    }

    public override string ToString()
    {
        Delegate callback = callback2 != DefaultTimerCallback ? (Delegate)callback2 : (Delegate)callback1;
        return string.Format("[Target={0}, Method={1}, IsRepeated={2}, Timeout={3}, Elapsed={4}]",
            callback != null ? callback.Target : null,
            callback != null ? callback.Method : null,
            this.IsRepeated,
            this.Timeout,
            this.manager != null ? this.Elapsed : 0);
    }

    //////////////////////////////////////////////////////////////////////////////

    #region Objects pool

    internal protected static Timer NextFreeTimer()
    {
        lock (mutex)
        {
            Timer timer;
            if (freeRoot != null)
            {
                timer = freeRoot;
                freeRoot = timer.next;
                timer.prev = timer.next = null;
            }
            else
            {
                timer = new Timer();
            }

            return timer;
        }
    }

    internal protected static void AddFreeTimer(Timer timer)
    {
        lock (mutex)
        {
            timer.Reset();

            if (freeRoot != null)
            {
                timer.next = freeRoot;
            }

            freeRoot = timer;
        }
    }

#if LUNAR_DEVELOPMENT

        public static Timer NextTimer(Timer timer)
        {
            return timer.next;
        }

        public static Timer PrevTimer(Timer timer)
        {
            return timer.prev;
        }

        public static Timer NextHelperListTimer(Timer timer)
        {
            return timer.helpListNext;
        }

#endif // LUNAR_DEVELOPMENT

    private void Reset()
    {
        next = prev = null;
        helpListNext = null;
        manager = null;
        callback1 = null;
        callback2 = null;
        numRepeats = numRepeated = 0;
        timeout = 0;
        fireTime = 0;
        scheduleTime = 0;
        cancelled = false;
        name = null;
        userData = null;
    }

    #endregion
}
