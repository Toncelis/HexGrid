using System;

public static class Enum<T> where T:Enum {
    public static T Random () {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
}