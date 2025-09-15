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

	public Product(Product product) : this(product._name, product._price, product._quantity) {}

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
