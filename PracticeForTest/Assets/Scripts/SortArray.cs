using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortArray : MonoBehaviour
{
    public int arraySize = 10;
    int[] arrayGiven;
    // Start is called before the first frame update
    void Start()
    {
        arrayGiven = new int[arraySize];
        for (int i = 0; i < arrayGiven.Length; i++)
        {
            arrayGiven[i] = i;
            
        }

        //ShiftRight(arrayGiven);
        ShiftLeft(arrayGiven);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShiftRight(int[] array)
    {
        

       
        //Debug.Log(array[arraySize - 1]);
        for (int i = 0; i < arrayGiven.Length - 1; i++)
        {
            //setting temp equal to i + 1 then changing array at i + 1 equal to temp
            int temp = array[0];
            array[0] = array[i + 1];
            array[i + 1] = temp;
        }


        for (int i = 0; i < arrayGiven.Length; i++)
        {
            Debug.Log(array[i]);
        }
        
    }


    public void ShiftLeft(int[] array)
    {
        for (int i = array.Length; i > 0; i--)
        {
            int temp = array[array.Length - 1];
            array[array.Length - 1] = array[i - 1];
            array[i - 1] = temp;

        }

        for (int i = 0; i < arrayGiven.Length; i++)
        {
            Debug.Log(array[i]);
        }
    }
}
