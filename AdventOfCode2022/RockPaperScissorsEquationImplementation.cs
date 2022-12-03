namespace AdventOfCode2022;

public static class RockPaperScissorsEquationImplementation
{
    public static int ScoreOfOpponentMoveAndPlayerMove(string round)
    {
        int i = round[0] - 'A';
        int j = round[2] - 'X';
        return (6 * i + 4 * j + 3) % 9 + 1;
    }

    public static int ScoreOfOpponentMoveAndResult(string round)
    {
        int i = round[0] - 'A';
        int j = round[2] - 'X';
        return 3 * j + (i + j + 2) % 3 + 1;
    }
}
