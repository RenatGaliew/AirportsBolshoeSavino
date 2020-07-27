using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using ReactiveUI;

namespace Airports
{
    public class TimeTableViewModel : ReactiveObject
    {
        private DateTime _departureTime;
        private DateTime _arrivalTime;
        private TypeAirplane _type;
        private AirPort _from;
        private AirPort _to;
        private AirplaneModel _plane;
        private bool _inRating;
        public bool InRating
        {
            get => _inRating;
            set => this.RaiseAndSetIfChanged(ref _inRating, value);
        }
        /// <summary>
        /// Время вылета самолета
        /// </summary>
        public DateTime DepartureTime
        {
            get => _departureTime;
            set => this.RaiseAndSetIfChanged(ref _departureTime, value);
        }

        /// <summary>
        /// Время прилета самолета
        /// </summary>
        public DateTime ArrivalTime
        {
            get => _arrivalTime;
            set => this.RaiseAndSetIfChanged(ref _arrivalTime, value);
        }

        /// <summary>
        ///  Тип самолета
        /// </summary>
        public TypeAirplane Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        /// <summary>
        /// Откуда
        /// </summary>
        public AirPort From
        {
            get => _from;
            set => this.RaiseAndSetIfChanged(ref _from, value);
        }

        /// <summary>
        /// Куда
        /// </summary>
        public AirPort To
        {
            get => _to;
            set => this.RaiseAndSetIfChanged(ref _to, value);
        }

        /// <summary>
        /// Самолет
        /// </summary>
        public AirplaneModel Plane
        {
            get => _plane;
            set => this.RaiseAndSetIfChanged(ref _plane, value);
        }

        /// <summary>
        /// Количество пассажиров
        /// </summary>
        public int Count { get; set; }

