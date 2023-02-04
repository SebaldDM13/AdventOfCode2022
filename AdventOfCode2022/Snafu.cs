namespace AdventOfCode2022;

public class Snafu
{
    private readonly List<char> digits;

    public Snafu() : this("0")
    {
    }

    public Snafu(string s)
    {
        digits = s.ToList();
    }

    private static int SnafuDigitToInt(char c) => c switch
    {
        '=' => -2,
        '-' => -1,
        '0' => 0,
        '1' => 1,
        '2' => 2,
        _ => 0
    };

    private static (char, char) IntToSnafuDigits(int i) => i switch
    {
        -5 => ('-', '0'),
        -4 => ('-', '1'),
        -3 => ('-', '2'),
        -2 => ('0', '='),
        -1 => ('0', '-'),
        0 => ('0', '0'),
        1 => ('0', '1'),
        2 => ('0', '2'),
        3 => ('1', '='),
        4 => ('1', '-'),
        5 => ('1', '0'),
        _ => ('0', '0')
    };

    public void Add(Snafu other)
    {
        while (digits.Count < other.digits.Count)
        {
            digits.Insert(0, '0');
        }

        while (other.digits.Count < digits.Count)
        {
            other.digits.Insert(0, '0');
        }

        char carry = '0';
        for (int i = digits.Count - 1; i >= 0; i--)
        {
            int placeSum = SnafuDigitToInt(carry) + SnafuDigitToInt(digits[i]) + SnafuDigitToInt(other.digits[i]);
            (carry, digits[i]) = IntToSnafuDigits(placeSum);
        }

        if (carry != '0')
        {
            digits.Insert(0, carry);
        }
    }

    public override string ToString()
    {
        return string.Concat(digits);
    }
}
