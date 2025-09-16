using System.Text;

public abstract class Role
{
	protected bool isChoosing;


	public abstract void Mode(VendingMachine vm);
	public abstract void ChooseAction(VendingMachine vm);
	public abstract string HelpInfo();

	public Role()
	{
		isChoosing = false;
	}

	protected string WrongCommand() => "wrong command";
}

public class AdminRole : Role
{
	public override string HelpInfo()
	{
		return new StringBuilder()
			.Append("add: to add product\n")
			.Append("exit: to logout\n")
			.Append("?: to help info\n")
			.ToString();
	}

	public override void ChooseAction(VendingMachine vm)
	{
		string line = Console.ReadLine();

		switch (line)
		{
			case "add":
				Product product = ReadProduct(vm);
				vm.GetProductStock().Add(product);
				vm.Print($"was added {product.GetName()}");
				break;
			case "exit":
				isChoosing = false;
				break;
			case "?":
				vm.Print(HelpInfo());
				break;
			default:
				vm.Print(WrongCommand());
				break;
		}
	}

	public override void Mode(VendingMachine vm)
	{
		vm.Print("Admin mode");
		vm.ResetSum();
		StockReplenishment(vm);
	}

	private Product ReadProduct(VendingMachine vm)
	{
		string name, price, quantity;
		vm.Print("you can add product:");

		bool isReading = vm.IsRunningAnd(true);
		do
		{
			vm.Print("input name:");
			name = Console.ReadLine();
			if (Product.ValidateName(name)) isReading = false;
		} while (vm.IsRunningAnd(isReading));

		isReading = vm.IsRunningAnd(true);
		do
		{
			vm.Print("input price:");
			price = Console.ReadLine();
			if (Product.ValidatePrice(price)) isReading = false;
		} while (vm.IsRunningAnd(isReading));

		isReading = vm.IsRunningAnd(true);
		do
		{
			vm.Print("input quantity:");
			quantity = Console.ReadLine();
			if (Product.ValidateQuantity(quantity)) isReading = false;
		} while (vm.IsRunningAnd(isReading));

		return new Product(name, int.Parse(price), int.Parse(quantity));
	}

	private void StockReplenishment(VendingMachine vm)
	{
		isChoosing = true;

		vm.Print("Admin may replenish stock");
		vm.Print(HelpInfo());
		while (vm.IsRunningAnd(isChoosing))
		{
			ChooseAction(vm);
		}
	}
}

public class UserRole : Role
{
	private int balance;

	public UserRole() : base()
	{
		balance = 0;
	}

	public override string HelpInfo()
	{
		return new StringBuilder()
			.Append("\n")
			.Append("buy: to buy\n")
			.Append("iron: to drop iron coin\n")
			.Append("bronze: to drop bronze coin\n")
			.Append("silver: to drop silver coin\n")
			.Append("gold: to drop gold coin\n")
			.Append("list: to see list of products\n")
			.Append("?: to see help info\n")
			.ToString();
	}

	public override void ChooseAction(VendingMachine vm)
	{
		string line = Console.ReadLine();

		switch (line)
		{
			case "buy":
				isChoosing = false;
				break;
			case "iron":
				balance += Add(new IronCoin());
				vm.Print(BalanceInfo(balance));
				break;
			case "bronze":
				balance += Add(new BronzeCoin());
				vm.Print(BalanceInfo(balance));
				break;
			case "silver":
				balance += Add(new SilverCoin());
				vm.Print(BalanceInfo(balance));
				break;
			case "gold":
				balance += Add(new GoldCoin());
				vm.Print(BalanceInfo(balance));
				break;
			case "list":
				vm.Print(vm.GetProductStock().ToString());
				break;
			case "?":
				vm.Print(HelpInfo());
				break;
			default:
				vm.Print(WrongCommand());
				break;
		}
	}

	public override void Mode(VendingMachine vm)
	{
		string name = "";

		name = ChooseProduct(vm);
		BuyProduct(name, vm);
	}

	private int Add(ICoin coin) => coin.Get();
	private string BalanceInfo(int balance) => $"your balance: {balance} iron";

	private string ChooseProduct(VendingMachine vm)
	{
		isChoosing = true;
		string result = "";

		vm.Print("choose product:");
		vm.Print(vm.GetProductStock().ToString());
		while (vm.IsRunningAnd(isChoosing))
		{
			string line = Console.ReadLine();
			Product? product = vm.GetProductStock().TryGet(line);

			if (product != null)
			{
				isChoosing = false;
				result = product.GetName();
			}
			else
			{
				vm.Print($"no such product: {line}");
				vm.Print(vm.GetProductStock().ToString());
			}
		}

		return result;
	}
	private void BuyProduct(string productName, VendingMachine vm)
	{
		isChoosing = true;

		Product? product = vm.GetProductStock().TryGet(productName);
		if (product == null)
		{
			vm.Print($"not have product: {productName}");
			return;
		}

		vm.Print(HelpInfo());
		vm.Print(BalanceInfo(balance));
		while (vm.IsRunningAnd(isChoosing))
		{
			ChooseAction(vm);
		}

		if (balance >= product.GetPrice())
		{
			product.DecQuantity();
			balance -= product.GetPrice();
			vm.AddSum(product.GetPrice());
			vm.Print($"\nit`s your product: {productName}");
			vm.Print($"it`s your change:");
			vm.Print(vm.GetChange(balance));
		}
		else
		{
			vm.Print($"you don`t pay enough money for having {productName}");
			vm.Print(BalanceInfo(balance));
			if (vm.IsRunningAnd(true)) BuyProduct(productName, vm);
		}
	}
}
