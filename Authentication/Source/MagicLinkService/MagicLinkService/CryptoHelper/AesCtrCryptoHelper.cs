namespace PTAS.MagicLinkService.CryptoHelper
{
    using Microsoft.IdentityModel.Tokens;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;

    /// <summary>
    /// Helper to encrypt information using AES-CTR.
    /// </summary>
    public class AesCtrCryptoHelper
    {
        /// <summary>
        /// The cypher mode.
        /// </summary>
        private SicBlockCipher mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AesCtrCryptoHelper" /> class.
        /// </summary>
        public AesCtrCryptoHelper()
        {
            this.mode = new SicBlockCipher(new AesFastEngine());
        }

        /// <summary>
        /// Encrypts the specified string.
        /// </summary>
        /// <param name="plain">The plain string.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns>The encrypted string.</returns>
        public string Encrypt(string plain, byte[] key, byte[] iv)
        {
            byte[] input = System.Text.Encoding.UTF8.GetBytes(plain);
            byte[] bytes = this.BouncyCastleCrypto(true, input, key, iv);
            string result = Base64UrlEncoder.Encode(bytes);

            return result;
        }

        /// <summary>
        /// Decrypts the specified cipher.
        /// </summary>
        /// <param name="cipher">The cipher.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns>The decrypted cypher.</returns>
        public string Decrypt(string cipher, byte[] key, byte[] iv)
        {
            byte[] bytes = this.BouncyCastleCrypto(false, Base64UrlEncoder.DecodeBytes(cipher), key, iv);
            string result = System.Text.Encoding.UTF8.GetString(bytes);
            return result;
        }

        private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, byte[] key, byte[] iv)
        {
            this.mode.Init(forEncrypt, new ParametersWithIV(new KeyParameter(key), iv));
            BufferedBlockCipher cipher = new BufferedBlockCipher(this.mode);
            return cipher.DoFinal(input);
        }
    }
}
