using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace projectOOP
{
    public class FileManager
    {
        protected string FilePath;

        public FileManager(string filePath)
        {
            FilePath = filePath;
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
            File.WriteAllLines(FilePath, lines);
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
        public Product Product { get; set; }
        public Client Client { get; set; }
    }

    public class OrderQueueManager : FileManager
    {
        public OrderQueueManager(string basePath) : base(basePath) { }

        public void EnqueueOrder(Order order)
        {
            var orders = ReadFromFile();
            orders.Add($"Order,{order.OrderId},{order.Product.ProductId},{order.Client.ClientId}");
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
                    Product = GetProduct(productId),
                    Client = GetClient(clientId)
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
                    var product = GetProduct(productId);
                    var client = GetClient(clientId);

                    if (product != null && client != null)
                    {
                        var newOrder = new Order
                        {
                            OrderId = orderId,
                            Product = product,
                            Client = client
                        };

                        orderList.Add(newOrder);
                    }
                    else
                    {
                        Console.WriteLine("Error reading order. Cannot find product or customer.");
                    }
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

        public Product GetProduct(int productId)
        {
            var products = new ProductFileManager().ReadFromFile();
            var productDetails = products.FirstOrDefault(p => p.StartsWith($"Product,{productId},"));
            if (productDetails != null)
            {
                var productName = productDetails.Split(',')[2];
                return new Product { ProductId = productId, Name = productName };
            }
            return null;
        }

        public Client GetClient(int clientId)
        {
            var clients = new ClientFileManager().ReadFromFile();
            var clientDetails = clients.FirstOrDefault(c => c.StartsWith($"Client,{clientId},"));
            if (clientDetails != null)
            {
                var clientName = clientDetails.Split(',')[2];
                return new Client { ClientId = clientId, Name = clientName };
            }
            return null;
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
            var basePath = @"D:\Programming\ProgrammingBasics\ProjectsC#\projectOOP_3.0_Matwei\projectOOP_3.0";
            var orderQueueManager = new OrderQueueManager(Path.Combine(basePath, "orders.txt"));

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add Order to Queue");
                Console.WriteLine("2. Process Next Order");
                Console.WriteLine("3. Show Current Orders");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddOrderToQueue(orderQueueManager);
                        break;

                    case "2":
                        ProcessNextOrder(orderQueueManager);
                        break;

                    case "3":
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

        static void AddOrderToQueue(OrderQueueManager orderQueueManager)
        {
            Console.WriteLine("Enter Product ID:");
            if (int.TryParse(Console.ReadLine(), out var productId))
            {
                Console.WriteLine("Enter Client ID:");
                if (int.TryParse(Console.ReadLine(), out var clientId))
                {
                    var product = orderQueueManager.GetProduct(productId);
                    var client = orderQueueManager.GetClient(clientId);

                    if (product != null && client != null)
                    {
                        var order = new Order { OrderId = orderQueueManager.GetNextOrderId(), Product = product, Client = client };
                        orderQueueManager.EnqueueOrder(order);
                        Console.WriteLine("Order added to the queue successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Product ID or Client ID. Order not added to the queue.");
                    }
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
                Console.WriteLine($"Processed Order ID: {nextOrder.OrderId}, Product: {nextOrder.Product.Name}, Client: {nextOrder.Client.Name}");
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
                Console.WriteLine($"Order ID: {currentOrder.OrderId}, Product: {currentOrder.Product.Name}, Client: {currentOrder.Client.Name}");
            }
        }
    }
}


