namespace SaysanPwa.Application.Utilities.Validation;

public static class ObjectValidation
{
    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }
}