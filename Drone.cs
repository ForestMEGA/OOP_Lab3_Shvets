using System;

namespace Lab3_OOP
{
    public class Drone
    {
        private string model;
        private double batteryCapacity;
        private double currentBatteryLevel;
        private double maxSpeed;

        public string Status { get; set; } = "Готовий до вильоту";
        public int Id { get; private set; }

        // конструктор без параметрів 
        public Drone()
        {
            CurrentBatteryLevel = 100.0;
        }

        // це головний конструктор з усіма параметрами
        public Drone(int id, string model, double batteryCapacity, double maxSpeed)
        {
            Id = id;
            Model = model;
            BatteryCapacity = batteryCapacity;
            CurrentBatteryLevel = 100.0;
            MaxSpeed = maxSpeed;
        }

        // якщо не вказана швидкість максимальна, вона буде за замовчуванням 60 км в годину
        public Drone(int id, string model, double batteryCapacity) : this(id, model, batteryCapacity, 60.0)
        {
        }

        public string Model
        {
            get { return model; }
            set
            {
                if (value == "") throw new ArgumentException("Назва моделі не може бути порожньою!");
                model = value;
            }
        }

        public double BatteryCapacity
        {
            get { return batteryCapacity; }
            set
            {
                if (value <= 0) throw new ArgumentException("Ємність батареї повинна бути більшою за 0!");
                batteryCapacity = value;
            }
        }

        public double CurrentBatteryLevel
        {
            get { return currentBatteryLevel; }
            set
            {
                if (value < 0 || value > 100) throw new ArgumentException("Заряд має бути від 0 до 100%!");
                currentBatteryLevel = value;
            }
        }

        public double MaxSpeed
        {
            get { return maxSpeed; }
            set
            {
                if (value <= 0) throw new ArgumentException("Максимальна швидкість повинна бути більшою за 0!");
                maxSpeed = value;
            }
        }

        public double RemainingEnergy
        {
            get { return (batteryCapacity * currentBatteryLevel) / 100.0; }
        }

        public string StartFlight(double distance)
        {
            CheckSystem();
            double time = CalculateTime(distance);
            CurrentBatteryLevel = currentBatteryLevel - (distance * 2.5);

            if (currentBatteryLevel < 20) Status = "Потрібна зарядка";

            return $"Політ успішний! Приблизний час у дорозі: {time} хв.";
        }

        // перевантаження методу, щоб дозволити вказати свою швидкість
        public string StartFlight(double distance, double customSpeed)
        {
            if (customSpeed <= 0 || customSpeed > maxSpeed)
                throw new ArgumentException($"Швидкість польоту повинна бути в межах від 1 до {maxSpeed} км/год!");

            CheckSystem();
            double time = (distance / customSpeed) * 60.0;
            CurrentBatteryLevel = currentBatteryLevel - (distance * 3.0);

            if (currentBatteryLevel < 20) Status = "Потрібна зарядка";

            return $"Політ (на кастомній швидкості {customSpeed} км/год) успішний! Час: {time} хв.";
        }

        // політ на певну дистанцію 
        public string StartFlight(double distance, string missionName)
        {
            CheckSystem();
            double time = CalculateTime(distance);
            CurrentBatteryLevel = currentBatteryLevel - (distance * 2.5);

            if (currentBatteryLevel < 20) Status = "Потрібна зарядка";

            return $"Місія '{missionName}' виконана успішно за {time} хв. на дистанції {distance} км.";
        }

        private void CheckSystem()
        {
            if (currentBatteryLevel < 15)
                throw new InvalidOperationException("Неможливо злетіти: критичний рівень заряду (менше 15%)!");
        }

        private double CalculateTime(double distance)
        {
            return (distance / maxSpeed) * 60.0;
        }

        public string GetDetails()
        {
            return $"[ID: {Id}] {model} | Макс. Швидкість: {maxSpeed} км/год | Заряд: {currentBatteryLevel}% ({RemainingEnergy} мАг) | Статус: {Status}";
        }
    }
}
