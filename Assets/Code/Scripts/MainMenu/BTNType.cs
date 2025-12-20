using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour
{
    public BTNType currentType;
    public Transform buttonScale;
    public AudioSource usedsource;
    public AudioClip usedclip;
    Vector3 defaultScale;

    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;

    bool isSound;
    bool isProcessing; // 버튼 연타 방지

    private void Start()
    {
        defaultScale = buttonScale.localScale;

        if (usedsource != null)
            usedsource.volume = 0.2f; // 0~1 사이 값으로 볼륨 조절
    }

    public void OnBtnClick()   // 버튼 OnClick에 연결
    {
        if (isProcessing) return;
        StartCoroutine(OnBtnClickRoutine());
    }

    private IEnumerator OnBtnClickRoutine()
    {
        isProcessing = true;

        // 1. 클릭 사운드 재생
        if (usedsource != null && usedclip != null)
        {
            usedsource.PlayOneShot(usedclip, 0.2f);

            // 클립 길이만큼 기다리기 (길면 0.1f~0.2f로 줄여도 됨)
            yield return new WaitForSeconds(usedclip.length);
        }

        // 2. 그 다음 버튼 기능 실행
        switch (currentType)
        {
            case BTNType.Start:
                SceneManager.LoadScene("2_Game");
                break;

            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;

            case BTNType.Sound:
                if (isSound)
                    Debug.Log("사운드 OFF");
                else
                    Debug.Log("사운드 ON");
                isSound = !isSound;
                break;

            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;

            case BTNType.Quit:
                Application.Quit();
                Debug.Log("게임 종료");
                break;

            case BTNType.Leave:
                SceneManager.LoadScene("1_MainMenu");
                break;
        }

        isProcessing = false;
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        if (cg == null) return;
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void CanvasGroupOff(CanvasGroup cg)
    {
        if (cg == null) return;
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
