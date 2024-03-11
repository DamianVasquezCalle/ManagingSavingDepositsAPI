using System.Text;
using System.Security.Cryptography;

namespace DB.Utils;

public static class Utils
{
	public static string EncodeString(string text)
	{
		using (SHA256 sha256 = SHA256.Create())
		{
			StringBuilder hash = new StringBuilder();
			byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
			foreach (byte b in hashArray)
			{
				hash.Append(b.ToString("x"));
			}
			return hash.ToString();
		}
	}
}
