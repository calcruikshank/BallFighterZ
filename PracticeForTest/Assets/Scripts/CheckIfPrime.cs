using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPrime : MonoBehaviour
{

    public int input = 20;
    // Start is called before the first frame update
    void Start()
    {
        bool isPrime = CheckIfPrimeNumber(input);
        Debug.Log(isPrime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIfPrimeNumber(int input)
    {
        if (input == 1 || input % 2 == 0) return false;
        else
        {
            return true;
        }
    }
}
