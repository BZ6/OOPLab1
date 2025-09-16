using System.Text;

public class Product
{
	private string _name;
	private int _price;
	private int _quantity;

	public Product(string name, int price, int quantity)
	{
		_name = name;
		_price = price;
		_quantity = quantity;
	}

	public Product(Product product) : this(product._name, product._price, product._quantity) { }

	public string GetName() => _name;
	public int GetQuantity() => _quantity;
	public int GetPrice() => _price;
	public void DecQuantity() => _quantity--;
	public void AddQuantity(int value) => _quantity += value;

	public static bool ValidateName(string name) => !string.IsNullOrEmpty(name);
	public static bool ValidatePrice(string price)
	{
		int _res;
		return int.TryParse(price, out _res) && _res > 0;
	}
	public static bool ValidateQuantity(string quantity)
	{
		int _res;
		return int.TryParse(quantity, out _res) && _res > 0;
	}

	public override string ToString() => $"{_name} {_price} {_quantity}";
}

public class ProductStock
{
	private Dictionary<string, Product> stock;

	public ProductStock()
	{
		stock = new Dictionary<string, Product>();

		DefaultStock();
	}

	public Product? TryGet(string name)
	{
		if (stock.Count <= 0) return null;
		if (!stock.ContainsKey(name)) return null;

		Product product = stock[name];

		if (product.GetQuantity() <= 0) return null;
		return product;
	}
	public void Add(Product product)
	{
		string name = product.GetName();
		Product? currentProduct = TryGet(name);

		if (currentProduct == null)
		{
			stock.Add(name, product);
		}
		else
		{
			stock[name].AddQuantity(product.GetQuantity());
		}
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append("name price quantity\n");
		foreach (var (key, value) in stock)
		{
			sb.Append(value);
			sb.Append("\n");
		}

		return sb.ToString();
	}

	private void DefaultStock()
	{
		Add(new Product("bear", 2, 30));
		Add(new Product("vodka", 1, 3));
	}
}
