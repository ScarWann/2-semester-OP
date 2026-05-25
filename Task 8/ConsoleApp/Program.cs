using System;
using System.Collections.Generic;
using System.Linq;
using Task_8.Domain;

namespace Task_8.ConsoleApp
{
    internal class Program
    {
        private static readonly HrEngine HrEngine = new HrEngine();

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            SeedData();

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("====================================================");
                Console.WriteLine("    ВІДДІЛ КАДРІВ: ВЕДЕННЯ ОСОБОВИХ СПРАВ    ");
                Console.WriteLine("====================================================");
                Console.WriteLine("1. Управління робітниками");
                Console.WriteLine("2. Управління підрозділами");
                Console.WriteLine("3. Управління посадами та аналітика");
                Console.WriteLine("4. Пошукова система");
                Console.WriteLine("0. Вихід з програми");
                Console.WriteLine("====================================================");
                Console.Write("Оберіть розділ: ");

                string choice = Console.ReadLine() ?? string.Empty;
                switch (choice)
                {
                    case "1": ExecuteEmployeeMenu(); break;
                    case "2": ExecuteDepartmentMenu(); break;
                    case "3": ExecutePositionMenu(); break;
                    case "4": ExecuteSearchMenu(); break;
                    case "0": running = false; break;
                    default:
                        break;
                }
            }
        }

        private static void SeedData()
        {
            Position dev = new Position("Middle C# Developer", 85000, 160);
            Position pm = new Position("Project Manager", 95000, 150);
            Position qa = new Position("QA Engineer", 50000, 160);
            HrEngine.AllPositions.AddRange(new[] { dev, pm, qa });

            Project alpha = new Project("Project Alpha", 500000m);
            Project beta = new Project("Project Beta", 1200000m);
            HrEngine.AllProjects.AddRange(new[] { alpha, beta });

            Department itDept = new Department("IT-Dev Department");
            Department hrDept = new Department("HR Department");
            HrEngine.AddDepartment(itDept);
            HrEngine.AddDepartment(hrDept);

            Employee emp1 = new Employee("Олексій", "Коваленко", "UA12345", 3, dev);
            Employee emp2 = new Employee("Марія", "Петренко", "UA67890", 1, dev);
            Employee emp3 = new Employee("Іван", "Сидоренко", "UA55555", 5, pm);

            emp1.Projects.Add(alpha);
            emp1.Projects.Add(beta);
            emp2.Projects.Add(alpha);
            emp3.Projects.Add(beta);

            HrEngine.AddEmployee(emp1);
            HrEngine.AddEmployee(emp2);
            HrEngine.AddEmployee(emp3);

            HrEngine.AssignEmployeeToDepartment(emp1, itDept);
            HrEngine.AssignEmployeeToDepartment(emp2, itDept);
            HrEngine.AssignEmployeeToDepartment(emp3, itDept);
        }

        private static void ExecuteEmployeeMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 1. УПРАВЛІННЯ РОБІТНИКАМИ ===");
            Console.WriteLine("1. Додати робітника");
            Console.WriteLine("2. Видалити робітника");
            Console.WriteLine("3. Змінити дані робітника (стаж)");
            Console.WriteLine("4. Переглянути дані конкретного робітника");
            Console.WriteLine("5. Переглянути проекти робітника");
            Console.WriteLine("6. Переглянути список всіх робітників (із сортуванням)");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine() ?? string.Empty;
            switch (choice)
            {
                case "1":
                    Console.Write("Ім'я: "); string fn = Console.ReadLine() ?? string.Empty;
                    Console.Write("Прізвище: "); string ln = Console.ReadLine() ?? string.Empty;
                    Console.Write("Рахунок ЗП: "); string acc = Console.ReadLine() ?? string.Empty;
                    Console.Write("Стаж (років): "); int exp = int.Parse(Console.ReadLine() ?? string.Empty);

                    Employee newEmp = new Employee(fn, ln, acc, exp, HrEngine.AllPositions.First() ?? new Position("Unemployed", 0, 0));
                    HrEngine.AddEmployee(newEmp);
                    Console.WriteLine("Робітника успішно додано.");
                    break;

                case "2":
                    Console.Write("Введіть прізвище робітника для видалення: ");
                    string lnDel = Console.ReadLine() ?? string.Empty;
                    Employee? empDel = HrEngine.AllEmployees.FirstOrDefault(e => e.LastName.Equals(lnDel, StringComparison.OrdinalIgnoreCase));
                    if (empDel != null)
                    {
                        try
                        {
                            HrEngine.DeleteEmployee(empDel);
                            Console.WriteLine("Робітника видалено.");
                        }
                        catch (Exception ex) { Console.WriteLine($"Помилка: {ex.Message}"); }
                    }
                    else
                    {
                        Console.WriteLine("Робітника не знайдено.");
                    }
                    break;

                case "3":
                    Console.Write("Введіть прізвище робітника для редагування: ");
                    string lnEdit = Console.ReadLine() ?? string.Empty;
                    Employee? empEdit = HrEngine.AllEmployees.FirstOrDefault(e => e.LastName.Equals(lnEdit, StringComparison.OrdinalIgnoreCase));
                    if (empEdit != null)
                    {
                        Console.Write("Введіть новий трудовий стаж: ");
                        empEdit.ExperienceYears = int.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Дані змінено.");
                    }
                    break;

                case "4":
                    Console.Write("Введіть прізвище робітника: ");
                    string lnView = Console.ReadLine() ?? string.Empty;
                    Employee? empView = HrEngine.AllEmployees.FirstOrDefault(e => e.LastName.Equals(lnView, StringComparison.OrdinalIgnoreCase));
                    if (empView != null) Console.WriteLine($"\nДані: {empView}");
                    break;

                case "5":
                    Console.Write("Введіть прізвище робітника: ");
                    string lnProj = Console.ReadLine() ?? string.Empty;
                    Employee? empProj = HrEngine.AllEmployees.FirstOrDefault(e => e.LastName.Equals(lnProj, StringComparison.OrdinalIgnoreCase));
                    if (empProj != null)
                    {
                        Console.WriteLine($"Проєкти, у яких бере участь {empProj.FirstName} {empProj.LastName}:");
                        empProj.Projects.ForEach(p => Console.WriteLine($"- {p.Name} ({p.TotalBudget} UAH)"));
                    }
                    break;

                case "6":
                    Console.WriteLine("\nСортувати за: 1 - Іменем, 2 - Прізвищем, 3 - Зарплатою посади");
                    int sortType = int.Parse(Console.ReadLine() ?? string.Empty);
                    var list = HrEngine.GetAllEmployeesSorted(sortType);
                    list.ForEach(e => Console.WriteLine(e));
                    break;
                default:
                    break;
            }
            Console.WriteLine("\nНатисніть Enter для продовження...");
            Console.ReadLine();
        }

        private static void ExecuteDepartmentMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 2. УПРАВЛІННЯ ПІДРОЗДІЛАМИ ===");
            Console.WriteLine("1. Змінити дані підрозділу (Назву)");
            Console.WriteLine("2. Додати підрозділ");
            Console.WriteLine("3. Переглянути дані конкретного підрозділу");
            Console.WriteLine("4. Переглянути список усіх робітників підрозділу (із сортуванням)");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine() ?? string.Empty;
            switch (choice)
            {
                case "1":
                    Console.Write("Введіть стару назву підрозділу: ");
                    string oldName = Console.ReadLine() ?? string.Empty;
                    Department? dEdit = HrEngine.AllDepartments.FirstOrDefault(d => d.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase));
                    if (dEdit != null)
                    {
                        Console.Write("Нова назва: ");
                        dEdit.Name = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Назву змінено.");
                    }
                    break;

                case "2":
                    Console.Write("Назва нового підрозділу: ");
                    HrEngine.AddDepartment(new Department(Console.ReadLine() ?? string.Empty));
                    Console.WriteLine("Підрозділ створено.");
                    break;

                case "3":
                    Console.Write("Введіть назву підрозділу: ");
                    Department? dView = HrEngine.AllDepartments.FirstOrDefault(d => d.Name.Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase));
                    if (dView != null) Console.WriteLine($"Підрозділ: {dView.Name}, Кількість працівників: {dView.Employees.Count}");
                    break;

                case "4":
                    Console.Write("Введіть назву підрозділу: ");
                    Department? dList = HrEngine.AllDepartments.FirstOrDefault(d => d.Name.Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase));
                    if (dList != null)
                    {
                        Console.WriteLine("Сортувати за: 1 - Посадою робітників, 2 - Сумарною вартістю проєктів");
                        int st = int.Parse(Console.ReadLine() ?? string.Empty);
                        var emps = HrEngine.GetDepartmentEmployeesSorted(dList, st);
                        emps.ForEach(e => Console.WriteLine($"{e.LastName} {e.FirstName} | Посада: {e.CurrentPosition?.Title} | Вартість проєктів: {e.GetTotalProjectsCost()}"));
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine("\nНатисніть Enter для продовження...");
            Console.ReadLine();
        }

        private static void ExecutePositionMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 3. УПРАВЛІННЯ ПОСАДАМИ ТА АНАЛІТИКА ===");
            Console.WriteLine("1. Змінити дані посади (Базову ставку)");
            Console.WriteLine("2. Визначити 5 найбільш привабливих посад");
            Console.WriteLine("3. Визначити найбільш прибуткового робітника на посаді");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine() ?? string.Empty;
            switch (choice)
            {
                case "1":
                    Console.Write("Введіть назву посади: ");
                    Position? pos = HrEngine.AllPositions.FirstOrDefault(p => p.Title.Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase));
                    if (pos != null)
                    {
                        Console.Write("Нова ставка: ");
                        pos.BaseSalary = double.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Ставку оновлено.");
                    }
                    break;

                case "2":
                    Console.WriteLine("Топ-5 привабливих посад (за співвідношенням Робочі години / Зарплата):");
                    var tops = HrEngine.GetTop5AttractivePositions();
                    foreach (var p in tops)
                    {
                        Console.WriteLine($"- {p.Title} (Ефективна ставка: {p.BaseSalary / p.WorkingHoursPerMonth:F2} за годину)");
                    }
                    break;

                case "3":
                    Console.Write("Введіть назву посади: ");
                    Position? posProfit = HrEngine.AllPositions.FirstOrDefault(p => p.Title.Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase));
                    if (posProfit != null)
                    {
                        Employee? best = HrEngine.GetMostProfitableEmployeeAtPosition(posProfit);
                        Console.WriteLine(best != null
                            ? $"Найбільш прибутковий співробітник: {best.FirstName} {best.LastName} (Сума проєктів: {best.GetTotalProjectsCost()})"
                            : "На цій посаді немає працівників.");
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine("\nНатисніть Enter для продовження...");
            Console.ReadLine();
        }

        private static void ExecuteSearchMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 4. ПОШУКОВА СИСТЕМА ===");
            Console.WriteLine("1. Пошук по ключовому слову серед робітників");
            Console.WriteLine("2. Пошук по ключовому слову серед проектів");
            Console.WriteLine("3. Наскрізний глобальний пошук по всім даним");
            Console.WriteLine("4. Розширений пошук робітника (Прізвище + Рахунок)");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine() ?? string.Empty;
            Console.Write("Введіть значення для пошуку: ");
            string query = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    var emps = HrEngine.SearchWorkersByKeyword(query);
                    emps.ForEach(e => Console.WriteLine($"Знайдено робітника: {e}"));
                    break;

                case "2":
                    var projs = HrEngine.SearchProjectsByKeyword(query);
                    projs.ForEach(p => Console.WriteLine($"Знайдено проєкт: {p.Name} (Бюджет: {p.TotalBudget})"));
                    break;

                case "3":
                    var global = HrEngine.GlobalSearch(query);
                    foreach (var entity in global)
                    {
                        var str = entity.ToString() ?? string.Empty;
                        Console.WriteLine((str.Length > 24) ? $"{str.Substring(0, 24)}... підходить під критерій."
                                                            : $"{str} підходить під критерій.");
                    }
                    if (global.Count == 0)
                    {
                        Console.WriteLine("Не знайдено нічого, що задольняє критерій.");
                    }
                    break;

                case "4":
                    Console.WriteLine("\n--- Налаштування фільтрів розширеного пошуку ---");
                    Console.WriteLine("(Залиште поле порожнім та натисніть [Enter], щоб пропустити фільтр)\n");

                    Console.Write("Ім'я містить: ");
                    string fName = Console.ReadLine() ?? string.Empty;

                    Console.Write("Прізвище містить: ");
                    string lName = Console.ReadLine() ?? string.Empty;

                    Console.Write("Номер рахунку містить: ");
                    string accNum = Console.ReadLine() ?? string.Empty;

                    Console.Write("Назва посади містить: ");
                    string posTitle = Console.ReadLine() ?? string.Empty;

                    Console.Write("Мінімальний стаж (років): ");
                    string expInput = Console.ReadLine() ?? string.Empty;
                    int? minExp = null;
                    if (int.TryParse(expInput, out int parsedExp))
                    {
                        minExp = parsedExp;
                    }

                    var advancedResults = HrEngine.AdvancedEmployeeSearch(
                        firstName: fName,
                        lastName: lName,
                        accountNumber: accNum,
                        positionTitle: posTitle,
                        minExperience: minExp
                    );

                    Console.WriteLine($"\n[Результат]: Знайдено робітників: {advancedResults.Count}");
                    Console.WriteLine("----------------------------------------------------");
                    foreach (var emp in advancedResults)
                    {
                        Console.WriteLine($"- {emp}");
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine("\nНатисніть Enter для продовження...");
            Console.ReadLine();
        }
    }
}