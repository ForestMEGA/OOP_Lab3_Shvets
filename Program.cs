using Lab3_OOP;
using System;
using System.Collections.Generic;

namespace Lab3_OOP
{
    class Program
    {
        private static DroneManager manager = new DroneManager();
        private static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // мої обєкти які я додав
            manager.AddDroneDirectly(new Drone(manager.GetNextId(), "DJI Mavic 3", 5000, 70));
            manager.AddDroneDirectly(new Drone(manager.GetNextId(), "FPV Bomber", 4500, 130));

            while (true)
            {
                Console.WriteLine("\n МЕНЮ КЕРУВАННЯ ДРОНАМИ ");
                Console.WriteLine("1 - Додати об'єкт");
                Console.WriteLine("2 - Переглянути всі об'єкти");
                Console.WriteLine("3 - Знайти об'єкт");
                Console.WriteLine("4 - Продемонструвати поведінку");
                Console.WriteLine("5 - Видалити об'єкт");
                Console.WriteLine("0 - Вийти з програми");
                Console.WriteLine("Виберіть дію:");

                string choice = Console.ReadLine();

                if (choice == "1") CreateDroneMenu();
                else if (choice == "2") ShowAllMenu();
                else if (choice == "3") FindMenu();
                else if (choice == "4") FlightMenu();
                else if (choice == "5") DeleteMenu();
                else if (choice == "0") { Console.WriteLine("Вихід з програми..."); break; }
                else Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
            }
        }

        static void CreateDroneMenu()
        {
            try
            {
                Console.WriteLine("Введіть модель дрона:");
                string name = Console.ReadLine();

                Console.WriteLine("Введіть ємність батареї (мАг):");
                double cap = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Введіть максимальну швидкість (км/год):");
                double speed = Convert.ToDouble(Console.ReadLine());

                int nextId = manager.GetNextId();
                Drone newDrone = null;

                // випадковий вибір конструктора 1, 2, чи 3
                int constructorType = rand.Next(1, 4);

                if (constructorType == 1)
                {
                    // виклик без параметрів
                    newDrone = new Drone()
                    {
                        Model = name,
                        BatteryCapacity = cap,
                        MaxSpeed = speed
                    };
                    // оскільки конструктор без параметрів не ставить Id, присво його через рефлексію або метод
                    newDrone.Model = name;
                    newDrone.BatteryCapacity = cap;
                    newDrone.MaxSpeed = speed;

                    Console.WriteLine("\n[ІНФО]: Спрацював КОНСТРУКТОР (без параметрів + ініціалізатори).");
                }
                else if (constructorType == 2)
                {
                    // це другий, але головний конструктур с параметрами усіма
                    newDrone = new Drone(nextId, name, cap, speed);
                    Console.WriteLine("\n[ІНФО]: Спрацював КОНСТРУКТОР (з усіма параметрами).");
                }
                else if (constructorType == 3)
                {
                    // конструктор виклик іншого конструктора, швикість 60км в годину
                    newDrone = new Drone(nextId, name, cap);
                    Console.WriteLine("\n[ІНФО]: Спрацював КОНСТРУКТОР (виклик іншого через 'this', за замовчуванням");
                }

                manager.AddDroneDirectly(newDrone);
                Console.WriteLine("Дрон успішно додано до системи!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка додавання: {ex.Message}");
            }
        }

        static void ShowAllMenu()
        {
            List<Drone> all = manager.GetList();
            if (all.Count == 0) { Console.WriteLine("Список об'єктів порожній."); return; }
            for (int i = 0; i < all.Count; i++) Console.WriteLine(all[i].GetDetails());
        }

        static void FindMenu()
        {
            Console.WriteLine("Введіть ID дрона для пошуку:");
            int id = Convert.ToInt32(Console.ReadLine());
            Drone found = manager.FindById(id);
            if (found != null) Console.WriteLine(found.GetDetails());
            else Console.WriteLine("Дрон із таким ID не знайдений.");
        }

        static void FlightMenu()
        {
            try
            {
                Console.WriteLine("Введіть ID дрона:");
                int id = Convert.ToInt32(Console.ReadLine());

                Drone activeDrone = manager.FindById(id);
                if (activeDrone == null) { Console.WriteLine("Дрон не знайдений!"); return; }

                Console.WriteLine("Введіть дистанцію польоту (км):");
                double dist = Convert.ToDouble(Console.ReadLine());

                // перевантаження методів
                Console.WriteLine("\n  Оберіть версію методу польоту для демонстрації ");
                Console.WriteLine("1 - Стандартний політ (базовий метод з одним параметром)");
                Console.WriteLine("2 - Політ з власною швидкістю (метод з двома параметрами: дистанція + швидкість)");
                Console.WriteLine("3 - Місія з коментарем (метод з двома параметрами: дистанція + назва місії)");
                string methodChoice = Console.ReadLine();

                string result = "";
                if (methodChoice == "1")
                {
                    result = activeDrone.StartFlight(dist);
                }
                else if (methodChoice == "2")
                {
                    Console.WriteLine("Введіть бажану швидкість польоту (км/год):");
                    double customSpeed = Convert.ToDouble(Console.ReadLine());
                    result = activeDrone.StartFlight(dist, customSpeed);
                }
                else if (methodChoice == "3")
                {
                    Console.WriteLine("Введіть назву спеціальної місії:");
                    string mission = Console.ReadLine();
                    result = activeDrone.StartFlight(dist, mission);
                }
                else
                {
                    Console.WriteLine("Неправильний вибір методу. Виконано стандартний політ.");
                    result = activeDrone.StartFlight(dist);
                }

                Console.WriteLine($"\nРезультат: {result}");
                Console.WriteLine($"Стан об'єкта після дії: {activeDrone.GetDetails()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час демонстрації поведінки: {ex.Message}");
            }
        }

        static void DeleteMenu()
        {
            Console.WriteLine("Введіть ID дрона для видалення:");
            int id = Convert.ToInt32(Console.ReadLine());
            if (manager.RemoveDrone(id)) Console.WriteLine("Об'єкт успішно видалено.");
            else Console.WriteLine("Не вдалося знайти дрон.");
        }
    }
}
