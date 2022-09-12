using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShareBear.Data;
using ShareBear.Data.Models;
using ShareBear.Data.Requests;
using ShareBear.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShareBear.Services
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(UserAuthenticationRequest payload, string ipAddress);
        Task<List<Users>> GetUsers();
        string IssueDebugJwtToken(string role);
        Task<AuthenticateResponse> RefreshToken(string token, string ipaddress);
        Task<bool> RevokeCookie(string token, string ipAddress);
        Task<bool> RevokeToken(string token, string ipAddress);
    }

    public class AccountService : IAccountService
    {
        private readonly IDbContextFactory<DefaultContext> contextFactory;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;
        private readonly Random r = new();

        public AccountService(IDbContextFactory<DefaultContext> _contextFactory, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            contextFactory = _contextFactory;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(UserAuthenticationRequest payload, string ipAddress)
        {
            using var a = contextFactory.CreateDbContext();

            var user = await a.Users.Include(x => x.RefreshTokens).FirstOrDefaultAsync(x => x.Email == payload.Email);


            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(payload.Password, user.HashedPassword))
                throw new UnauthorizedAccessException("Invalid credentials.");



            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            // On login, remove all active refresh tokens except the new one 
            var activeRF = await a.RefreshTokens.Where(x => x.UsersId == user.Id && x.Revoked == null).ToListAsync();
            activeRF.ForEach(x =>
            {
                x.Revoked = DateTime.UtcNow;
                x.RevokedByIp = ipAddress;
                x.ReplacedByToken = refreshToken.Token;
            });

            user.LastLoggedAt = DateTime.Now;

            user.RefreshTokens.Add(refreshToken);
            a.Update(user);
            await a.SaveChangesAsync();

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public async Task<bool> RevokeCookie(string token, string ipAddress)
        {
            using var a = contextFactory.CreateDbContext();
            var user = a.Users.Include(x => x.RefreshTokens)
                .SingleOrDefault(x => x.RefreshTokens.Any(y => y.Token == token));

            // No user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            // No active refresh tokens
            if (!refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = "";

            a.Update(user);
            await a.SaveChangesAsync();

            return true;
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipaddress)
        {
            using var a = contextFactory.CreateDbContext();

            var rToken = await a.RefreshTokens.Include(x => x.Users).FirstOrDefaultAsync(x => x.Token == token);
            if (rToken == null || !rToken.IsActive)
                throw new UnauthorizedAccessException("Invalid token provided.");


            var newRefreshToken = generateRefreshToken(ipaddress);
            rToken.Revoked = DateTime.UtcNow;
            rToken.RevokedByIp = ipaddress;
            rToken.ReplacedByToken = newRefreshToken.Token;

            rToken.Users.RefreshTokens.Add(newRefreshToken);

            rToken.Users.LastLoggedAt = DateTime.Now;


            a.Update(rToken.Users);
            await a.SaveChangesAsync();


            var jwtToken = generateJwtToken(rToken.Users);

            return new AuthenticateResponse(rToken.Users, jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            using var a = contextFactory.CreateDbContext();

            var rfToken = await a.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

            // return false if no user found with token
            if (rfToken == null) return false;

            // return false if token is not active
            if (!rfToken.IsActive) return false;

            // revoke token and save
            rfToken.Revoked = DateTime.UtcNow;
            rfToken.RevokedByIp = ipAddress;
            a.Update(rfToken);
            await a.SaveChangesAsync();

            return true;
        }

        public async Task<List<Users>> GetUsers()
        {
            using var a = contextFactory.CreateDbContext();
            return await a.Users.ToListAsync();
        }

        public string IssueDebugJwtToken(string role)
        {
            return generateJwtToken(new Users()
            {
                Email = $"TestingUser - {r.Next(0, 10000)}",
                Role = role == "admin" ? "admin" : role == "user" ? "user" : throw new ArgumentException($"Invalid role ${role}"),
                UserGlobalIdentifier = r.Next(0, 10000).ToString()
            });
        }

        private string generateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserGlobalIdentifier),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = appSettings.Issuer,
                Audience = appSettings.Audience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshTokens generateRefreshToken(string ipAddress)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return new RefreshTokens
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
