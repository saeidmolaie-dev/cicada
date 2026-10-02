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

	public float Length => MathF.Sqrt(SquaredLength);

	public float SquaredLength => Dot(this);

	public float Distance(Vector other)
		=> MathF.Sqrt(SquaredDistance(other));

	public float SquaredDistance(Vector other)
	{
		Guard.ThrowIfNull(other, nameof(other));

		EnsureSameDimensions(this, other);

		var sum = 0f;

		for (var i = 0; i < Dimensions; i++)
		{
			var difference =
				_components[i] - other[i];

			sum += difference * difference;
		}

		return sum;
	}

	public float Dot(Vector other)
	{
		Guard.ThrowIfNull(other, nameof(other));

		EnsureSameDimensions(this, other);

		var dotProduct = 0f;

		for (var i = 0; i < Dimensions; i++)
			dotProduct += _components[i] * other[i];

		return dotProduct;
	}

	public float CosineSimilarity(Vector other)
	{
		Guard.ThrowIfNull(other, nameof(other));

		EnsureSameDimensions(this, other);

		var denominator =
			MathF.Sqrt(SquaredLength * other.SquaredLength);

		Guard.ThrowIfTrue(
			denominator == 0f,
			"Cannot compute cosine similarity with a zero-length vector");

		var similarity = Dot(other) / denominator;

		return Math.Clamp(
			similarity, min: -1f, max: 1f);
	}

	public Vector Normalize()
	{
		var length = Length;

		Guard.ThrowIfTrue(
			length == 0f, "Cannot normalize a zero-length vector");

		return this / length;
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

	public static Vector operator *(Vector other, float scalar)
		=> Apply(other, scalar, ScalarOperation.Multiplication);

	public static Vector operator *(float scalar, Vector other)
		=> Apply(other, scalar, ScalarOperation.Multiplication);

	public static Vector operator /(Vector other, float scalar)
		=> Apply(other, scalar, ScalarOperation.Division);

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

	private static Vector Apply(Vector other, float scalar, ScalarOperation scalarOperation)
	{
		Guard.ThrowIfNull(other, nameof(other));
		Guard.ThrowIfTrue(
			scalarOperation == ScalarOperation.Division && scalar == 0f,
			"Cannot divide a vector by zero");

		Func<float, float> operation =
			scalarOperation == ScalarOperation.Multiplication
				? component => component * scalar
				: component => component / scalar;

		var result =
			new float[other.Dimensions];

		for (var i = 0; i < result.Length; i++)
			result[i] = operation(other[i]);

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