using System.Collections;
using System.Data.Common;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

public class VendingMachine
{
	private static bool isRunning;
	private ProductStock productStock;
	private CoinList coinList;
	private UserRole user;
	private AdminRole admin;
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
		productStock = new ProductStock();
		coinList = new CoinList();
		user = new UserRole();
		admin = new AdminRole();

		Console.CancelKeyPress += new ConsoleCancelEventHandler(MyCancelHandler); // пока что не понятно куда засунуть
		isRunning = false;

		ResetSum();
	}

	public ProductStock GetProductStock() => productStock;
	public void ResetSum() => sum = 0;
	public void AddSum(int n) => sum += (ulong)n;
	public string GetChange(int balance) => ChangeToString(CalculateChange(balance));
	public bool IsRunningAnd(bool flag) => isRunning && flag;
	public void Print(string msg)
	{
		if (isRunning) Console.WriteLine(msg);
	}
	public void ChooseMode(string? line)
	{
		switch (line)
		{
			case "switch":
				isAdmin = !isAdmin;
				break;
			default:
				break;
		}
	}
	public void Run()
	{
		isRunning = true;

		while (isRunning)
		{
			Print($"current role: {((isAdmin) ? "admin" : "user")}");
			Print("choose mode (write 'switch' to switch role):");
			ChooseMode(Console.ReadLine());

			Role role = (isAdmin) ? admin : user;
			role.Mode(this);
		}
	}

	private Dictionary<Coin, int> CalculateChange(int balance)
	{
		Dictionary<Coin, int> result = new Dictionary<Coin, int>();

		foreach (var item in coinList.Get())
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
	private string ChangeToString(Dictionary<Coin, int> change)
	{
		StringBuilder sb = new StringBuilder();

		foreach (var (key, value) in change)
		{
			sb.AppendFormat("{0} x{1}\n", key.ToString(), value);
		}

		return sb.ToString();
	}
}
