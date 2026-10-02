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

	public float SquaredLength => Dot(this);

	public float Length => MathF.Sqrt(SquaredLength);

	public float Dot(Vector vector)
	{
		Guard.ThrowIfNull(vector, nameof(vector));

		EnsureSameDimensions(this, vector);

		var dotProduct = 0f;

		for (var i = 0; i < Dimensions; i++)
			dotProduct += _components[i] * vector[i];

		return dotProduct;
	}

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

	public static Vector operator *(Vector vector, float scalar)
		=> Apply(vector, scalar, ScalarOperation.Multiplication);

	public static Vector operator *(float scalar, Vector vector)
		=> Apply(vector, scalar, ScalarOperation.Multiplication);

	public static Vector operator /(Vector vector, float scalar)
		=> Apply(vector, scalar, ScalarOperation.Division);

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

	private static Vector Apply(Vector vector, float scalar, ScalarOperation scalarOperation)
	{
		Guard.ThrowIfNull(vector, nameof(vector));
		Guard.ThrowIfZeroOrNegative(scalar, nameof(scalar));

		Guard.ThrowIfTrue(
			scalarOperation == ScalarOperation.Division && scalar == 0,
			"Cannot divide a vector by zero");

		Func<float, float> operation =
			scalarOperation == ScalarOperation.Division
				? component => component * scalar
				: component => component / scalar;

		var result =
			new float[vector.Dimensions];

		for (var i = 0; i < result.Length; i++)
			result[i] = operation(vector[i]);

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

	private enum ScalarOperation
	{
		Multiplication,
		Division
	}
}