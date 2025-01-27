using System;
namespace GameEngine.Gameplay;

static class RandomNumber
{
    private static Random rand = new Random();

    public static int GetRandomNumber(int min, int max)
    {
        return rand.Next(min, max);
    }

    public static float GetRandomFloat(float min, float max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }
}