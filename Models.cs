using System;
using System.ComponentModel;

namespace Airports
{
    public class AirPort
    {
        /// <summary>
        /// Имя аэропорта
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Город аэропорта
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Время в пути
        /// </summary>
        public TimeSpan Time { get; set; }

        public AirPort(string name, string city, TimeSpan time)
        {
            Name = name;
            City = city;
            Time = time;
        }
    }

    public enum TypeAirplane
    {
        /// <summary>
        /// Вылет
        /// </summary>
        Departure,
        /// <summary>
        /// Прибытие
        /// </summary>
        Arrival
    }
    public enum ImmitationSpeed
    {
        [Description("X1")]
        X1 = 1,
        [Description("X10")]
        X10 = 10,
        [Description("X100")]
        X100 = 100,
        [Description("X1000")]
        X1000 = 1000,
        [Description("X10000")]
        X10000 = 10000
    }
    public class AirplaneModel
    {
        /// <summary>
        /// Вместимость самолета
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Название самолета
        /// </summary>
        public string Name { get; set; }

        public AirplaneModel(int capacity, string name)
        {
            Capacity = capacity;
            Name = name;
        }
    }

    public class TimeTableModel
    {
        /// <summary>
        /// Время вылета самолета
        /// </summary>
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// Время прилета самолета
        /// </summary>
        public DateTime ArrivalTime { get; set; }
        /// <summary>
        ///  Тип самолета
        /// </summary>
        public TypeAirplane Type { get; set; }
        /// <summary>
        /// Откуда
        /// </summary>
        public AirPort From { get; set; }
        /// <summary>
        /// Куда
        /// </summary>
        public AirPort To { get; set; }
        /// <summary>
        /// Самолет
        /// </summary>
        public AirplaneModel Plane { get; set; }
        

        public TimeTableModel(DateTime departureTime, DateTime arrivalTime, TypeAirplane type, AirPort @from, AirPort to, AirplaneModel plane)
        {
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Type = type;
            From = @from;
            To = to;
            Plane = plane;
        }
    }

    public class TableModel
    {
        /// <summary>
        /// Список рейсов
        /// </summary>
        public TimeTableModel[] TimeTables { get; set; }

        public TableModel(TimeTableModel[] timeTables)
        {
            TimeTables = timeTables;
        }
    }

    public class SettingsModel
    {
        public TableModel TableModel { get; set; }
        public ImmitationSpeed Speed { get; set; }

        public SettingsModel(TableModel tableModel, ImmitationSpeed speed)
        {
            TableModel = tableModel;
            Speed = speed;
        }
    }
}
