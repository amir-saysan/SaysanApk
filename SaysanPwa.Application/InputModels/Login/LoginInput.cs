using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.InputModels.Login;

public record LoginInput
{
    [Required(ErrorMessage = "نام کاربری نمیتواند خالی باشد")]
    public string Username { get; init; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور نمیتواند خالی باشد"),
        DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    public bool RememberMe { get; init; } = false;
}
