using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
#region List

    public static void ShuffleList<T>(this List<T> list)
    {
        List<T> tempList = list;
        for (int i = 0; i < tempList.Count; i++)
        {
            var temp = tempList[i];
            int randomIndex = Random.Range(i, tempList.Count);
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;
        }
    }
    
    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    
    public static T PopRandomElement<T>(this List<T> list)
    {
        object item = list[Random.Range(0, list.Count)];
        
        list.Remove((T)item);
        
        return (T) item;
    }
    
#endregion

#region int

    public static int CalculateProbability(this int intToCalculate, List<float> probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Count; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Count - 1;
    }
	
#endregion
}