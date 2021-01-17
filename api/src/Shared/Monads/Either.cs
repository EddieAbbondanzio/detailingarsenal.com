using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/// <summary>
/// Either monad for working with two different values.
/// </summary>
/// <typeparam name="TL">The left side type.</typeparam>
/// <typeparam name="TR">The right side type</typeparam>
public class Either<TL, TR> {
    public bool IsLeft => isLeft;

    /// <summary>
    /// Value on the left.
    /// </summary>
    private readonly TL left;

    /// <summary>
    /// Value on the right.
    /// </summary>
    private readonly TR right;

    /// <summary>
    /// If the either holds a left value.
    /// </summary>
    private readonly bool isLeft;

    /// <summary>
    /// Create a new left either.
    /// </summary>
    /// <param name="left">The left value.</param>
    public Either(TL left) {
        this.left = left;
        this.isLeft = true;

        /*
        * Don't like this but it seems the best option if we want to support both nullable
        * reference types, and value types without having to box them.
        */
        this.right = (default(TR))!;
    }

    /// <summary>
    /// Create a new right either.
    /// </summary>
    /// <param name="right">The right value.</param>
    public Either(TR right) {
        this.right = right;
        this.isLeft = false;
        this.left = (default(TL))!;
    }

    [JsonConstructor]
    public Either(TL? left, TR? right) {
        if (left == null && right != null) {
            this.left = default(TL)!;
            this.right = right;
            this.isLeft = true;
        }

        if (right == null && left != null) {
            this.right = default(TR)!;
            this.left = left;
            this.isLeft = true;
        }

        throw new ArgumentException("Both values are null!");
    }

    /// <summary>
    /// Pattern match the stored value of the either.
    /// </summary>
    /// <param name="leftFunc">Handler for the left.</param>
    /// <param name="rightFunc">Handler for the right.</param>
    /// <typeparam name="T">Return value type.</typeparam>
    public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc) => this.isLeft ? leftFunc(this.left) : rightFunc(this.right);

    /// <summary>
    /// Pattern match the store value in async fashing.
    /// </summary>
    /// <param name="leftFunc">Handler for the left.</param>
    /// <param name="rightFunc">Handler for the right.</param>
    /// <typeparam name="T">Return value type.</typeparam>
    public async Task<T> MatchAsync<T>(Func<TL, Task<T>> leftFunc, Func<TR, Task<T>> rightFunc) => this.isLeft ? await leftFunc(this.left) : await rightFunc(this.right);

    public TL Left() => isLeft ? left : throw new InvalidOperationException();
    public TR Right() => !isLeft ? right : throw new InvalidOperationException();

    public override bool Equals(object? obj) {
        return obj is Either<TL, TR> either &&
               IsLeft == either.IsLeft &&
               EqualityComparer<TL>.Default.Equals(left, either.left) &&
               EqualityComparer<TR>.Default.Equals(right, either.right) &&
               isLeft == either.isLeft;
    }

    public override int GetHashCode() {
        return HashCode.Combine(IsLeft, left, right, isLeft);
    }

    public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

    public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
}