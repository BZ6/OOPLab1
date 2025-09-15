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

	public override string ToString() => $"{_name} {_price} {_quantity}";
}
