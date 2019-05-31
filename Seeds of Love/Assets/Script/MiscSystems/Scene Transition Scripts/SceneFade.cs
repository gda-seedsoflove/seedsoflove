using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*-------------------------------------------------------
SceneFade.cs / LoadScene.cs README
- Put both SceneFade and LoadScene into the canvas
    - Make a FadeManager prefab that has the canvas and the scripts so it is easier to implement
- Set SceneFade's FadeScreen to desired image
- Set SceneFade's starting color to opaque black
- Set SceneFade's end color to transparent
- Duration can be manually set
- SceneStarting should only be set to true if it is not the first scene in the game
- LoadScene's scene number corresponds to the scene it will go to in the build
    - Note: Could exchange the integer value for an obj/scene variable if this is not adequate
- If a button needs to be used to transition scene, then set the button's click
to Trigger() from LoadScene.
- Similarly, trigger() can be used to trigger a transition.
--------------------------------------------------------*/

public class SceneFade : MonoBehaviour {

    // Variables
    public Image fadeScreen;
    public Color startColor;
    public Color endColor;
    public float duration;
    public bool sceneStarting = true;

    void Awake() {
        fadeScreen.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sceneStarting) {
            StartScene();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Fades from black to clear -> Starting scene
    private IEnumerator FadeToClear() {
        float timer = 0f;
        while (timer <= duration) {
            fadeScreen.color = Color.Lerp(startColor, endColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // Fades from clear to black -> Ending scene
    private IEnumerator FadeToBlack(int sceneNumber) {
        float timer = 0f;
        while (timer <= duration) {
            fadeScreen.color = Color.Lerp(endColor, startColor, timer / duration);
            timer += Time.deltaTime;
            if (fadeScreen.color.a >= 0.99f || timer > duration) {
                SceneManager.LoadScene(sceneNumber);
                yield break;
            }
            else {
                yield return null;
            }
        }
    }

    // Triggers starting scene in Start
    public void StartScene() {
        StartCoroutine(FadeToClear());
        sceneStarting = false;
    }

    // Begins transition w/ desired scene number
    public void BeginTransition(int sceneNumber) { 
        Debug.Log("Begin Transition");
        StartCoroutine("FadeTransition", sceneNumber);
    }

    // Calls FadeToBlack to begin fading
    private IEnumerator FadeTransition(int sceneNumber) {
        Debug.Log("Begin Fade");
        fadeScreen.enabled = true;
        StartCoroutine(FadeToBlack(sceneNumber));
        yield return null;
    }
}
