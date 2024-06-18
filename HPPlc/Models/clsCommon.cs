using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services;

namespace HPPlc.Models
{
	public static class clsCommon
	{
		//private static byte[] key = { };
		//private static byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
		//public static string Encrypt(string stringToEncrypt)
		//{
		//    try
		//    {

		//        string SEncryptionKey = "!*&@8762387623";
		//        key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey.Substring(0, 8));
		//        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
		//        byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
		//        MemoryStream ms = new MemoryStream();
		//        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
		//        cs.Write(inputByteArray, 0, inputByteArray.Length);
		//        cs.FlushFinalBlock();
		//        return Convert.ToBase64String(ms.ToArray());
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}
		//public static string Decrypt(string stringToDecrypt)
		//{
		//    stringToDecrypt = stringToDecrypt.Replace(" ", "+");
		//    string sEncryptionKey = "!*&@8762387623";
		//    byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];

		//    MemoryStream ms = null;
		//    Encoding encoding = null;
		//    try
		//    {
		//        ms = new MemoryStream();

		//        key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
		//        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
		//        inputByteArray = Encoding.ASCII.GetBytes(stringToDecrypt);// Convert.FromBase64String(stringToDecrypt);
		//        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
		//        cs.Write(inputByteArray, 0, inputByteArray.Length);
		//        //if (!stringToDecrypt.Equals(""))
		//        //{
		//        cs.FlushFinalBlock();
		//        //}
		//        encoding = System.Text.Encoding.UTF8;
		//    }
		//    catch
		//    {
		//        throw;
		//    }

		//    return encoding.GetString(ms.ToArray());
		//}

		public static string Encrypt(string input, string password = "E6t187^D43%F")
		{
			string encryptedBytes = String.Empty;
			try
			{
				if (!String.IsNullOrWhiteSpace(input))
				{
					byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
					passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

					// Set your salt here, change it to meet your flavor:  
					// The salt bytes must be at least 8 bytes.  
					byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
					using (MemoryStream ms = new MemoryStream())
					{
						using (RijndaelManaged aes = new RijndaelManaged())
						{
							aes.KeySize = 256;
							aes.BlockSize = 128;
							var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
							aes.Key = key.GetBytes(aes.KeySize / 8);
							aes.IV = key.GetBytes(aes.BlockSize / 8);
							aes.Mode = CipherMode.CBC;
							aes.Padding = PaddingMode.Zeros;
							using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
							{
								cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
								cs.Close();
							}
							encryptedBytes = Convert.ToBase64String(ms.ToArray()).Replace("\0", "");
						}
					}
				}
			}
			catch { }

			return encryptedBytes;

			//string EncryptionKey = "MAKV2SPBNI99212";
			//byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			//using (Aes encryptor = Aes.Create())
			//{
			//	Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
			//	encryptor.Key = pdb.GetBytes(32);
			//	encryptor.IV = pdb.GetBytes(16);
			//	using (MemoryStream ms = new MemoryStream())
			//	{
			//		using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
			//		{
			//			cs.Write(clearBytes, 0, clearBytes.Length);
			//			cs.Close();
			//		}
			//		clearText = Convert.ToBase64String(ms.ToArray());
			//	}
			//}
			//return clearText;
		}
		public static string Decrypt(string decryptionText, string password = "E6t187^D43%F")
		{
			string decryptedBytes = String.Empty;
			try
			{
				if (!String.IsNullOrWhiteSpace(decryptionText))
				{
					decryptionText = decryptionText.Replace(" ", "+");
					byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptionText);
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
					passwordBytes = SHA256.Create().ComputeHash(passwordBytes);


					// Set your salt here, change it to meet your flavor:  
					// The salt bytes must be at least 8 bytes.  
					byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
					using (MemoryStream ms = new MemoryStream())
					{
						using (RijndaelManaged AES = new RijndaelManaged())
						{
							AES.KeySize = 256;
							AES.BlockSize = 128;
							var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
							AES.Key = key.GetBytes(AES.KeySize / 8);
							AES.IV = key.GetBytes(AES.BlockSize / 8);
							AES.Mode = CipherMode.CBC;
							AES.Padding = PaddingMode.Zeros;
							using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
							{
								cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
								cs.Close();
							}
							decryptedBytes = Encoding.UTF8.GetString(ms.ToArray()).Replace("\0", "");
						}
					}
				}
			}
			catch { }