        public TimeTableViewModel(TimeTableModel model)
        {
            From = model.From;
            To = model.To;
            ArrivalTime = model.ArrivalTime;
            DepartureTime = model.DepartureTime;
            Plane = model.Plane;
            Type = model.Type;
        }
    }
    public class RaitingToChartViewModel : ReactiveObject
    {
        private int _count;
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => this.RaiseAndSetIfChanged(ref _date, value);
        }
        public int Count
        {
            get => _count;
            set => this.RaiseAndSetIfChanged(ref _count, value);
        }
    }
    public class MainVM : ReactiveObject
    {
        private AirPort MyAirPort = new AirPort("Большое Савино", "Пермь",new TimeSpan());
        private List<AirPort> AirPorts;
        private List<AirplaneModel> AirplaneModels;
        private Timer _timer;
        public ImmitationServiceViewModel _immitationServiceVM;
        public List<TimeTableViewModel> AllList;
        public RaitingViewModel ArrivalRaitingVM;
        public RaitingViewModel DepartureRaitingVM;
        public TimeTableViewModel _lastTimeTable;
        public List<TimeTableViewModel> Last24Tables;
        private SettingsModel _settings;
        public ImmitationSpeed _speed;
        private ObservableCollection<RaitingToChartViewModel> _arrivalCounts;
        public ObservableCollection<RaitingToChartViewModel> ArrivalCounts 
        {
            get => _arrivalCounts;
            set => this.RaiseAndSetIfChanged(ref _arrivalCounts, value);
        }

        private ObservableCollection<RaitingToChartViewModel> _departmentsCounts;
        public ObservableCollection<RaitingToChartViewModel> DepartmentsCounts
        {
            get => _departmentsCounts;
            set => this.RaiseAndSetIfChanged(ref _departmentsCounts, value);
        }
        public ImmitationServiceViewModel ImmitationServiceVM
        {
            get => _immitationServiceVM;
            set => this.RaiseAndSetIfChanged(ref _immitationServiceVM, value);
        }
        public ImmitationSpeed Speed
        {
            get => _speed;
            set => this.RaiseAndSetIfChanged(ref _speed, value);
        }
        
        public MainVM()
        {
            ArrivalRaitingVM = new RaitingViewModel(); 
            DepartureRaitingVM = new RaitingViewModel();
            AirPorts = new List<AirPort>();
            AllList = new List<TimeTableViewModel>();
            Last24Tables = new List<TimeTableViewModel>();
            AirplaneModels = new List<AirplaneModel>();
            ArrivalCounts = new ObservableCollection<RaitingToChartViewModel>();
            DepartmentsCounts = new ObservableCollection<RaitingToChartViewModel>();
            AirPorts.Add(new AirPort("Внуково", "Москва", new TimeSpan(2, 0, 0)));
            AirPorts.Add(new AirPort("Домодедово", "Москва", new TimeSpan(2, 5, 0)));
            AirPorts.Add(new AirPort("Шереметьево", "Москва", new TimeSpan(2,15,0)));
            AirPorts.Add(new AirPort("Мурманск", "Мурманск", new TimeSpan(4, 0, 0)));
            AirPorts.Add(new AirPort("Казань", "Казань", new TimeSpan(1, 0, 0)));
            AirPorts.Add(new AirPort("Красноярск", "Красноярск", new TimeSpan(3, 40, 0)));
            AirPorts.Add(new AirPort("Волгоград", "Волгоград", new TimeSpan(2, 25, 0)));
            AirPorts.Add(new AirPort("Пулково", "Санкт-Петербург", new TimeSpan(2, 30, 0)));
            AirPorts.Add(new AirPort("Кольцово", "Екатеринбург", new TimeSpan(0, 30, 0)));
            AirPorts.Add(new AirPort("Курумоч", "Самара", new TimeSpan(1, 20, 0)));
            AirPorts.Add(new AirPort("Стригино", "Нижний Новгород", new TimeSpan(1, 35, 0)));
            AirPorts.Add(new AirPort("Симферополь", "Симферополь", new TimeSpan(3, 15, 0)));
            AirPorts.Add(new AirPort("Платов", "Ростов-на-Дону", new TimeSpan(2, 20, 0)));

            AirplaneModels.Add(new AirplaneModel(96, "Ту-134"));
            AirplaneModels.Add(new AirplaneModel(180, "Ту-154"));
            AirplaneModels.Add(new AirplaneModel(214, "Ту-204"));
            AirplaneModels.Add(new AirplaneModel(198, "Ил-62"));
            AirplaneModels.Add(new AirplaneModel(314, "Ил-86"));
            AirplaneModels.Add(new AirplaneModel(300, "Ил-96-300"));
            AirplaneModels.Add(new AirplaneModel(183, "Airbus A310"));
            AirplaneModels.Add(new AirplaneModel(149, "Airbus A320"));
            AirplaneModels.Add(new AirplaneModel(440, "Airbus A330"));
            AirplaneModels.Add(new AirplaneModel(114, "Boeing-737"));
            AirplaneModels.Add(new AirplaneModel(298, "Boeing-747"));
            AirplaneModels.Add(new AirplaneModel(252, "Boeing-767"));
            AirplaneModels.Add(new AirplaneModel(148, "Boeing-777"));

            LoadSettings();
            this.WhenAnyValue(x => x.Settings).Subscribe(OnSettingsChanged);
            this.WhenAnyValue(x => x.Speed).Subscribe(OnSpeedChanged);
            this.WhenAnyValue(x => x.ImmitationServiceVM.Time).Subscribe(OnTimeChanged);
            _timer = new Timer {Interval = 1000};
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                foreach (var raitingToChartViewModel in DepartmentsCounts.ToList())
                {
                    if (raitingToChartViewModel != null)
                    {
                        if (raitingToChartViewModel.Date < ImmitationServiceVM.Time - TimeSpan.FromHours(24))
                        {
                            DepartmentsCounts.Remove(raitingToChartViewModel);
                        }
                    }
                }

                foreach (var raitingToChartViewModel in ArrivalCounts.ToList())
                {
                    if (raitingToChartViewModel != null)
                    {
                        if (raitingToChartViewModel.Date < ImmitationServiceVM.Time - TimeSpan.FromHours(24))
                        {
                            ArrivalCounts.Remove(raitingToChartViewModel);
                        }
                    }
                }

                for (int i = 0; i < 24; i++)
                {
                    var date24 = ImmitationServiceVM.Time - TimeSpan.FromHours(23 - i);
                    var firstDate = new DateTime(date24.Year, date24.Month, date24.Day, date24.Hour, 0, 0);

                    var last0 = DepartmentsCounts.FirstOrDefault(x => x.Date.Hour == firstDate.Hour);
                    if (last0 == null)
                    {
                        DepartmentsCounts.Add(new RaitingToChartViewModel()
                        {
                            Count = 0,
                            Date = firstDate
                        });

                    }

                    last0 = ArrivalCounts.FirstOrDefault(x => x.Date.Hour == firstDate.Hour);
                    if (last0 == null)
                    {
                        ArrivalCounts.Add(new RaitingToChartViewModel()
                        {
                            Count = 0,
                            Date = firstDate
                        });
                    }
                }

                foreach (var last in DepartmentsCounts)
                {
                    foreach (var reis in Last24Tables.ToList().Where(x => !x.InRating && x.Type == TypeAirplane.Departure 
                                                                          && x.DepartureTime.Hour == last.Date.Hour 
                                                                          && x.DepartureTime.Day == last.Date.Day))
                    {
                        last.Count += reis.Count;
                        reis.InRating = true;
                    }
                }

                foreach (var last in ArrivalCounts)
                {
                    foreach (var reis in Last24Tables.ToList().Where(x => !x.InRating && x.Type == TypeAirplane.Arrival 
                                                                          && x.ArrivalTime.Hour == last.Date.Hour 
                                                                          && x.DepartureTime.Day == last.Date.Day))
                    {
                        last.Count += reis.Count;
                        reis.InRating = true;
                    }
                }
            });
        }

        private  void OnSettingsChanged(SettingsModel obj)
        {
            if (obj != null)
            {
                ImmitationServiceVM = new ImmitationServiceViewModel(DateTime.Now, obj.Speed);
                ImmitationServiceVM.Start();
            }
        }
        private void OnSpeedChanged(ImmitationSpeed obj)
        {
            if(obj != 0)
                ImmitationServiceVM?.Update(obj);
        }
        private void OnTimeChanged(DateTime time)
        {
            if(AllList.Count < 10)
            {
                Random r = new Random();
                var count = 100;
                TimeTableModel[] tables = new TimeTableModel[count];
                for (int i = 0; i < count; i++)
                {
                    int fromOrTo = r.Next(AirPorts.Count);
                    TypeAirplane type = r.Next(0,101) % 2 == 0 ? TypeAirplane.Arrival : TypeAirplane.Departure;
                    TimeTableModel timeTableModel;
                    var airportFromOrTo = AirPorts[fromOrTo];
                    var plane = AirplaneModels[r.Next(AirplaneModels.Count)]; 
                    DateTime arrivalTime = time + TimeSpan.FromHours(r.Next(5)) + TimeSpan.FromMinutes(r.Next(+61));
                    DateTime departmentTime = arrivalTime + airportFromOrTo.Time;
                    timeTableModel = new TimeTableModel(
                        departmentTime, 
                        arrivalTime, type,
                        type == TypeAirplane.Arrival ? airportFromOrTo : MyAirPort,
                        type == TypeAirplane.Arrival ? MyAirPort : airportFromOrTo, plane);
                    
                    tables[i] = timeTableModel;
                }
                
                foreach (var table in tables)
                {
                    AllList.Add(new TimeTableViewModel(table)
                    {
                        Count = r.Next(table.Plane.Capacity / 3, table.Plane.Capacity+1)
                    });
                }
            }
            foreach (var reis in AllList.ToList())
            {
                //прибытие
                if (reis.Type == TypeAirplane.Arrival)
                {
                    if (reis.ArrivalTime < time)
                    {
                        ArrivalRaitingVM.AllCount += reis.Count; 
                        Last24Tables.Add(reis);
                        ArrivalRaitingVM.H24Count += reis.Count;
                        ArrivalRaitingVM.LastCount = reis.Count;
                        LastTimeTable = reis;
                        AllList.Remove(reis);
                    }
                }

                //убытие
                if (reis.Type == TypeAirplane.Departure)
                {
                    if (reis.DepartureTime < time)
                    {
                        DepartureRaitingVM.AllCount += reis.Count;
                        Last24Tables.Add(reis);
                        DepartureRaitingVM.H24Count += reis.Count;
                        DepartureRaitingVM.LastCount = reis.Count;
                        LastTimeTable = reis;
                        AllList.Remove(reis);
                    }
                }
            }

            foreach (var reis in Last24Tables.ToList())
            {
                if (reis.Type == TypeAirplane.Arrival)
                {
                    if (time - TimeSpan.FromDays(1) > reis.ArrivalTime)
                    {
                        ArrivalRaitingVM.H24Count -= reis.Count;
                        Last24Tables.Remove(reis);
                    }
                }

                if (reis.Type == TypeAirplane.Departure)
                {
                    if (time - TimeSpan.FromDays(1) > reis.DepartureTime)
                    {
                        DepartureRaitingVM.H24Count -= reis.Count;
                        Last24Tables.Remove(reis);
                    }
                }
            }
        }

        public SettingsModel Settings
        {
            get => _settings;
            set => this.RaiseAndSetIfChanged(ref _settings, value);
        }

        public TimeTableViewModel LastTimeTable
        {
            get { return _lastTimeTable; }
            set => this.RaiseAndSetIfChanged(ref _lastTimeTable, value);
        }

        public void LoadSettings()
        {
            try
            {
                string jsonString = File.ReadAllText("settings.txt");
                Settings = JsonSerializer.Deserialize<SettingsModel>(jsonString);
            }
            catch(FileNotFoundException exception)
            {

                Random r = new Random();
                var count = 100;
                TimeTableModel[] tables = new TimeTableModel[count];
                for (int i = 0; i < count; i++)
                {
                    int fromOrTo = r.Next(AirPorts.Count);
                    TypeAirplane type = r.Next(0, 101) % 2 == 0 ? TypeAirplane.Arrival : TypeAirplane.Departure;
                    TimeTableModel timeTableModel;
                    var airportFromOrTo = AirPorts[fromOrTo];
                    var plane = AirplaneModels[r.Next(AirplaneModels.Count)];
                    DateTime arrivalTime = DateTime.Now + TimeSpan.FromHours(r.Next(5)) + TimeSpan.FromMinutes(r.Next(+61));
                    DateTime DepartmentTime = arrivalTime + airportFromOrTo.Time;
                    timeTableModel = new TimeTableModel(DepartmentTime, arrivalTime, type,
                        type == TypeAirplane.Arrival ? airportFromOrTo : MyAirPort,
                        type == TypeAirplane.Arrival ? MyAirPort : airportFromOrTo, plane);

                    tables[i] = timeTableModel;
                }

                TableModel tableModel = new TableModel(tables);

                Settings = new SettingsModel(tableModel, ImmitationSpeed.X1);
                foreach (var table in Settings.TableModel.TimeTables)
                {
                    AllList.Add(new TimeTableViewModel(table)
                    {
                        Count = r.Next(table.Plane.Capacity / 3, table.Plane.Capacity)
                    });
                }
                Speed = Settings.Speed;
            }
            catch (Exception)
            {
                //do nothing
            }
        }
        public void SaveSettings()
        {
            string jsonString = JsonSerializer.Serialize<SettingsModel>(Settings);
            File.WriteAllText("settings.txt", jsonString);
        }
    }
}
