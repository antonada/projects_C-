using System;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public Supplier Supplier { get; set; }
}

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
}

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
}

public class InventoryManager
{
    private List<Product> products;
    private List<Category> categories;
    private List<Supplier> suppliers;

    public InventoryManager()
    {
        products = new List<Product>();
        categories = new List<Category>();
        suppliers = new List<Supplier>();
    }

    public void AddProduct(string productName, string categoryName, string supplierName)
    {
        var category = GetOrCreateCategory(categoryName);
        var supplier = GetOrCreateSupplier(supplierName);

        var product = new Product
        {
            ProductId = products.Count + 1,
            Name = productName,
            Category = category,
            Supplier = supplier
        };

        products.Add(product);
        Console.WriteLine($"Product '{productName}' added to inventory.");
    }

    public List<Product> SearchProducts(string searchTerm)
    {
        return products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                    p.Category.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                       .ToList();
    }

    public void UpdateProduct(int productId, string newName, string newCategory, string newSupplier)
    {
        var product = products.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            product.Name = newName;
            product.Category = GetOrCreateCategory(newCategory);
            product.Supplier = GetOrCreateSupplier(newSupplier);

            Console.WriteLine($"Product information updated for product ID {productId}.");
        }
        else
        {
            Console.WriteLine($"Product with ID {productId} not found.");
        }
    }

    public void RemoveProduct(int productId)
    {
        var product = products.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            products.Remove(product);
            Console.WriteLine($"Product '{product.Name}' removed from inventory.");
        }
        else
        {
            Console.WriteLine($"Product with ID {productId} not found.");
        }
    }

    private Category GetOrCreateCategory(string categoryName)
    {
        var category = categories.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
        if (category == null)
        {
            category = new Category
            {
                CategoryId = categories.Count + 1,
                Name = categoryName
            };
            categories.Add(category);
        }
        return category;
    }

    private Supplier GetOrCreateSupplier(string supplierName)
    {
        var supplier = suppliers.FirstOrDefault(s => s.Name.Equals(supplierName, StringComparison.OrdinalIgnoreCase));
        if (supplier == null)
        {
            supplier = new Supplier
            {
                SupplierId = suppliers.Count + 1,
                Name = supplierName
            };
            suppliers.Add(supplier);
        }
        return supplier;
    }
}

class Program
{
    static void Main()
    {
        var inventoryManager = new InventoryManager();

        inventoryManager.AddProduct("Laptop", "Electronics", "Tech Supplier");
        inventoryManager.AddProduct("Chair", "Furniture", "Furniture Supplier");

        Console.WriteLine("Search results for 'Chair':");
        var searchResults = inventoryManager.SearchProducts("Chair");
        PrintProducts(searchResults);

        inventoryManager.UpdateProduct(1, "Updated Laptop", "Electronics", "New Tech Supplier");

        Console.WriteLine("Search results for 'Updated Laptop':");
        searchResults = inventoryManager.SearchProducts("Updated Laptop");
        PrintProducts(searchResults);

        inventoryManager.RemoveProduct(2);

        Console.WriteLine("Search results after removing 'Chair':");
        searchResults = inventoryManager.SearchProducts("Chair");
        PrintProducts(searchResults);
    }

    static void PrintProducts(List<Product> products)
    {
        foreach (var product in products)
        {
            Console.WriteLine($"Product ID: {product.ProductId}, Name: {product.Name}, Category: {product.Category.Name}, Supplier: {product.Supplier.Name}");
        }
    }
}

