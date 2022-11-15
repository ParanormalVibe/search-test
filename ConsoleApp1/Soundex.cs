using System.Text.RegularExpressions;

namespace ConsoleApp1;

public static class Soundex
{

    static Regex codePattern = new Regex("[A-Z][0-9]{3}", RegexOptions.Compiled);
    public static string GetCode(string value)
    {
        value = value.ToLower();

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException();

        var letter = value[0];
        value = value[1..value.Length];

        if (string.IsNullOrWhiteSpace(value))
            return letter.ToString().ToUpper() + "000";

        if (!char.IsLetter(letter))
            letter = 'x'; // this is a hack to allow encoding for words that start with special characters

        var encodedValue = "";
        foreach (var c in value)
        {
            encodedValue += EncodeCharacter(c);
        }
        encodedValue = RemoveAllZeroes(encodedValue);
        encodedValue += "000";

        if (EncodeCharacter(letter) == encodedValue[0])
        {
            encodedValue = letter + encodedValue[1..encodedValue.Length];
        }
        else
        {
            encodedValue = letter + encodedValue;
        }

        if (encodedValue.Length < 4)
        {
            encodedValue += "000";
        }

        return encodedValue[0..4].ToUpper();
    }

    public static int GetDifference(string soundexString1, string soundexString2)
    {
        if (!IsValidSoundex(soundexString1))
        {
            throw new ArgumentException("Argument is not a Soundex encoded string", "soundexString1");
        }
        else if (!IsValidSoundex(soundexString2))
        {
            throw new ArgumentException("Argument is not a Soundex encoded string", "soundexString2");
        }
        var encodedValue1 = int.Parse(soundexString1[1..4]);
        var encodedValue2 = int.Parse(soundexString2[1..4]);
        return Math.Abs(encodedValue1 - encodedValue2);
    }

    private static bool IsValidSoundex(string str)
    {
        var regexMatch = codePattern.Match(str);
        var isSoundex = regexMatch.Value == str;
        return isSoundex;
    }

    private static char EncodeCharacter(char c)
    {
        switch (c)
        {
            case 'b':
            case 'f':
            case 'p':
            case 'v':
                return '1';
            case 'c':
            case 'g':
            case 'j':
            case 'k':
            case 'q':
            case 's':
            case 'x':
            case 'z':
                return '2';
            case 'd':
            case 't':
                return '3';
            case 'l':
                return '4';
            case 'm':
            case 'n':
                return '5';
            case 'r':
                return '6';
        }
        return '0';
    }

    private static string RemoveAllZeroes(string str)
    {
        return string.Concat(str.Where(x => x != '0'));
    }
}