using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseString : MonoBehaviour
{
    string input = "step on no pets";

    public int numOfWords = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("input " + input);
        string output = ReverseInput(input);
        Debug.Log(output);

        CheckIfPalindrome(input, output);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string ReverseInput(string input)
    {
        char[] letters = input.ToCharArray();
        for (int i = 0, j = input.Length - 1; i < j; i++, j--)
        {
            letters[i] = input[j];
            letters[j] = input[i];
        }
        string output = new string(letters);
        return output;
    }

    void CheckIfPalindrome(string input, string output)
    {
        if (input == output)
        {
            Debug.Log("is a palindrome");
        }
        if (input != output)
        {
            Debug.Log("is not a palindrome");
        }
    }


}
