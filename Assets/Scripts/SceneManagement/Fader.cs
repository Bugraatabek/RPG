using System.Collections;
using UnityEngine;

namespace RPG.Scenemanagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentlyActiveFade = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
            
        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        
        public Coroutine FadeOut(float time)
        {
            return Fade(time,1);
        }

        
        public Coroutine FadeIn(float time)
        {
            return Fade(time,0);
        }

        private Coroutine Fade(float time, float target)
        {
            if(currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }
            currentlyActiveFade = StartCoroutine(FadeRoutine(time, target));
            return currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float time, float target)
        {
            float deltaAlpha = Time.unscaledDeltaTime/time; // using unscaledDeltaTime because deltaTime won't work with Time.timescale = 0;
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, deltaAlpha);
                yield return null;
            }
        }

        
    }
}
