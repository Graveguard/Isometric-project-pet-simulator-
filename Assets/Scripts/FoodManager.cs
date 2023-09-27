using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FoodManager : MonoBehaviour
{
    public Slider slider;
    public float durationToReduceFoodCell = 60f;
    public float AnimationTransitionTime = 1f;
    public float elapsedTime = 0f;
    private bool isReducing = false;
    public GameObject foodobject;
    public bool dogIsDie;
    private void Update()
    {
        // Check if the 60-second delay is reached.
        if (!GameManager.Instance.m_DogController.Eating)
        {
            if (slider.value > 0)
            {
                if (!isReducing && elapsedTime >= durationToReduceFoodCell)
                {
                    // Start reducing the slider value.
                    isReducing = true;
                    elapsedTime = 0f; // Reset the timer.
                }

                // Reduce the slider value if we are in the reducing state.
                if (isReducing)
                {
                    // Calculate the new slider value using Lerp.
                    slider.DOValue(slider.value - .2f, AnimationTransitionTime);
                    isReducing = false;
                    //Debug.LogError("slider.value" + slider.value);

                    if (slider.value < 0.4)
                    {
                        dogIsDie = true;
                        //Debug.LogError("Dog is about to die");
                    }
                }

                // Increment the elapsed time.
                elapsedTime += Time.deltaTime;
            }
        }
    }
}
