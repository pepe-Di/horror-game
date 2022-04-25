using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Text Percent;
    public Image ProgressBar;
    private static Transition instance;
    private static bool playEndAnim = false;
    private Animator animator;
    private AsyncOperation loadingSceneOperation;
    public static void LoadScene(string sceneName)
    {
        instance.animator.SetTrigger("Start");
        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingSceneOperation.allowSceneActivation = false;
        instance.ProgressBar.fillAmount = 0;
    }
    void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();
        if (playEndAnim) 
        { 
            animator.SetTrigger("End");
            instance.ProgressBar.fillAmount = 1;
            // Чтобы если следующий переход будет обычным SceneManager.LoadScene, не проигрывать анимацию opening:
            playEndAnim = false;
        }
    }
    public void OnAnimationOver()
    {
        playEndAnim = true;
        loadingSceneOperation.allowSceneActivation = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (loadingSceneOperation != null)
        {
           // Debug.Log(""+ loadingSceneOperation.progress*100);
            Percent.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";
            //ProgressBar.fillAmount = loadingSceneOperation.progress / 0.9f;
            ProgressBar.fillAmount = Mathf.Lerp(ProgressBar.fillAmount, loadingSceneOperation.progress, Time.deltaTime * 5);
        }
    }
}
