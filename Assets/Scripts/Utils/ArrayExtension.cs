using System;

public static class ArrayExtension
{
    public static T[] ShuffleDistinct<T>(this T[] array)
    {
        if (array.Length == 1)
        {
            return array;
        }

        T[] tempStorage = new T[array.Length];
        array.CopyTo(tempStorage, 0);

        int breakCounter = 1000;

        for(int i = 0; i < breakCounter; i++)
        {
            array.Shuffle();

            for (int j = 0; j < array.Length; j++)
            {
                if (!(tempStorage[i].Equals(array[i])))
                {
                    return array;
                }
            }
        }

        return array;
    }

    public static T[] Shuffle<T>(this T[] array)
    {
        if (array.Length == 1)
        {
            return array;
        }

        int n = array.Length;
        var rng = new Random();

        while (n > 1)
        {
            int k = rng.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }

        return array;
    }

    public static T[] Shuffle<T>(this T[] array, int fromIndex, int toIndex, bool distinct = false)
    {
        if (array.Length == 1)
        {
            return array;
        }

        if (fromIndex < 0)
        {
            fromIndex = 0;
        }

        if(toIndex > array.Length - 1)
        {
            toIndex = array.Length - 1;
        }

        if(fromIndex > toIndex)
        {
            return array;
        }

        T[] tempStorage = new T[toIndex + 1 - fromIndex];

        for(int i = 0; i < tempStorage.Length; i++)
        {
            tempStorage[i] = array[fromIndex + i];
        }
      
        if (distinct)
        {
            tempStorage.ShuffleDistinct();
        }
        else
        {
            tempStorage.Shuffle();
        }

        for (int i = 0; i < tempStorage.Length; i++)
        {
            array[fromIndex + i] = tempStorage[i];
        }

        return array;
    }

    public static int[] FillArrayWithIndexes(this int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i;
        }

        return array;
    }
}