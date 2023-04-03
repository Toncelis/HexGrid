using System.Collections.Generic;

public static class ListExt {
    public static void AddIfNewAndNotNull<T>(this IList<T> list, T obj) {
        if (obj != null && !list.Contains(obj)) {
            list.Add(obj);
        }
    }
}