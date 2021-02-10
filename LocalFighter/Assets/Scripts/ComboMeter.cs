using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboMeter : MonoBehaviour
{
    Slider slider;
    
    

    // Start is called before the first frame update
    void Start()
    {
        slider = this.transform.GetComponent<Slider>();
    }


    public void SetMeter(int meterCount)
    {
        slider.value = meterCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
