using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace projectOOP
{
    public class FileManager
    {
        protected string FilePath;

        public FileManager(string fileName)
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public List<string> ReadFromFile()
        {
            if (File.Exists(FilePath))
            {
                return File.ReadAllLines(FilePath).ToList();
            }
            return new List<string>();
        }

        public void WriteToFile(List<string> lines)
        {
            string directoryPath = Path.GetDirectoryName(FilePath);
        }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
    }

    public class DataFileManager : FileManager
    {
        public DataFileManager(string basePath) : base(Path.Combine(basePath, "data.txt")) { }
    }

    public class OrderQueueManager : FileManager
    {
        public OrderQueueManager(string basePath) : base(Path.Combine(basePath, "data.txt")) { }

        public void AddProduct(Product product)
        {
            var products = new DataFileManager(FilePath).ReadFromFile();
            products.Add($"Product,{product.ProductId},{product.Name}");
            new DataFileManager(FilePath).WriteToFile(products);
        }

        public void AddClient(Client client)
        {
            var clients = new DataFileManager(FilePath).ReadFromFile();
            clients.Add($"Client,{client.ClientId},{client.Name}");
            new DataFileManager(FilePath).WriteToFile(clients);
        }

        public void AddOrder(Order order)
        {
            try
            {
                var orders = ReadFromFile();
                orders.Add($"Order,{order.OrderId},{order.ProductId},{order.ClientId}");
                WriteToFile(orders);
                Console.WriteLine("Order added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order: {ex.Message}");
            }
        }

        public void EnqueueOrder(Order order)
        {
            var orders = ReadFromFile();
            orders.Add($"Order,{order.OrderId},{order.ProductId},{order.ClientId}");
            WriteToFile(orders);
        }

        public Order DequeueOrder()
        {
            var orders = ReadFromFile();
            if (orders.Any())
            {
                var firstOrder = orders.First();
                orders.RemoveAt(0);
                WriteToFile(orders);

                var orderDetails = firstOrder.Split(',');
                var orderId = int.Parse(orderDetails[1]);
                var productId = int.Parse(orderDetails[2]);
                var clientId = int.Parse(orderDetails[3]);

                return new Order
                {
                    OrderId = orderId,
                    ProductId = productId,
                    ClientId = clientId
                };
            }
            else
            {
                return null;
            }
        }

        public List<Order> GetOrders()
        {
            var orders = ReadFromFile();
            var orderList = new List<Order>();

            foreach (var order in orders)
            {
                var orderDetails = order.Split(',');

                if (orderDetails.Length == 4 &&
                    orderDetails[0] == "Order" &&
                    int.TryParse(orderDetails[1], out int orderId) &&
                    int.TryParse(orderDetails[2], out int productId) &&
                    int.TryParse(orderDetails[3], out int clientId))
                {
                    var newOrder = new Order
                    {
                        OrderId = orderId,
                        ProductId = productId,
                        ClientId = clientId
                    };

                    orderList.Add(newOrder);
                }
                else
                {
                    Console.WriteLine("Error reading order. Incorrect data format.");
                }
            }

            return orderList;
        }

        public int GetNextOrderId()
        {
            var orders = ReadFromFile();
            return orders.Count > 0 ? orders.Max(o => GetOrderIdFromLine(o)) + 1 : 1;
        }

        private int GetOrderIdFromLine(string orderLine)
        {
            var orderDetails = orderLine.Split(',');
            if (orderDetails.Length == 4 &&
                orderDetails[0] == "Order" &&
                int.TryParse(orderDetails[1], out int orderId))
            {
                return orderId;
            }
            return -1;
        }
    }

    public class ProductFileManager : FileManager
    {
        public ProductFileManager() : base("products.txt") { }
    }

    public class ClientFileManager : FileManager
    {
        public ClientFileManager() : base("clients.txt") { }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var basePath = @"D:\Programming\ProgrammingBasics\ProjectsC#\projectOOP_3.0_Oleg\projectOOP";
            var orderQueueManager = new OrderQueueManager(Path.Combine(basePath, "data.txt"));

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Add Client");
                Console.WriteLine("3. Add Order to Queue");
                Console.WriteLine("4. Process Next Order");
                Console.WriteLine("5. Show Current Orders");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct(orderQueueManager);
                        break;

                    case "2":
                        AddClient(orderQueueManager);
                        break;

                    case "3":
                        AddOrderToQueue(orderQueueManager);
                        break;

                    case "4":
                        ProcessNextOrder(orderQueueManager);
                        break;

                    case "5":
                        ShowCurrentOrders(orderQueueManager);
                        break;

                    case "0":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }

        static void AddProduct(OrderQueueManager orderQueueManager)
        {
            Console.WriteLine("Enter Product ID:");
            if (int.TryParse(Console.ReadLine(), out var productId))
            {
                Console.WriteLine("Enter Product Name:");
                var productName = Console.ReadLine();
                var product = new Product { ProductId = productId, Name = productName };
                orderQueueManager.AddProduct(product);
                Console.WriteLine("Product added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid Product ID. Product not added.");
            }
        }

        static void AddClient(OrderQueueManager orderQueueManager)
        {
            Console.WriteLine("Enter Client ID:");
            if (int.TryParse(Console.ReadLine(), out var clientId))
            {
                Console.WriteLine("Enter Client Name:");
                var clientName = Console.ReadLine();
                var client = new Client { ClientId = clientId, Name = clientName };
                orderQueueManager.AddClient(client);
                Console.WriteLine("Client added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid Client ID. Client not added.");
            }
        }

        static void AddOrderToQueue(OrderQueueManager orderQueueManager)
        {
            Console.WriteLine("Enter Product ID:");
            if (int.TryParse(Console.ReadLine(), out var productId))
            {
                Console.WriteLine("Enter Client ID:");
                if (int.TryParse(Console.ReadLine(), out var clientId))
                {
                    var order = new Order { OrderId = orderQueueManager.GetNextOrderId(), ProductId = productId, ClientId = clientId };
                    orderQueueManager.EnqueueOrder(order);
                    Console.WriteLine("Order added to the queue successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid Client ID. Order not added to the queue.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Product ID. Order not added to the queue.");
            }
        }

        static void ProcessNextOrder(OrderQueueManager orderQueueManager)
        {
            var nextOrder = orderQueueManager.DequeueOrder();
            if (nextOrder != null)
            {
                Console.WriteLine($"Processed Order ID: {nextOrder.OrderId}, Product ID: {nextOrder.ProductId}, Client ID: {nextOrder.ClientId}");
            }
            else
            {
                Console.WriteLine("No orders in the queue.");
            }
        }

        static void ShowCurrentOrders(OrderQueueManager orderQueueManager)
        {
            Console.WriteLine("Current Orders:");
            var currentOrders = orderQueueManager.GetOrders();
            foreach (var currentOrder in currentOrders)
            {
                Console.WriteLine($"Order ID: {currentOrder.OrderId}, Product ID: {currentOrder.ProductId}, Client ID: {currentOrder.ClientId}");
            }
        }
    }
}



