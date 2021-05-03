using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using FamilijaApi.Models;

namespace FamilijaApi.Utility
{
    public static class PasswordUtility
    {
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }

        public static Password GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            Password hashSalt = new Password { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        public static bool ValidatePassword(string password, out string message){
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if(!hasMiniMaxChars.IsMatch(password)){
                message="Password must contain 8-15 characters";
                return false;
            }
            if(!hasNumber.IsMatch(password)){
                message="Password must contain number";
                return false;
            }
            if(!hasUpperChar.IsMatch(password)){
                message="Password must contain upper char";
                return false;
            }
            if(!hasLowerChar.IsMatch(password)){
                message="Password must contain lower case";
                return false;
            }
            if(!hasSymbols.IsMatch(password)){
                message="Password must contain at least one character";
                return false;
            }
            message="all is okey";
            return true;
        }
    }
}