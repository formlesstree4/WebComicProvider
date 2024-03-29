﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WebComicProvider.Interfaces;
using WebComicProviderApi.Managers;

namespace WebComicProviderApi
{
    public static class Extensions
    {

        /// <summary>
        /// Configures the <see cref="AuthenticationBuilder"/> to use the JWT Bearer pattern for security
        /// </summary>
        /// <param name="builder"><see cref="AuthenticationBuilder"/></param>
        /// <param name="issuer">The name of the issuer</param>
        /// <param name="audience">The name of the target audience</param>
        /// <param name="secret">The password secret</param>
        /// <param name="tokenManager"><see cref="UserTokenManager"/></param>
        /// <returns><see cref="AuthenticationBuilder"/></returns>
        public static AuthenticationBuilder AddJwtBearerConfiguration(this AuthenticationBuilder builder, IConfiguration configuration, IUserTokenManager tokenManager)
        {
            return builder.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = new TimeSpan(0, 0, 30),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = CreateSigningKey(configuration["Jwt:Secret"])
                };
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = async (context) =>
                    {
                        var jwtToken = (JwtSecurityToken)context.SecurityToken;

                        var name = jwtToken.GetUsername();
                        var token = jwtToken.RawData;

                        if (name is null)
                        {
                            context.Fail(new Exception("Invalid or Expired Token"));
                            return;
                        }

                        var sessionData = await tokenManager.GetSession(name);
                        if (sessionData is not null && !sessionData.Token.Equals(token))
                        {
                            context.Fail(new Exception("Invalid or Expired Token"));
                        }
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                        // Add some extra context for expired tokens.
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure is SecurityTokenExpiredException authenticationException)
                        {
                            context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                            context.Response.Headers.Add("token-expired", "true");
                            context.ErrorDescription = $"The token expired on {authenticationException.Expires:o}";
                        }

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        }));
                    },
                    //OnAuthenticationFailed = context =>
                    //{
                    //    context.Fail("The access token provided is not valid.");
                    //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //    context.Response.ContentType = "application/json";

                    //    string response =
                    //        JsonSerializer.Serialize("The access token provided is not valid.");
                    //    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    //    {
                    //        context.Response.Headers.Add("Token-Expired", "true");
                    //        response =
                    //            JsonSerializer.Serialize("The access token provided has expired.");
                    //    }

                    //    context.Response.WriteAsync(response);
                    //    return Task.CompletedTask;
                    //}
                };
            });
        }

        /// <summary>
        /// Retrieves the Username from the current <see cref="ClaimsPrincipal"/>
        /// </summary>
        /// <param name="principal"><see cref="ClaimsPrincipal"/></param>
        /// <returns>The Username</returns>
        public static string? GetUsername(this ClaimsPrincipal principal) => principal.FindFirst("Username")?.Value;

        /// <summary>
        /// Retrieves the Username from the current <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="token"><see cref="JwtSecurityToken"/></param>
        /// <returns>The Username</returns>
        public static string? GetUsername(this JwtSecurityToken token) => token.Claims.FirstOrDefault(claim => claim.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value;

        /// <summary>
        /// Retrieves the Email Address from the current <see cref="ClaimsPrincipal"/>
        /// </summary>
        /// <param name="principal"><see cref="ClaimsPrincipal"/></param>
        /// <returns></returns>
        public static string? GetEmail(this ClaimsPrincipal principal) => principal.FindFirst("Email")?.Value;

        /// <summary>
        /// Retrieves the UserId from the current <see cref="ClaimsPrincipal"/>
        /// </summary>
        /// <param name="principal"><see cref="ClaimsPrincipal"/></param>
        /// <returns>The UserId</returns>
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            var rawUserId = principal.FindFirst("UserId")?.Value;
            if (rawUserId is null) return null;
            if (int.TryParse(rawUserId, out var userId))
            {
                return userId;
            }
            return null;
        }

        /// <summary>
        /// Returns the value of the Description attribute if present for a given type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">The source value</param>
        /// <returns></returns>
        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }


        private static SymmetricSecurityKey CreateSigningKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }



        public static Task DeleteAsync(string path) => Task.Run(() => { File.Delete(path); });

        public static Task<FileStream> CreateAsync(string path) => Task.Run(() => File.Create(path));

        public static Task MoveAsync(string sourceFileName, string destFileName) => Task.Run(() => { File.Move(sourceFileName, destFileName); });

        public static byte[] CalculateImageHashBytes(Stream image)
        {
            using var hasher = SHA512.Create();
            return hasher.ComputeHash(image);
        }

        public static string BytesToString(this byte[] b) => Encoding.UTF8.GetString(b);

    }
}
