using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private IEnumerator FadeToClear() {
        float timer = 0f;
        while (timer <= duration) {
            fadeScreen.color = Color.Lerp(startColor, endColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeToBlack(int sceneNumber) {
        float timer = 0f;
        while (timer <= duration) {
            fadeScreen.color = Color.Lerp(endColor, startColor, timer / duration);
            timer += Time.deltaTime;
            if (fadeScreen.color.a >= 0.99f) {
                SceneManager.LoadScene(sceneNumber);
                yield break;
            }
            else {
                yield return null;
            }
        }
    }

    public void StartScene() {
        StartCoroutine(FadeToClear());
        sceneStarting = false;
    }

    public void BeginTransition(int sceneNumber) { 
        Debug.Log("Begin Transition");
        StartCoroutine("FadeTransition", sceneNumber);
    }

    private IEnumerator FadeTransition(int sceneNumber) {
        Debug.Log("Begin Fade");
        fadeScreen.enabled = true;
        StartCoroutine(FadeToBlack(sceneNumber));
        yield return null;
    }
}
