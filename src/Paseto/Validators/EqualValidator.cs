﻿namespace Paseto.Validators;

using System;
using Internal;

/// <summary>
/// The Equality Validator. This class cannot be inherited.
/// </summary>
/// <seealso cref="Paseto.Validation.BaseValidator" />
public sealed class EqualValidator : BaseValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EqualValidator"/> class.
    /// </summary>
    /// <param name="payload">The payload.</param>
    public EqualValidator(PasetoPayload payload, string claim) : base(payload)
    {
        if (string.IsNullOrWhiteSpace(claim))
            throw new ArgumentNullException(nameof(claim));

        ClaimName = claim;
    }

    /// <summary>
    /// Gets the name of the claim.
    /// </summary>
    /// <value>The name of the claim.</value>
    public override string ClaimName { get; }

    /// <summary>
    /// Validates the payload against the provided optional expected value. Throws an exception if not valid.
    /// </summary>
    /// <param name="expected">The optional expected value.</param>
    /// <exception cref="PasetoTokenValidationException">
    /// Token is not yet valid.
    /// </exception>
    public override void Validate(IComparable expected)
    {
        if (!Payload.TryGetValue(ClaimName, out var value))
            throw new PasetoTokenValidationException($"Claim '{ClaimName}' not found.");

        if (value is IComparable comparable)
        {
            if (Comparer.GetEqualsResult(comparable, expected))
                return;
        }
        else
        {
            if (Equals(value, expected))
                return;
        }

        throw new PasetoTokenValidationException($"Token Claim '{ClaimName}' is not valid.")
        {
            Expected = expected,
            Received = value
        };
    }

    /// <summary>
    /// Validates the payload against the provided optional expected value.
    /// </summary>
    /// <param name="expected">The optional expected value.</param>
    /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
    public override bool IsValid(IComparable expected = null)
    {
        try
        {
            Validate(expected);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
