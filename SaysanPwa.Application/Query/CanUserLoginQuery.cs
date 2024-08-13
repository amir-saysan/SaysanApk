using MediatR;
using SaysanPwa.Application.Utilities.Cryptography;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query;

public class CanUserLoginQuery : IRequest<SysResult<bool>>
{
    public User User { get; set; } = null!;

    public string Password { get; set; } = string.Empty;


    public CanUserLoginQuery(User user, string password) => (User, Password) = (user, password);
}


public class CanUserLoginQueryHandler : IRequestHandler<CanUserLoginQuery, SysResult<bool>>
{
    public async Task<SysResult<bool>> Handle(CanUserLoginQuery request, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            if (request.User.State_User)
            {
                Encrypt_CLS encrypt_CLS = new();
                string actualPassword = encrypt_CLS.Decrypt_Text(request.User.Password, "09146739");
                if (request.Password == actualPassword)
                {
                    return new(true);
                }
                return new(false, new List<string>() { "رمز عبور اشتباه است." });
            }
            return new SysResult<bool>(false, new List<string>() { "حساب کاربری فعال نیست" });
        });
    }
}

