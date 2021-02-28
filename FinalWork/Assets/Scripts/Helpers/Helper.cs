using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public List<int> GenerateRandomNumbers(List<int> numbers)
    {
        List<int> randomNumbers = new List<int>();

        while (randomNumbers.Count < numbers.Count)
        {
            int randomIndex = Random.Range(0, numbers.Count);
            if (!randomNumbers.Contains(randomIndex))
            {
                randomNumbers.Add(randomIndex);
            }
        }

        return randomNumbers;
    }
}
