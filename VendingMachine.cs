using System.Runtime.CompilerServices;
using System.Text;

public class VendingMachine
{
	static private bool isRunning;
	private List<Coin> listCoin = new List<Coin>();
	private Dictionary<string, Product> stock = new Dictionary<string, Product>();
	private bool isPaying;
	private bool isChoosing;
	private int balance;
	bool isAdmin;
	ulong sum;

	protected static void MyCancelHandler(object sender, ConsoleCancelEventArgs args)
	{
		Console.WriteLine("\nCtrl+C pressed! Performing cleanup...");
		Console.WriteLine("Press enter to exit...");
		args.Cancel = true;
		isRunning = false;
	}

	public VendingMachine()
	{
		Console.CancelKeyPress += new ConsoleCancelEventHandler(MyCancelHandler); // пока что не понятно куда засунуть
		isRunning = false;

		ResetSum();
		balance = 0;
		isPaying = false;
		isChoosing = false;
		isAdmin = false;

		listCoin.Add(new IronCoin());
		listCoin.Add(new BronzeCoin());
		listCoin.Add(new SilverCoin());
		listCoin.Add(new GoldCoin());
		listCoin.Sort();

		Add(new Product("bear", 2, 30));
		Add(new Product("vodka", 1, 3));
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

	public int Add(ICoin coin) => coin.Get();

	public void ResetSum()
	{
		sum = 0;
	}

	public string ProductsInfo()
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

	public string BalanceInfo(int balance)
	{
		return $"your balance: {balance} iron";
	}

	private void Print(string msg)
	{
		if (isRunning) Console.WriteLine(msg);
	}

	public string Info()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append("\n");
		sb.Append("buy: to buy\n");
		sb.Append("iron: to drop iron coin\n");
		sb.Append("bronze: to drop bronze coin\n");
		sb.Append("silver: to drop silver coin\n");
		sb.Append("gold: to drop gold coin\n");
		sb.Append("list: to see list of products\n");
		sb.Append("?: to see help info\n");

		return sb.ToString();
	}

	public Dictionary<Coin, int> CalculateChange()
	{
		Dictionary<Coin, int> result = new Dictionary<Coin, int>();

		foreach (var item in listCoin)
		{
			int count = 0;

			while (balance >= item.Get())
			{
				count++;
				balance -= item.Get();
			}

			result.Add(item, count);
		}

		return result;
	}

	public string ChangeToString(Dictionary<Coin, int> change)
	{
		StringBuilder sb = new StringBuilder();

		foreach (var (key, value) in change)
		{
			sb.AppendFormat("{0} x{1}\n", key.ToString(), value);
		}

		return sb.ToString();
	}

	public void ChooseBuyingAction(string? line)
	{
		switch (line)
		{
			case "buy":
				isPaying = false;
				break;
			case "iron":
				balance += Add(new IronCoin());
				Print(BalanceInfo(balance));
				break;
			case "bronze":
				balance += Add(new BronzeCoin());
				Print(BalanceInfo(balance));
				break;
			case "silver":
				balance += Add(new SilverCoin());
				Print(BalanceInfo(balance));
				break;
			case "gold":
				balance += Add(new GoldCoin());
				Print(BalanceInfo(balance));
				break;
			case "list":
				Print(ProductsInfo());
				break;
			case "?":
				Print(Info());
				break;
			default:
				Print("wrong command");
				break;
		}
	}

	public void BuyProduct(string productName)
	{
		isPaying = true;

		Product? product = TryGet(productName);
		if (product == null)
		{
			Print($"not have product: {productName}");
			return;
		}

		Print(Info());
		Print(BalanceInfo(balance));
		while (isPaying && isRunning)
		{
			ChooseBuyingAction(Console.ReadLine());
		}

		if (balance >= product.GetPrice())
		{
			product.DecQuantity();
			balance -= product.GetPrice();
			sum += (ulong)product.GetPrice();
			Print($"\nit`s your product: {productName}");
			Print($"it`s your change:");
			Print(ChangeToString(CalculateChange()));
		}
		else
		{
			Print($"you don`t pay enough money for having {productName}");
			Print(BalanceInfo(balance));
			if (isRunning) BuyProduct(productName);
		}
	}

	public string ChooseProduct()
	{
		isChoosing = true;
		string result = "";

		Print("choose product:");
		Print(ProductsInfo());
		while (isChoosing && isRunning)
		{
			string line = Console.ReadLine();
			Product? product = TryGet(line);

			if (product != null)
			{
				isChoosing = false;
				result = product.GetName();
			}
			else
			{
				Print($"no such product: {line}");
				Print(ProductsInfo());
			}
		}

		return result;
	}

	public void UserMode()
	{
		string name = "";

		name = ChooseProduct();
		BuyProduct(name);
	}

	public void AdminMode()
	{
		Print("Admin mode");
	}

	public void ChooseMode(string? line)
	{
		switch (line)
		{
			case "admin":
				isAdmin = true;
				break;
			default:
				isAdmin = false;
				break;
		}
	}

	public void Run()
	{
		isRunning = true;

		while (isRunning)
		{
			Print("choose mode (write admin to login admin mode):");
			ChooseMode(Console.ReadLine());

			if (!isAdmin)
			{
				UserMode();
			}
			else
			{
				AdminMode();
			}
		}
	}
}
