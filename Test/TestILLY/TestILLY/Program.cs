using System;
using System.Collections.Generic;
using System.Linq;

public class ValidationResult<TSuccess, TError> // generic class with 2 type parameters
{
    // 3 properties:
    public bool IsValid { get; private set; } // indicates whether the validation succeeded or not (Bool - On or Off)
    public TSuccess SuccessResult { get; private set; } // tracks the Succedded Validations
    public List<TError> Failures { get; private set; } // tracks the Failed Validations

    private ValidationResult(bool isValid, TSuccess successResult, List<TError> errors)
    {
        IsValid = isValid;
        SuccessResult = successResult;
        Failures = errors;
    }

    // Pass method to return a new instance of 'ValidationResult' class with IsValid property set to true and an empty list of errors
    public static ValidationResult<TSuccess, TError> Pass(TSuccess successResult)
    {
        return new ValidationResult<TSuccess, TError>(true, successResult, new List<TError>());
    }
    // NotPassed method to return a new instance of 'ValidationResult' class with IsValid property set to false and an empty list of errors
    public static ValidationResult<TSuccess, TError> NotPassed(params TError[] errors)
    {
        return new ValidationResult<TSuccess, TError>(false, default(TSuccess), errors.ToList());
    }
}

public static class Validator // generic class
{
    public static ValidationResult<T, string> Validate<T>(T item, params Func<T, ValidationResult<T, string>>[] validators)
    /* Generic method 'Validate' that takes an item of type T and a variable number of validation functions, each of which accepts an input of type T and returns a ValidationResult object.

     * The method loops through all the validation functions, executes each one, and collects any errors returned and stores it in a list.
     * If there are errors, it returns a ValidationResult object with the list of errors.
     * Else, it returns a ValidationResult object with the original input item.*/
    {
        var errors = new List<string>();

        foreach (var validator in validators)
        {
            var result = validator(item);
            if (!result.IsValid)
            {
                errors.AddRange(result.Failures);
            }
        }

        if (errors.Any())
        {
            return ValidationResult<T, string>.NotPassed(errors.ToArray());
        }
        return ValidationResult<T, string>.Pass(item);
    }
}
