using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public List<int> GenerateRandomNumbers(List<int> numbers)
    {
        List<int> randomNumbers = new List<int>();

        for (int i = 0; i < numbers.Count; i++)
        {
            var temp = numbers[i];
            int randomIndex = Random.Range(i, numbers.Count);
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        return randomNumbers;
    }
}
