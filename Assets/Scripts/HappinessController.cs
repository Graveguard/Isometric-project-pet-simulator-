using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HappinessController : MonoBehaviour
{
    public Slider slider;
    public float durationToReduceHappinessCell = 60f;
    public float AnimationTransitionTime = 1f;
    public float elapsedTime = 0f;
    private bool isReducing = false;
    public bool isDogstartRunning=false;
    private void Update()
    {
        // Check if the 60-second delay is reached.
        if (!GameManager.Instance.m_Player.IsInteracting)
        {
            if (slider.value > 0)
            {
                if (!isReducing && elapsedTime >= durationToReduceHappinessCell)
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

                    if (slider.value < 0.4)
                    {
                        isDogstartRunning = true;
                        Debug.LogError("Dog is about to run");
                    }
                }

                // Increment the elapsed time.
                elapsedTime += Time.deltaTime;
            }
        }
    }
}
