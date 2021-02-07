using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboCounterBehavior : MonoBehaviour
{

    private static int sortingOrder;
    private TextMeshPro textMesh;
    private float dissapearTimer;
    private Color textColor;
    private const float dissapearTimerMax = 1f;
    Vector3 moveVector;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int comboCount)
    {
        textMesh.sortingOrder = sortingOrder;
        textMesh.SetText("COMBO " + comboCount.ToString());
        textColor = textMesh.color;
        dissapearTimer = dissapearTimerMax;
        moveVector = new Vector3(1, 1) * 30f;
    }
    
    void Update()
    {
        float moveYSpeed = 5f;
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 15f * Time.deltaTime;
        if (dissapearTimer > dissapearTimerMax * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        dissapearTimer -= Time.deltaTime;


        if (dissapearTimer < 0)
        {
            float dissapearSpeed = 3f;
            textColor.a -= dissapearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
