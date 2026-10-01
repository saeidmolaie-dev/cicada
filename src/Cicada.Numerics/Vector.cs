using Cicada.Shared;

namespace Cicada.Numerics;

public sealed class Vector
{
	private readonly float[] _components;

	public Vector(int dimensions)
	{
		Guard.ThrowIfZeroOrNegative(
			dimensions, nameof(dimensions));

		_components = new float[dimensions];
	}

	public Vector(float[] components)
	{
		Guard.ThrowIfNull(components, nameof(components));
		Guard.ThrowIfZeroOrNegative(components.Length, nameof(components.Length));

		_components = (float[])components.Clone();
	}

	public int Dimensions => _components.Length;

	public float this[int index]
	{
		get
		{
			ValidateIndex(index);
			return _components[index];
		}
		set
		{
			ValidateIndex(index);
			_components[index] = value;
		}
	}

	public static Vector operator +(Vector left, Vector right)
		=> Apply(left, right, (a, b) => a + b);

	public static Vector operator -(Vector left, Vector right)
		=> Apply(left, right, (a, b) => a - b);

	private static Vector Apply(
		Vector left,
		Vector right,
		Func<float, float, float> operation)
	{
		Guard.ThrowIfNull(left, nameof(left));
		Guard.ThrowIfNull(right, nameof(right));

		EnsureSameDimensions(left, right);

		var result =
			new float[left.Dimensions];

		for (var i = 0; i < result.Length; i++)
			result[i] = operation(left[i], right[i]);

		return new Vector(result);
	}

	private static void EnsureSameDimensions(Vector left, Vector right)
	{
		Guard.ThrowIfTrue(
			left.Dimensions != right.Dimensions,
			$"Dimension mismatch: {left.Dimensions} vs {right.Dimensions}");
	}

	private void ValidateIndex(int index)
	{
		Guard.ThrowIfOutOfRange(
			index, 0, Dimensions - 1, nameof(index));
	}
}