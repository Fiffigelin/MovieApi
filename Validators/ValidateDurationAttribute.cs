using System;
using System.ComponentModel.DataAnnotations;

/** 
* https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute?view=net-11.0
* https://devblogs.dewiride.com/blog/how-to-write-a-custom-validation-attribute-in-dotnet-csharp#step-3-write-the-rule
*/
public class ValidMovieDurationAttribute : ValidationAttribute
{
  public override bool IsValid(object? value)
  {
    if (value is not TimeSpan duration)
      return false;

    return duration > TimeSpan.Zero &&
           duration <= TimeSpan.FromHours(6);
  }

  public override string FormatErrorMessage(string name)
  {
    return $"{name} must be greater than 0 and at most 6 hours.";
  }
}