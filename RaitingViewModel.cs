using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace Airports
{
    public class RaitingViewModel : ReactiveObject
    {
        private int _lastCount;
        private int _24Count;
        private int _allCount;

        public int LastCount
        {
            get => _lastCount;
            set => this.RaiseAndSetIfChanged(ref _lastCount, value);
        }
        public int H24Count
        {
            get => _24Count;
            set => this.RaiseAndSetIfChanged(ref _24Count, value);
        }
        public int AllCount
        {
            get => _allCount;
            set => this.RaiseAndSetIfChanged(ref _allCount, value);
        }
    }
}