			return decryptedBytes;
		}

		//public static string Decrypt(string decryptionText, string password = "E6t187^D43%F")
		//{
		//	string decryptedBytes = String.Empty;
		//	try
		//	{
		//		if (!String.IsNullOrWhiteSpace(decryptionText))
		//		{
		//			decryptionText = decryptionText.Replace(" ", "+");
		//			byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptionText);
		//			byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
		//			passwordBytes = SHA256.Create().ComputeHash(passwordBytes);


		//			// Set your salt here, change it to meet your flavor:  
		//			// The salt bytes must be at least 8 bytes.  
		//			byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
		//			using (MemoryStream ms = new MemoryStream())
		//			{
		//				using (RijndaelManaged AES = new RijndaelManaged())
		//				{
		//					AES.KeySize = 256;
		//					AES.BlockSize = 128;
		//					var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
		//					AES.Key = key.GetBytes(AES.KeySize / 8);
		//					AES.IV = key.GetBytes(AES.BlockSize / 8);
		//					AES.Mode = CipherMode.CBC;
		//					AES.Padding = PaddingMode.Zeros;
		//					using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
		//					{
		//						cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
		//						cs.Close();
		//					}
		//					decryptedBytes = Encoding.UTF8.GetString(ms.ToArray()).Replace("\0", "");
		//				}
		//			}
		//		}
		//	}
		//	catch { }

		//	return decryptedBytes;

		//	//string EncryptionKey = "MAKV2SPBNI99212";
		//	//byte[] cipherBytes = Convert.FromBase64String(cipherText);
		//	//using (Aes encryptor = Aes.Create())
		//	//{
		//	//	Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
		//	//	encryptor.Key = pdb.GetBytes(32);
		//	//	encryptor.IV = pdb.GetBytes(16);
		//	//	using (MemoryStream ms = new MemoryStream())
		//	//	{
		//	//		using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
		//	//		{
		//	//			cs.Write(cipherBytes, 0, cipherBytes.Length);
		//	//			cs.Close();
		//	//		}
		//	//		cipherText = Encoding.Unicode.GetString(ms.ToArray());
		//	//	}
		//	//}

		//	//return cipherText;
		//}



		public static List<T> ConvertDataTable<T>(DataTable dt)
		{
			List<T> data = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T item = GetItem<T>(row);
				data.Add(item);
			}
			return data;
		}
		private static T GetItem<T>(DataRow dr)
		{
			Type temp = typeof(T);
			T obj = Activator.CreateInstance<T>();

			foreach (DataColumn column in dr.Table.Columns)
			{
				foreach (PropertyInfo pro in temp.GetProperties())
				{
					if (pro.Name == column.ColumnName)
						pro.SetValue(obj, dr[column.ColumnName], null);
					else
						continue;
				}
			}
			return obj;
		}


		public static string DecryptInt(string TextToBeDecrypted)
		{
			RijndaelManaged RijndaelCipher = new RijndaelManaged();



			string Password = "HPPLC";
			string DecryptedData;



			try
			{
				byte[] EncryptedData = Convert.FromBase64String(TextToBeDecrypted);



				byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
				//Making of the key for decryption
				PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
				//Creates a symmetric Rijndael decryptor object.
				ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));



				MemoryStream memoryStream = new MemoryStream(EncryptedData);
				//Defines the cryptographics stream for decryption.THe stream contains decrpted data
				CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);



				byte[] PlainText = new byte[EncryptedData.Length];
				int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
				memoryStream.Close();
				cryptoStream.Close();



				//Converting to string
				DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
			}
			catch (Exception ex)
			{
				DecryptedData = TextToBeDecrypted;
			}
			return DecryptedData;
		}



		public static string EncryptInt(string TextToBeEncrypted)
		{
			RijndaelManaged RijndaelCipher = new RijndaelManaged();
			string Password = "HPPLC";
			byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(TextToBeEncrypted.Trim());
			byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
			//Creates a symmetric encryptor object.
			ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
			MemoryStream memoryStream = new MemoryStream();
			//Defines a stream that links data streams to cryptographic transformations
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(PlainText, 0, PlainText.Length);
			//Writes the final state and clears the buffer
			cryptoStream.FlushFinalBlock();
			byte[] CipherBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			string EncryptedData = Convert.ToBase64String(CipherBytes);



			return EncryptedData.Replace("+", "_dkg_");
		}

		public static string encrypto(string encryptString)
		{
			string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
			0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
		});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					encryptString = Convert.ToBase64String(ms.ToArray());
				}
			}
			return encryptString;
		}

		public static string Decrypto(string cipherText)
		{
			string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
					0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
				});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}

		public static string encrypwithspecialkey(string encryptString)
		{
			string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ&=:/";
			byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
			0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
		});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					encryptString = Convert.ToBase64String(ms.ToArray());
				}
			}
			return encryptString;
		}

		public static string Decryptowithspecialkey(string cipherText)
		{
			string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ&=:/";
			cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
					0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
				});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText.Replace("%3A", ":").Replace("%3a", ":").Replace("%2f", "/");
		}
		public static string GenerateReferralCode()
		{
			var chars = "0123456789";
			var random = new Random();
			var result = new string(
				Enumerable.Repeat(chars, 4)
						  .Select(s => s[random.Next(s.Length)])
						  .ToArray());
			return "REF" + result;
		}

		public static string GenerateTransactionId()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			var result = new string(
				Enumerable.Repeat(chars, 12)
						  .Select(s => s[random.Next(s.Length)])
						  .ToArray());
			return result;
		}

		public static string GetDictionaryValueForLanguage(string key, CultureInfo culture, ServiceContext services)
		{
			var dictionaryItem = services.LocalizationService.GetDictionaryItemByKey(key);
			if (dictionaryItem != null)
			{
				var translation = dictionaryItem.Translations.SingleOrDefault(x => x.Language.CultureInfo.Equals(culture));
				if (translation != null)
				{
					return translation.Value;
				}
			}
			return string.Empty;
		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}


		public static string Encryptwithbase64Code(string input, string password = "E6t187^D43%F")
		{
			string encryptedBytes = String.Empty;
			try
			{
				if (!String.IsNullOrWhiteSpace(input))
				{
					byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
					passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

					// Set your salt here, change it to meet your flavor:  
					// The salt bytes must be at least 8 bytes.  
					byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
					using (MemoryStream ms = new MemoryStream())
					{
						using (RijndaelManaged aes = new RijndaelManaged())
						{
							aes.KeySize = 256;
							aes.BlockSize = 128;
							var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
							aes.Key = key.GetBytes(aes.KeySize / 8);
							aes.IV = key.GetBytes(aes.BlockSize / 8);
							aes.Mode = CipherMode.CBC;
							aes.Padding = PaddingMode.Zeros;
							using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
							{
								cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
								cs.Close();
							}
							encryptedBytes = Convert.ToBase64String(ms.ToArray()).Replace("\0", "");
							encryptedBytes = Base64Encode(encryptedBytes);
						}
					}
				}
			}
			catch { }

			return encryptedBytes;

			//string EncryptionKey = "MAKV2SPBNI99212";
			//byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			//using (Aes encryptor = Aes.Create())
			//{
			//	Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
			//	encryptor.Key = pdb.GetBytes(32);
			//	encryptor.IV = pdb.GetBytes(16);
			//	using (MemoryStream ms = new MemoryStream())
			//	{
			//		using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
			//		{
			//			cs.Write(clearBytes, 0, clearBytes.Length);
			//			cs.Close();
			//		}
			//		clearText = Convert.ToBase64String(ms.ToArray());
			//	}
			//}
			//return clearText;
		}
		public static string DecryptWithBase64Code(string decryptionText, string password = "E6t187^D43%F")
		{
			string decryptedBytes = String.Empty;
			try
			{
				if (!String.IsNullOrWhiteSpace(decryptionText))
				{
					decryptionText = Base64Decode(decryptionText);
					decryptionText = decryptionText.Replace(" ", "+");
					byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptionText);
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
					passwordBytes = SHA256.Create().ComputeHash(passwordBytes);


					// Set your salt here, change it to meet your flavor:  
					// The salt bytes must be at least 8 bytes.  
					byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
					using (MemoryStream ms = new MemoryStream())
					{
						using (RijndaelManaged AES = new RijndaelManaged())
						{
							AES.KeySize = 256;
							AES.BlockSize = 128;
							var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
							AES.Key = key.GetBytes(AES.KeySize / 8);
							AES.IV = key.GetBytes(AES.BlockSize / 8);
							AES.Mode = CipherMode.CBC;
							AES.Padding = PaddingMode.Zeros;
							using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
							{
								cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
								cs.Close();
							}
							decryptedBytes = Encoding.UTF8.GetString(ms.ToArray()).Replace("\0", "");
						}
					}
				}
			}
			catch { }

			return decryptedBytes;
		}

	}
}
