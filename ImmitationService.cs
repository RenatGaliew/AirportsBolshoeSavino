using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Timers;
using ReactiveUI;
using Timer = System.Timers.Timer;

namespace Airports
{
    public class ImmitationServiceViewModel : ReactiveObject
    {
        private DateTime _time;
        private TimeSpan TimeToAdd;
        private Timer timer;

        public DateTime Time
        {
            get => _time;
            private set => this.RaiseAndSetIfChanged(ref _time, value);
        }
        
        public ImmitationServiceViewModel(DateTime time, ImmitationSpeed settingsSpeed)
        {
            Time = time;
            var t = (int)settingsSpeed;
            TimeToAdd = TimeSpan.FromSeconds(Convert.ToDouble(t));
            timer = new Timer(1000);
            timer.Elapsed += AddTime;
        }

        private void AddTime(object sender, ElapsedEventArgs e)
        {
            Time = Time + TimeToAdd;
        }

        public void Start()
        {
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
        }

        public void Update(ImmitationSpeed immitationSpeed)
        {
            var t = (int)immitationSpeed;
            TimeToAdd = TimeSpan.FromSeconds(Convert.ToDouble(t));
        }
    }
}
