
public static class NumberString
{
	#region Fields

	private static readonly string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

	#endregion

	#region Methods

	public static string Get(int number)
	{
		return numbers[number];
	}

	#endregion
}
