using CodeBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OpenDentBusiness.UI
{
    public class SigBox
    {
        /// <summary>
        /// Get the key used to encrypt the signature in a sheet. The key is made up of all the 
        /// field values in the sheet in the order they were inserted into the db. This method 
        /// assumes the list of sheet fields was already sorted.
        /// </summary>
        public static string GetSignatureKeySheets(List<SheetField> sheetFields)
        {
            var stringBuilder = new StringBuilder();

            foreach (var sheetField in sheetFields)
            {
                if (sheetField.FieldValue == "" ||
                    sheetField.FieldType.In(SheetFieldType.SigBox, SheetFieldType.SigBoxPractice))
                {
                    continue;
                }

                stringBuilder.Append(sheetField.FieldValue);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encrypts signature text and returns a base 64 string so that it can go directly into 
        /// the database. Takes in a hashed key, and a string describing the signature using 
        /// semi-colon separated points (i.e. "x1,y1;x2,y2;").
        /// </summary>
        public static string EncryptSigString(byte[] key, string signatureAsPoints)
        {
            if (string.IsNullOrWhiteSpace(signatureAsPoints)) return "";

            byte[] signatureBytes = Encoding.UTF8.GetBytes(signatureAsPoints);

            // Compression could have been done here, using DeflateStream
            // A decision was made not to use compression because it would have taken more time
            // and not saved much space.

            using (var memoryStream = new MemoryStream())
            using (var crypt = Rijndael.Create())
            {
                crypt.KeySize = 128;
                crypt.Key = key;
                crypt.IV = new byte[16];

                using (var cryptoStream = new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(signatureBytes, 0, signatureBytes.Length);
                    cryptoStream.FlushFinalBlock();
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}
