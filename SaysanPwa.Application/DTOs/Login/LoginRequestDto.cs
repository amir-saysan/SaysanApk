using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.DTOs.Login;

public record LoginRequestDto(
    [Required(ErrorMessage = "نام کاربری نمیتواند خالی باشد"), 
    MaxLength(50, ErrorMessage = "نام کاربری نمیتواند بیشتر از 50 حرف باشد"),
    MinLength(3, ErrorMessage = "نام کاربری نمیتواند کمتر از 3 حرف باشد")]
    string Username,


    [Required(ErrorMessage = "رمز عبور نمیتواند خالی باشد"),
    MaxLength(50, ErrorMessage = "رمز عبور نمیتواند بیشتر از 50 حرف باشد"),
    MinLength(1, ErrorMessage = "رمز عبور نمیتواند کمتر از 1 حرف باشد")]
    string Password);
