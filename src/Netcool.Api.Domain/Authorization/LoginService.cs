using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netcool.Api.Domain.Users;
using Netcool.Core;
using Netcool.Core.Authorization;
using Netcool.Core.Helpers;
using Netcool.Core.Repositories;

namespace Netcool.Api.Domain.Authorization
{
    public class LoginService : ILoginService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;

        public LoginService(IRepository<User> userRepository, IMapper mapper, IOptions<JwtOptions> jwtOptionsAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtOptions = jwtOptionsAccessor.Value;
        }

        public LoginResult Login(LoginInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            input.Name = input.Name.SafeString();
            input.Password = input.Password.SafeString();
            var user = _userRepository.GetAll().AsNoTracking()
                .FirstOrDefault(t =>
                    (t.Name == input.Name || t.Email == input.Name || t.Phone == input.Name) &&
                    t.Password == Encrypt.Md5By32(input.Password));
            if (user == null) throw new UserFriendlyException("用户名或密码错误!");

            return CreateLoginResult(user);
        }

        private LoginResult CreateLoginResult(User user)
        {
            if (user == null) throw new ArgumentException(nameof(user));

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var now = DateTime.Now;
            var expires = _jwtOptions.ExpiryMinutes > 0 ? now.AddSeconds(_jwtOptions.ExpiryMinutes) : now.AddDays(1);

            Claim[] claims =
            {
                new Claim(AppClaimTypes.UserId, user.Id.ToString()),
                new Claim(AppClaimTypes.UserName, user.Name)
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = expires
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            return new LoginResult
            {
                User = _mapper.Map<UserDto>(user),
                AccessToken = tokenHandler.WriteToken(token),
                ExpiryAt = expires
            };
        }
    }
}