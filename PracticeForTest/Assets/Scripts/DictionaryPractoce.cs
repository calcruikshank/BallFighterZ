using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryPractoce : MonoBehaviour
{

    public Dictionary<string, int> dictionary;

    List<int> intList = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        
        intList.Add(1);

        Debug.Log(intList[0]);


        dictionary = new Dictionary<string, int>();
        dictionary.Add("john", 120);
        Debug.Log(dictionary["john"]);
        //intList.Add();
        //List.RemoveAt
        //List.Insert

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
