using System;
using System.Text;

public static class StringUtils {
    public static string UpperCaseFirstChar(string str) {
        if (String.IsNullOrEmpty(str) || Char.IsUpper(str, 0))
            return str;

        return Char.ToUpperInvariant(str[0]) + str.Substring(1);
    }

    public static string LowerCaseFirstChar(string str) {
        if (String.IsNullOrEmpty(str) || Char.IsLower(str, 0))
            return str;

        return Char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    public static string ToSnakeCaseFromPascal(string input) {
        if (input.Length < 2) {
            return input;
        }
        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(input[0]));
        for (int i = 1; i < input.Length; ++i) {
            char c = input[i];
            if (char.IsUpper(c)) {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            } else {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static string ToPascalCaseFromSnake(string input) {
        if (input.Length < 2) {
            return input.ToUpper();
        }

        var split = input.Split("_");

        StringBuilder sb = new StringBuilder();
        foreach (var word in split) {
            sb.Append(StringUtils.UpperCaseFirstChar(word));
        }

        return sb.ToString();
    }
}