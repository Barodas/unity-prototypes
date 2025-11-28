using System;

public static class Number
{
    public static string Format(int num)
    {
        var power = MathF.Floor(MathF.Log10(num));
        var mantissa = num / MathF.Pow(10, power);
        if (power < 3)
        {
            return num.ToString();
        }
        return $"{mantissa:F2}e{power}";
    }
}
