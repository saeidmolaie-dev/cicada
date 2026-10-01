namespace Cicada.Shared;

public static partial class Guard
{
	public static void ThrowIfZero(float value, string parameterName)
	{
		if (value == 0)
			ThrowException(
				$"'{parameterName}' must not be zero");
	}

	public static void ThrowIfZeroOrNegative(float value, string parameterName)
	{
		if (value <= 0)
			ThrowException(
				$"'{parameterName}' must be greater than zero, but was {value}");
	}

	public static void ThrowIfNegative(float value, string parameterName)
	{
		if (value < 0)
			ThrowException($"'{parameterName}' must not be negative, but was {value}");
	}

	public static void ThrowIfOutOfRange(
		float value,
		float min, float max,
		string parameterName)
	{
		if (value < min || value > max)
			ThrowException(
				$"'{parameterName}' must be between {min} and {max} (inclusive), but was {value}");
	}
}