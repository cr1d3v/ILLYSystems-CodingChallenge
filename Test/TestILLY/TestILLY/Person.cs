using System;

// The consumer of the library, defined in class Program.cs
public class Person
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
}

public static class PersonValidator // defines two validation functions, Validated!
{
    public static ValidationResult<Person, string> ValidatedPerson(Person person)
    {
        return Validator.Validate(person, ValidatedName, ValidatedBirthDate);
    }

    private static ValidationResult<Person, string> ValidatedName(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.Name))
        {
            return ValidationResult<Person, string>.NotPassed("The name, cannot be empty");
        }

        return ValidationResult<Person, string>.Pass(person);
    }

    private static ValidationResult<Person, string> ValidatedBirthDate(Person person)
    {
        if (person.BirthDate > DateTime.Now)
        {
            return ValidationResult<Person, string>.NotPassed("The birth date, cannot be placed in the future");
        }

        return ValidationResult<Person, string>.Pass(person);
    }
}

public class Program
{
    public static void Main()
    {
        // create an instance of the Person class with invalid data and pass it to ValidatedPerson method to call the 2 validation functions + collect the errors that have been returned
        var person = new Person { Name = "", BirthDate = DateTime.Now.AddDays(1) };

        var result = PersonValidator.ValidatedPerson(person);

        if (result.IsValid)
        {
            Console.WriteLine("The Person is valid");
        }
        else
        {
            foreach (var error in result.Failures)
            {
                Console.WriteLine(error);
            }
        }
    }
}
