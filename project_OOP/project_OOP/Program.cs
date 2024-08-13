using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project_OOP
{
    public class Issues
    {
        public string Issue { get; }

        public Issues(string issue)
        {
            Issue = issue;
        }

        public void Alarm()
        {
            Console.WriteLine($"Warning: {Issue}");
        }
    }

    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Person(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}";
        }
    }

    public class Employee : Person
    {
        public string Department { get; }

        public Employee(string id, string name, string department) : base(id, name)
        {
            Department = department;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Department: {Department}";
        }
    }

    public class Device
    {
        public string DeviceName { get; }
        public bool IsOn { get; protected set; }

        public Device(string deviceName)
        {
            DeviceName = deviceName;
            IsOn = true;
        }

        public virtual void TurnOn()
        {
            if (PowerGridControlDevice.IsGridOn)
            {
                Console.WriteLine($"{DeviceName} is turned on.");
                IsOn = true;
            }
            else
            {
                Console.WriteLine($"Power grid is off. Cannot turn on {DeviceName}.");
            }
        }

        public virtual void TurnOff()
        {
            Console.WriteLine($"{DeviceName} is turned off.");
            IsOn = false;
        }
    }

    public class TemperatureControlDevice : Device
    {
        public int Temperature { get; private set; } = 25;

        public TemperatureControlDevice(string deviceName) : base(deviceName)
        {
        }

        public void AdjustTemperature(int desiredTemperature)
        {
            if (IsOn)
            {
                Temperature = desiredTemperature;
                Console.WriteLine($"{DeviceName} is adjusting temperature to {desiredTemperature} degrees.");
            }
            else
            {
                Console.WriteLine($"{DeviceName} is off.");
            }
        }

        public void CurrentTemperature()
        {
            if (IsOn)
            {
                Console.WriteLine($"Temperature is {Temperature}°C");
            }
            else
            {
                Console.WriteLine($"{DeviceName} is off.");
            }
        }
    }

    public class LightingControlDevice : Device
    {
        public LightingControlDevice(string deviceName) : base(deviceName)
        {
        }
    }

    public class PowerGridControlDevice : Device
    {
        public static bool IsGridOn { get; private set; } = true;

        public PowerGridControlDevice(string deviceName) : base(deviceName)
        {
        }

        public override void TurnOn()
        {
            Console.WriteLine($"{DeviceName} is turning on the power grid.");
            IsGridOn = true;
            base.TurnOn();
        }

        public override void TurnOff()
        {
            Console.WriteLine($"{DeviceName} is turning off the power grid.");
            IsGridOn = false;
            base.TurnOff();
        }
    }

    public class CsvDatabase
    {
        private readonly string _filePath;

        public CsvDatabase(string filePath)
        {
            _filePath = filePath;
        }

        public void AddPerson(Person person)
        {
            try
            {
                string data = person is Employee employee
                    ? $"{employee.Id},{employee.Name},{employee.Department}"
                    : $"{person.Id},{person.Name}";

                using (StreamWriter writer = File.AppendText(_filePath))
                {
                    writer.WriteLine(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void DeletePersonById(string id)
        {
            string[] lines = File.ReadAllLines(_filePath);

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 1 && parts[0] != id)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public List<Person> GetAllPeople()
        {
            List<Person> people = new List<Person>();
            try
            {
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 2)
                        {
                            string id = parts[0];
                            string name = parts[1];

                            if (parts.Length >= 3)
                            {
                                string department = parts[2];
                                people.Add(new Employee(id, name, department));
                            }
                            else
                            {
                                people.Add(new Person(id, name));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return people;
        }

        public void UpdatePersonById(string id, Person updatedPerson)
        {
            string[] lines = File.ReadAllLines(_filePath);

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 1 && parts[0] == id)
                    {
                        writer.WriteLine($"{updatedPerson.Id},{updatedPerson.Name}");
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }

    public class Building
    {
        private readonly CsvDatabase _database;
        private readonly string _name;
        private readonly string _address;
        private readonly List<Person> _peopleInside = new List<Person>();
        private bool _isOpen = true;

        public Building(string name, string address, CsvDatabase database)
        {
            _name = name;
            _address = address;
            _database = database;
        }

        public void EnterTheBuilding(Person person)
        {
            _peopleInside.Add(person);
            Console.WriteLine($"{person.Name} entered the building");
            if (!_database.GetAllPeople().Any(p => p.Id == person.Id))
            {
                Console.WriteLine($"{person.Name} is not authorized to enter");
                Issues breakIn = new Issues("Intruder");
                breakIn.Alarm();
            }
        }

        public void WhoIsInBuilding()
        {
            foreach (var person in _peopleInside)
            {
                Console.WriteLine($"{person.Name} is in the building");
            }
        }

        public void ExitTheBuilding(string personId)
        {
            Person personExiting = _peopleInside.FirstOrDefault(person => person.Id == personId);

            if (personExiting != null)
            {
                _peopleInside.Remove(personExiting);
                Console.WriteLine($"{personExiting.Name} exited the building");
            }
            else
            {
                Console.WriteLine($"Person with ID {personId} is not in the building");
            }
        }

        public void CloseGates()
        {
            if (PowerGridControlDevice.IsGridOn)
            {
                Console.WriteLine("Closing the gates");
                _isOpen = false;
            }
            else
            {
                Console.WriteLine("Power is off");
            }
        }

        public void OpenGates()
        {
            if (PowerGridControlDevice.IsGridOn)
            {
                Console.WriteLine("Opening the gates");
                _isOpen = true;
            }
            else
            {
                Console.WriteLine("Power is off");
            }
        }

        public void StartBuildingManagement()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine($"Managing building: {_name}");
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Enter the building");
                Console.WriteLine("2. Exit the building");
                Console.WriteLine("3. View who is currently in the building");
                Console.WriteLine("4. Check if gates are open");
                Console.WriteLine("5. Close gates");
                Console.WriteLine("6. Open gates");
                Console.WriteLine("7. Report issue");
                Console.WriteLine("8. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter ID: ");
                        string id = Console.ReadLine();
                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Is this an employee? (Y/N): ");
                        string isEmployeeInput = Console.ReadLine();
                        if (isEmployeeInput.ToLower() == "y")
                        {
                            Console.Write("Enter Department: ");
                            string department = Console.ReadLine();
                            EnterTheBuilding(new Employee(id, name, department));
                        }
                        else
                        {
                            EnterTheBuilding(new Person(id, name));
                        }
                        break;
                    case "2":
                        Console.Write("Enter ID to delete: ");
                        string deleteId = Console.ReadLine();
                        ExitTheBuilding(deleteId);
                        break;
                    case "3":
                        WhoIsInBuilding();
                        break;
                    case "4":
                        Console.WriteLine(_isOpen ? "Gates are already open." : "Gates are closed.");
                        break;
                    case "5":
                        CloseGates();
                        break;
                    case "6":
                        OpenGates();
                        break;
                    case "7":
                        ReportIssue();
                        break;
                    case "8":
                        running = false;
                        Console.WriteLine("Exiting program.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine();
            }
        }

        public void ReportIssue()
        {
            Console.Write("Enter issue description: ");
            string issueDescription = Console.ReadLine();
            Issues newIssue = new Issues(issueDescription);
            newIssue.Alarm();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            CsvDatabase database = new CsvDatabase("db/person.csv");
            Building apartment = new Building("Apartment Complex 1", "Random Address 123", database);

            bool running = true;
            while (running)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Add Person to database");
                Console.WriteLine("2. Delete Person by ID from database");
                Console.WriteLine("3. Get All People in database");
                Console.WriteLine("4. Update Person by ID in database");
                Console.WriteLine("5. Manage the building");
                Console.WriteLine("6. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter ID: ");
                        string id = Console.ReadLine();
                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Is this an employee? (Y/N): ");
                        string isEmployeeInput = Console.ReadLine();
                        if (isEmployeeInput.ToLower() == "y")
                        {
                            Console.Write("Enter Department: ");
                            string department = Console.ReadLine();
                            database.AddPerson(new Employee(id, name, department));
                        }
                        else
                        {
                            database.AddPerson(new Person(id, name));
                        }
                        break;
                    case "2":
                        Console.Write("Enter ID to delete: ");
                        string deleteId = Console.ReadLine();
                        database.DeletePersonById(deleteId);
                        break;
                    case "3":
                        List<Person> people = database.GetAllPeople();
                        foreach (var person in people)
                        {
                            Console.WriteLine(person);
                        }
                        break;
                    case "4":
                        Console.Write("Enter ID to update: ");
                        string updateId = Console.ReadLine();
                        Console.Write("Enter New Name: ");
                        string newName = Console.ReadLine();
                        database.UpdatePersonById(updateId, new Person(updateId, newName));
                        break;
                    case "5":
                        apartment.StartBuildingManagement();
                        break;
                    case "6":
                        running = false;
                        Console.WriteLine("Exiting program.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}

