using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarImage;
    public Gradient ColorGradient;

    private float healthVal = 1f;

    void Start()
    {
        UpdateHealth(1f);
    }

    public void UpdateHealth (float val)
    {
        HealthBarImage.fillAmount = val;
        HealthBarImage.color = ColorGradient.Evaluate(val);
    }

    private void Update()
    {
        this.transform.parent.LookAt(Camera.main.transform);
        //if (Input.GetKey(KeyCode.W) && healthVal < 1f)
        //    healthVal += Time.deltaTime;
        //else if (Input.GetKey(KeyCode.S) && healthVal > 0f)
        //    healthVal -= Time.deltaTime;
        //UpdateHealth(healthVal);
    }
}
