public interface ICoin
{
	int Get();
}

public abstract class Coin : IComparable<Coin>, ICoin
{
	private int _value;

	protected Coin(int value) => _value = value;
	public int Get() => _value;
	public int CompareTo(Coin? coin)
	{
		if (coin == null) return 1;
		return -_value.CompareTo(coin.Get());
	}
}

public class IronCoin : Coin
{
	public IronCoin() : base(1) {}

	public override string ToString() => "iron coin";
}


public class BronzeCoin : Coin
{
	public BronzeCoin() : base(10) { }

	public override string ToString() => "bronze coin";
}

public class SilverCoin : Coin
{
	public SilverCoin() : base(100) { }

	public override string ToString() => "silver coin";
}

public class GoldCoin : Coin
{
	public GoldCoin() : base(1000) { }

	public override string ToString() => "gold coin";
}

public class CoinList
{
	private List<Coin> listCoin;

	public CoinList()
	{
		listCoin = new List<Coin>();
		listCoin.Add(new IronCoin());
		listCoin.Add(new BronzeCoin());
		listCoin.Add(new SilverCoin());
		listCoin.Add(new GoldCoin());
		listCoin.Sort();
	}
	public List<Coin> Get() => listCoin;
}
