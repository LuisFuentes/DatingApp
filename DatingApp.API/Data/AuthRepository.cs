using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            // Function fetches the user by the username from the database.
            // If no user is found or the user's password doesn't match the password entered,
            // returns null. Otherwise returns the User if password is correct.

            // First fetch the user by username
            User user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!IsPasswordHashCorrect(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            // Function accepts a User modal and password.
            // Create a password hash & salt; updating the modal and committing the
            // changes to the database. Returns the updated user modal.

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            // Function returns whether or not if the username exists
            return await _context.Users.AnyAsync(x => x.Username == username);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Function calls the create password with no key given.
            CreatePasswordHash(password, null, out passwordHash, out passwordSalt);
        }

        private void CreatePasswordHash(string password, byte[] key, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Function uses SHA512 to encrypt the passed in password.
            // If a key is given, generates the hash and salt using that key. Otherwise, doesn't use the key,
            // Function Outputs the password hash and salt

            HMACSHA512 encrypt;

            if (key is null)
                encrypt = new HMACSHA512();
            else
                encrypt = new HMACSHA512(key);

            passwordSalt = encrypt.Key;
            passwordHash = encrypt.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool IsPasswordHashCorrect(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Function checks if the password provided matches the User's encrypted password 
            // using the hash & salt.

            byte[] outputPasswordHash, outputPasswordSalt;
            // Get the password hash
            CreatePasswordHash(password, passwordSalt, out outputPasswordHash, out outputPasswordSalt);

            // Check if the password's hash byte array matches the hashed password entered by the user

            if (passwordHash.Length != outputPasswordHash.Length)
                return false;

            for (int i = 0; i < outputPasswordHash.Length; i++)
                if (passwordHash[i] != outputPasswordHash[i])
                    return false;

            // Passwords match
            return true; 

        }

    }
}
