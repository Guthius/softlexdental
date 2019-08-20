using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OpenDentBusiness
{
    public class Authentication
    {
        /// <summary>
        /// Returns the hashed password from the parameters passed in formatted like "HashType$Salt$Hash". Will always include a salt.
        /// The hash generated will utilize the hashType passed in.  Throws an exception if a passed in hash type is not implimented.
        /// </summary>
        public static string GeneratePasswordHash(string inputPass, HashTypes hashType) =>
            GenerateLoginDetails(inputPass, hashType).ToString();

        /// <summary>
        /// Returns a Password container with the hashedPass.  Will always include a salt.  Generates it using the passed in hashType.
        /// Throws an exception if a passed in hash type is not implimented.
        /// </summary>
        public static PasswordContainer GenerateLoginDetails(string inputPass, HashTypes hashType)
        {
            // Always generate a salt because this should be used for passwords, which shuold always have salt.
            string salt = GenerateSalt(hashType);

            // Use salt to generate new hash.
            string passNew = GetHash(inputPass, salt, hashType);
            return new PasswordContainer(hashType, salt, passNew);
        }

        /// <summary>
        /// Checks to see if the hash of inputPass matches for a Userod object.  If the user password is blank, inputPass must be 
        /// blank as well.  When isEcw is true then inputPass should already be hashed. If the inputPass is correct, the algorithm 
        /// used was MD5, and updateIfNeeded is true then the password stored in the database will be updated to SHA3-512
        /// </summary>
        public static bool CheckPassword(User userod, string inputPass, bool isEcw = false)
        {
            PasswordContainer loginDetails = userod.LoginDetails;
            if (loginDetails.HashType == HashTypes.None)
            {
                return inputPass == "";
            }

            if (isEcw) return ConstantEquals(inputPass, loginDetails.Hash);
            
            if (!CheckPassword(inputPass, loginDetails))
            {
                return false;
            }

            //The password passed in was correct.
            return true;
        }

        /// <summary>
        /// Checks a password against the UserWeb object.  Retruns true if the inputPass is correct.
        /// Automatically upgrades the password to SHA3-512 if it isn't already hashed as such.
        /// </summary>
        public static bool CheckPassword(UserWeb userWeb, string inputPass)
        {
            string hashedPass = userWeb.PasswordHash;
            if (hashedPass == "")
            {
                return inputPass == "";
            }

            bool result = CheckPassword(inputPass, userWeb.LoginDetails);
            if (!result)
            {
                return false;
            }

            if (userWeb.LoginDetails.HashType != HashTypes.SHA3_512)
            {
                // Force update to SHA3-512.
                UpdatePasswordUserWeb(userWeb, inputPass, HashTypes.SHA3_512);
            }
            return true;
        }

        /// <summary>
        /// Trys to find the correct algorithm and compares the hashes. 
        /// If the passHash is blank the inputPass should be too.
        /// The salt is only used when using SHA3-512, not MD5 or MD5_ECW.
        /// </summary>
        public static bool CheckPassword(string inputPass, string salt, string passHash, bool isEcw = false)
        {
            if (salt == null)
            {
                salt = "";
            }

            if (passHash == "")
            {
                return inputPass == "";
            }

            // ECW pre-hashes passwords because they use an ascii encoding instead of unicode like we do.
            if (isEcw) return ConstantEquals(inputPass, passHash);
            
            // MD5 hashed are 128 bits or 22 chars in base-64.  Sha3-512 is 512 bits or 86 chars in base-64.
            // Passwords are stored in base-64, where each byte represents 6 bits of data, with '=' used for padding.
            // Therefore, we can figure out which algorithm to use by looking at the length of the hash.
            // This way of finding the hash algorithm won't work if another algorithm is added that has a output hash of the same length.
            string hashedInputPass;
            if (passHash.Length == 24)
            {
                // It's non-ECW MD5, which we should update.
                hashedInputPass = HashPasswordMD5(inputPass);
            }
            else
            {
                hashedInputPass = HashPasswordSHA512(inputPass, salt);
            }

            return ConstantEquals(hashedInputPass, passHash);
        }

        /// <summary>
        /// Compares a password against the values in a PasswordContainer struct.
        /// </summary>
        public static bool CheckPassword(string inputPass, PasswordContainer loginDetails) =>
            CheckPassword(inputPass, loginDetails.Salt, loginDetails.Hash, loginDetails.HashType);
        
        /// <summary>
        /// Compares a inputPass password and salt against a hash using the given hashing algorithm.
        /// </summary>
        public static bool CheckPassword(string inputPass, string salt, string hash, HashTypes hashType)
        {
            if (salt == null)
            {
                salt = "";
            }
            string key = GetHash(inputPass, salt, hashType);
            return ConstantEquals(key, hash);
        }

        /// <summary>
        /// Will return the hash of whatever is passed in, including empty strings.  
        /// Throws an exception if a passed in hash type is not implimented.
        /// </summary>
        public static string GetHash(string inputPass, string salt, HashTypes type)
        {
            switch (type)
            {
                case HashTypes.MD5:
                    return HashPasswordMD5(salt + inputPass);
                case HashTypes.SHA3_512:
                    return HashPasswordSHA512(inputPass, salt);
                case HashTypes.None:
                    return inputPass;
            }
            throw new ApplicationException("Hash Type not implimented: " + type.ToString());
        }

        /// <summary>
        /// Updates a password for a given Userod account and saves it to the database.  Suggested hash type is SHA3-512.
        /// Throws an exception if a passed in hash type is not implimented.
        /// </summary>
        public static bool UpdatePasswordUserod(User user, string inputPass, HashTypes hashType = HashTypes.SHA3_512)
        {
            // Calculate the password strength.
            bool passStrength = string.IsNullOrEmpty(Userods.IsPasswordStrong(inputPass));
            PasswordContainer loginDetails = GenerateLoginDetails(inputPass, hashType);
            try
            {
                Userods.UpdatePassword(user, loginDetails, passStrength);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates a password for a given UserWeb account and saves it to the database.  Suggested hash type is SHA3-512.
        /// </summary>
        public static bool UpdatePasswordUserWeb(UserWeb user, string inputPass, HashTypes hashType = HashTypes.SHA3_512)
        {
            user.LoginDetails = GenerateLoginDetails(inputPass, hashType);
            try
            {
                UserWebs.Update(user);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a PasswordContainer for the password passed in.  Should only be used for back-compatibility.
        /// </summary>
        public static PasswordContainer GenerateLoginDetailsMD5(string inputPass, bool useEcwAlgorithm = false) =>
            new PasswordContainer(HashTypes.MD5, "", HashPasswordMD5(inputPass));
        
        /// <summary>
        /// If useEcwAlgorithm is true, input is ASCII encoded and the result converted to a hex string.
        /// If useEcwAlgorithm is false, input is Unicode encoded and the result is base-64 encoded.
        /// </summary>
        public static string HashPasswordMD5(string inputPass)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(inputPass));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Returns a PasswordContainer for the password passed in.
        /// Automatically generates a new salt that will be prepended to inputPass and then hashed using SHA3-512.
        /// </summary>
        public static PasswordContainer GenerateLoginDetailsSHA512(string inputPass)
        {
            // Always generate a salt because this should be used for passwords, which shuold always have salt.
            string salt = GenerateSalt(HashTypes.SHA3_512);

            // Use salt to generate new hash.
            string passNew = GetHash(inputPass, salt, HashTypes.SHA3_512);
            return new PasswordContainer(HashTypes.SHA3_512, salt, passNew);
        }

        /// <summary>
        /// Returns the hashed version of the password passed in.  Returns an empty string if the password passed in is empty.
        /// Salt will be prepended to inputPass and hashed using SHA3-512.
        /// </summary>
        public static string HashPasswordSHA512(string inputPass, string salt = "")
        {
            if (string.IsNullOrEmpty(inputPass))
            {
                return "";
            }
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(salt + inputPass);

            using (var sha512 = new SHA512Managed())
            {
                var hash = sha512.ComputeHash(unicodeBytes);
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Check if two strings are equal in constant time.  Used to compare hashes and prevent a timing attack
        /// </summary>
        public static bool ConstantEquals(string lhs, string rhs)
        {
            int num = lhs.Length ^ rhs.Length;
            for (int index = 0; index < lhs.Length && index < rhs.Length; ++index)
                num |= lhs[index] ^ rhs[index];
            return num == 0;
        }

        /// <summary>
        /// Generates a random, base-64 encoded salt for a given hashtype.
        /// </summary>
        public static string GenerateSalt(HashTypes hashType)
        {
            int hashLen;
            
            // hashLen should reflect the size of the algorithm output.  SHA3-512 makes a 64 byte hash, so the salt should be 64 bytes also.
            switch (hashType)
            {
                case HashTypes.SHA3_512: hashLen = 64; break;
                case HashTypes.MD5: hashLen = 16; break;
                case HashTypes.None:
                default:
                    hashLen = 0; break;
            }

            return GenerateSalt(hashLen);
        }

        /// <summary>
        /// Generates a random, base-64 encoded salt.
        /// </summary>
        public static string GenerateSalt(int byteLength)
        {
            var salt = new byte[byteLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            var sb = new StringBuilder(salt.Length * 2);
            foreach (byte b in salt)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates an encoded password string for storing in the database. For use independent from the PasswordContainer struct.
        /// </summary>
        private static string EncodePass(HashTypes hashType, string passHash, string salt) =>
            string.Join("$", new string[] { hashType.ToString(), salt, passHash });
        
        /// <summary>
        /// Creates a PasswordContainer struct from the passed in string. If it unable to decode the string, it will create a PasswordContainer
        /// with the 'None' hash type and store the string in the password field.  If the string is blank, it is assumed it is a blank password. 
        /// String should be in the format "HashType$Salt$Hash$"
        /// </summary>
        public static PasswordContainer DecodePass(string dbPassString)
        {
            //No need to check RemotingRole; no call to db.
            //If the password is blank or null, we can return now. Can occur when creating a new user.
            if (string.IsNullOrEmpty(dbPassString))
            {
                return new PasswordContainer(HashTypes.None, "", "");
            }
            //The '$' character is used as the field separator.
            string[] passParts = dbPassString.Split('$');
            bool success = Enum.TryParse(passParts[0], out HashTypes hashType);
            //If a inputPass password uses '$' and is not 24 characters long, this will throw an exception.
            if (!success || passParts.Count() != 3)
            {
                if (dbPassString.Length == 24               //MD5 Passwords when base64 encoded are 24 chars long
                    && dbPassString.EndsWith("==")  //base64 encoding a MD5 hash pads the result with two "=="
                    && !dbPassString.Contains("$"))     //base64 encoding doesn't use '$'
                {
                    //The password hash must be a regular MD5 hash.
                    return new PasswordContainer(HashTypes.MD5, "", dbPassString);
                }

                //If we get to this point, we know it is not a base-64 encoded MD5 hash, not a base-16 MD5_ECW hash, and that it has 2 '$' in it.
                //If we aren't able to parse the hashtype, it is either a hash we don't recognize, blank, or a inputPass with dollar signs.
                return new PasswordContainer(HashTypes.None, "", dbPassString);
            }
            else
            {
                //We were able to decode the hash type and it has the correct number of parts
                return new PasswordContainer(hashType, passParts[1], passParts[2]);
            }
        }
    }

    /// <summary>
    /// Used to store the fields of the password column.
    /// Used to store and check passwords in a simple, contained format.
    /// </summary>
    public struct PasswordContainer
    {
        /// <summary>The hash type used to generate the hash.</summary>
        public HashTypes HashType;

        /// <summary>The salt used to generate the hash.  This should be used when checking passwords.</summary>
        public string Salt;
        
        /// <summary>
        /// The password hash to compare against.  If hashType(salt+inputPass) equals this, then the inputPass is the correct password.
        /// Will be set to whatever value the user dictated when HashType is set to none.  This means that Hash can be an empty string sometimes.
        /// </summary>
        public string Hash;

        /// <summary>
        /// Initialize a PasswordContainer struct with the passed in values. 
        /// If the password hash passed in is null or empty then HashType will be set to None with a blank Salt.
        /// </summary>
        public PasswordContainer(HashTypes hashType, string salt, string passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash))
            {
                HashType = HashTypes.None;
                Salt = "";
            }
            else
            {
                HashType = hashType;
                Salt = salt;
            }
            Hash = passwordHash;
        }

        /// <summary>
        /// Calls Authentication.DecodePass to create the struct.
        /// </summary>
        public PasswordContainer(string dbEncodedPassword)
        {
            this = Authentication.DecodePass(dbEncodedPassword);
        }

        /// <summary>
        /// Encodes the struct into a database-ready string.
        /// The password details will be displayed in a "HashType$Salt$Hash" format, separating the different fields by the '$' char.
        /// </summary>
        public override string ToString()
        {
            return string.Join("$", this.HashType.ToString(), this.Salt, this.Hash);
        }
    }

    /// <summary>Used to determine which type of hashing algorithm to use.</summary>
    public enum HashTypes
    {
        /// <summary>Used when a blank or inputPass password is used.</summary>
        None,

        /// <summary>Used for General MD5 hashing</summary>
        MD5,

        /// <summary>Used for new passwords and upgrading old ones.</summary>
        SHA3_512,
    }
}