public class FallingPiece
{
    private byte[] row;

    public FallingPiece(string[] lines)
    {
        row = lines.Select(LineToRow).ToArray();
    }

    private static byte LineToRow(string line)
    {
        byte b = 0;
        for (int i = line.Length - 1; i >= 0; i--)
        {
            b >>= 1;
            if (line[i] != ' ')
            {
                b |= 0x80;
            }
        }
        return b;
    }

    private bool MoveLeft()
    {
        if (row.Any(b => (b & 0x80) != 0))
            return false;

        for (int i = 0; i < row.Length; i++)
        {
            row[i] <<= 1;
        }

        return true;
    }

    private bool MoveRight()
    {
        if (row.Any(b => (b & 0x03) != 0))
            return false;

        for (int i = 0; i < row.Length; i++)
        {
            row[i] >>= 1;
        }

        return true;
    }

    private bool MoveDown()
    {
        return true;
    }
}