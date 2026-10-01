namespace Cicada.Shared;

public static partial class Guard
{
	public static void ThrowIfNull(object? value, string parameterName)
	{
		if (value is null)
			ThrowException(
				$"'{parameterName}' must not be null");
	}

	public static void ThrowIfTrue(bool condition, string message)
	{
		if (condition)
			ThrowException(message);
	}

	private static void ThrowException(string message)
	{
		throw new Exception(message);
	}
}