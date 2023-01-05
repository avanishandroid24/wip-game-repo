using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomID
{
    public static string GenerateRandom4(bool isAlphaNumeric)
    {
        string charCollection;
        string randomString = "";
        if (isAlphaNumeric) charCollection = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        else charCollection = "0123456789";

        for(int i = 1; i <= 4; i++)
        {
            int index = Random.Range(0, 61);
            randomString += charCollection[index];
        }

        return randomString;
    }

	public static string GenerateRandom8(bool isAlphaNumeric)
	{
		string charCollection;
		string randomString = "";
		if (isAlphaNumeric) charCollection = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		else charCollection = "0123456789";

		for (int i = 1; i <= 8; i++)
		{
			int index = Random.Range(0, 61);
			randomString += charCollection[index];
		}

		return randomString;
	}

	public static string GenerateRandom16(bool isAlphaNumeric)
	{
		string charCollection;
		string randomString = "";
		if (isAlphaNumeric) charCollection = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		else charCollection = "0123456789";

		for (int i = 1; i <= 16; i++)
		{
			int index = Random.Range(0, 61);
			randomString += charCollection[index];
		}

		return randomString;
	}



}
