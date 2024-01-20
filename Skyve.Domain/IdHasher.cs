using System;
using System.Security.Cryptography;
using System.Text;

namespace Skyve.Domain;
public static class IdHasher
{
	public static string HashToShortString(int id)
	{
		using var md5 = MD5.Create();
		var inputBytes = BitConverter.GetBytes((int.MaxValue / 2) - id);

		var stringBuilder = new StringBuilder();
		for (var i = 0; i < inputBytes.Length; i++)
		{
			stringBuilder.Append(inputBytes[i].ToString("X2"));
		}

		return stringBuilder.ToString();
	}

	public static int ShortStringToHash(string hashedValue)
	{
		using var md5 = MD5.Create();
		var hashBytes = new byte[4];
		for (var i = 0; i < hashedValue.Length; i += 2)
		{
			hashBytes[i / 2] = Convert.ToByte(hashedValue.Substring(i, 2), 16);
		}

		return (int.MaxValue / 2) - BitConverter.ToInt32(hashBytes, 0);
	}
}
