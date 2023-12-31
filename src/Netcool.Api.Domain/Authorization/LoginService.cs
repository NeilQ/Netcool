using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netcool.Api.Domain.Users;
using Netcool.Core;
using Netcool.Core.AspNetCore;
using Netcool.Core.Authorization;
using Netcool.Core.Repositories;

namespace Netcool.Api.Domain.Authorization
{
    public class LoginService : ILoginService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserLoginAttempt> _userLoginAttemptRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _jwtOptions;
        private readonly IClientInfoProvider _clientInfoProvider;

        public LoginService(IRepository<User> userRepository,
            IRepository<UserLoginAttempt> userLoginAttemptRepository,
            IMapper mapper,
            IOptions<JwtOptions> jwtOptionsAccessor,
            IClientInfoProvider clientInfoProvider,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _clientInfoProvider = clientInfoProvider;
            _unitOfWork = unitOfWork;
            _userLoginAttemptRepository = userLoginAttemptRepository;
            _jwtOptions = jwtOptionsAccessor.Value;
        }

        public async Task<LoginResult> LoginAsync(LoginInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            input.Name = input.Name.SafeString();
            input.Password = input.Password.SafeString();
            var user = _userRepository.GetAll().AsNoTracking()
                .Include(t => t.UserRoles)
                .ThenInclude(t => t.Role)
                .ThenInclude(t => t.RolePermissions)
                .ThenInclude(t => t.Permission)
                .FirstOrDefault(t =>
                    (t.Name == input.Name || t.Email == input.Name || t.Phone == input.Name) &&
                    t.Password == Encrypt.Md5By32(input.Password));
            await SaveLoginAttempt(input, user);
            if (user == null) throw new UserFriendlyException("用户名或密码错误!");
            if (user.IsActive == false) throw new UserFriendlyException("用户未激活!");


            return CreateLoginResult(user);
        }

        private async Task SaveLoginAttempt(LoginInput input, User user)
        {
            var attempt = new UserLoginAttempt
            {
                Success = user != null && user.IsActive,
                UserId = user?.Id ?? 0,
                LoginName = input.Name,
                ClientIp = _clientInfoProvider.ClientIpAddress,
                ClientName = _clientInfoProvider.ClientName,
                BrowserInfo = _clientInfoProvider.BrowserInfo
            };
            await _userLoginAttemptRepository.InsertAsync(attempt);
            await _unitOfWork.SaveChangesAsync();
        }

        private LoginResult CreateLoginResult(User user)
        {
            if (user == null) throw new ArgumentException(nameof(user));

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.IssuerSigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var now = DateTime.Now;
            var expires = _jwtOptions.ExpiryMinutes > 0 ? now.AddMinutes(_jwtOptions.ExpiryMinutes) : now.AddDays(1);

            var claims = new List<Claim>()
            {
                new Claim(AppClaimTypes.UserId, user.Id.ToString()),
                new Claim(AppClaimTypes.UserName, user.Name)
            };
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(AppClaimTypes.Role, userRole.Role.Name));
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _jwtOptions.ValidAudience,
                Issuer = _jwtOptions.ValidIssuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = expires,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);

            var permissionCodes = user.UserRoles
                .SelectMany(t => t.Role.RolePermissions.Select(rp => rp.Permission))
                .Select(t => t.Code)
                .Distinct()
                .ToList();

            return new LoginResult
            {
                User = _mapper.Map<UserDto>(user),
                AccessToken = tokenHandler.WriteToken(token),
                ExpiryAt = expires,
                PermissionCodes = permissionCodes
            };
        }
    }
}
