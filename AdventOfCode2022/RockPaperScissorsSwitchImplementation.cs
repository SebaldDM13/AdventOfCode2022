namespace AdventOfCode2022;

public static class RockPaperScissorsSwitchImplementation
{
    public enum HandShape
    {
        None = 0,
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public enum Result
    {
        Lose = 0,
        Tie = 3,
        Win = 6
    }

    public static int ScoreOfOpponentMoveAndPlayerMove(string round)
    {
        return ScoreOf(CharToHandShape(round.First()), CharToHandShape(round.Last()));
    }

    public static int ScoreOfOpponentMoveAndResult(string round)
    {
        Result r = CharToResult(round.Last());
        return (int)r + (int)PlayerMoveOf(CharToHandShape(round.First()), r);
    }

    public static HandShape CharToHandShape(char c) => char.ToUpper(c) switch
    {
        'A' => HandShape.Rock,
        'B' => HandShape.Paper,
        'C' => HandShape.Scissors,
        'X' => HandShape.Rock,
        'Y' => HandShape.Paper,
        'Z' => HandShape.Scissors,
        _ => HandShape.None
    };

    public static Result CharToResult(char c) => char.ToUpper(c) switch
    {
        'X' => Result.Lose,
        'Y' => Result.Tie,
        'Z' => Result.Win,
        _ => Result.Tie
    };

    public static int ScoreOf(HandShape opponentMove, HandShape playerMove)
    {
        return (int)PlayerResultOf(opponentMove, playerMove) + (int)playerMove;
    }

    private static Result PlayerResultOf(HandShape opponentMove, HandShape playerMove) => (opponentMove, playerMove) switch
    {
        (HandShape.Rock, HandShape.Scissors) => Result.Lose,
        (HandShape.Scissors, HandShape.Paper) => Result.Lose,
        (HandShape.Paper, HandShape.Rock) => Result.Lose,
        (HandShape.Rock, HandShape.Paper) => Result.Win,
        (HandShape.Scissors, HandShape.Rock) => Result.Win,
        (HandShape.Paper, HandShape.Scissors) => Result.Win,
        (_, _) => Result.Tie
    };

    public static HandShape PlayerMoveOf(HandShape opponentMove, Result playerResult) => (opponentMove, playerResult) switch
    {
        (HandShape.Rock, Result.Lose) => HandShape.Scissors,
        (HandShape.Scissors, Result.Lose) => HandShape.Paper,
        (HandShape.Paper, Result.Lose) => HandShape.Rock,
        (HandShape.Rock, Result.Win) => HandShape.Paper,
        (HandShape.Scissors, Result.Win) => HandShape.Rock,
        (HandShape.Paper, Result.Win) => HandShape.Scissors,
        (_, _) => opponentMove
    };
}
