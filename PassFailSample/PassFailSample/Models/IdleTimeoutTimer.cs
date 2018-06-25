using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PassFailSample.Models
{
    public class IdleTimeoutTimer
    {
        #region Properties

        public bool IsInitialized { get; private set; }
        private Timer TimeoutTimer { get; set; }

        #endregion

        #region Constructor, Init/Deinit

        public IdleTimeoutTimer()
        {
            this.IsInitialized = false;
        }

        public bool Initialize(TimerCallback callBack, int? dueTime = null)
        {
            if(callBack != null)
            {
                this.TimeoutTimer = new Timer(callBack, null, dueTime ?? Timeout.Infinite, Timeout.Infinite);
                this.IsInitialized = true;
            }
            else
            {
                this.IsInitialized = false;
            }
            return this.IsInitialized;

        }

        #endregion

        #region Public Methods

        public bool ResetTimer(int dueTime, bool triggerNow = false)
        {
            if (this.IsInitialized)
            {
                this.TimeoutTimer.Change(triggerNow ? 0 : dueTime, Timeout.Infinite);
            }
            return this.IsInitialized;
        }

        #endregion
    }
}
