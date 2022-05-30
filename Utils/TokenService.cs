using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using Microsoft.IdentityModel.Tokens;
using System.Web;
using WebApp.Models;

namespace WebApp.Utils
{
    public static class TokenService
    {
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(Encript.Secret);
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.ID.ToString()),
                    new Claim(ClaimTypes.Email, user.EMAIL)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool TokenOk(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(Encript.Secret);

                var handler = new JwtSecurityTokenHandler();
                var validations = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static string GetId (string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(Encript.Secret);

                var handler = new JwtSecurityTokenHandler();
                var validations = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                return claims.FindFirst(ClaimTypes.Sid).Value;
            }
            catch
            {
                return "";
            }
        }

        public static string GetEmail(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(Encript.Secret);

                var handler = new JwtSecurityTokenHandler();
                var validations = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                return claims.FindFirst(ClaimTypes.Email).Value;
            }
            catch
            {
                return "";
            }
        }

        public static void RefreshSession(User user)
        {
            HttpContext.Current.Session["UserIsAuthenticated"] = true;
            HttpContext.Current.Session["Id"] = user.ID;
            HttpContext.Current.Session["Nome"] = user.NOME;
            HttpContext.Current.Session["Email"] = user.EMAIL;
            HttpContext.Current.Session["Role"] = user.ROLE;
            HttpContext.Current.Session["urlimagem"] = user.URLIMAGEM != "" ? user.URLIMAGEM : "/images/user icon.jpeg";
            HttpContext.Current.Session.Timeout = 2440; // 24 horas.

            var sessionDefault = new User().ValidacaoSession(user.ID);
            HttpContext.Current.Session["master_idUsuario"] = sessionDefault.MASTER_ID;
            HttpContext.Current.Session["master_qtdUsuarios"] = sessionDefault.QTD_USUARIOS;
            HttpContext.Current.Session["master_qtdIdeias"] = sessionDefault.QTD_IDEIAS;
            HttpContext.Current.Session["master_qtdAvaliacoes"] = sessionDefault.QTD_AVALIACOES;
        }
    }
}
