using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Zadanie3;

using System.Net.Mail;
using System.Text.RegularExpressions;

public class ValidatorUtil
{
    private static readonly string REGEX_FORMATTE = "^(?!\\s*$).+";
    private static readonly string REGEX_NUMBER = "s([0-9]+)$";

    public static bool isStringValid(params string[] names)
    {
        return names.All(name => Regex.Match(name, REGEX_FORMATTE).Success);
    }
    
    public static bool isEmailValid(string email)
    {
        try
        {
            MailAddress m = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool isIndexValid(string index)
    {
        return Regex.Match(index, REGEX_NUMBER).Success;
    }
    
}