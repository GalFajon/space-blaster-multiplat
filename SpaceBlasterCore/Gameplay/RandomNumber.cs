using System;

static class RandomNumber
{
    private static Random rand = new Random();

    public static int GetRandomNumber(int min, int max)
    {
        return rand.Next(min, max);
    }
}