using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPageManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject HowToPlay;
    public GameObject Settings;
    public GameObject TheMakingOf;
    public GameObject Play;
    public GameObject NewOrLoad;
    public GameObject ExitToDesktop;
    [HideInInspector] public GameObject[] screens;
    [Header("Pages ")]

    public GameObject[] SettingsPages;
    public GameObject[] TheMakingOfPages;

    public Image[] SettingButtonImage;
    public Image[] TheMakingOfImage;

    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    private Color buttonDefultColor = new Color(0.69f, 0.69f, 0.69f);

    public Button[] SettingNextButtons;
    public Button[] MakingOfNextButtons;

    private int currentPageIndex;
    void Start()
    {
        InitializeScreensArray();
        canvasGroup.alpha = 0f;
        ReturnToMenu();
    }

    private void InitializeScreensArray()
    {
        screens = new GameObject[]
        {
            HowToPlay,
            Settings,
            TheMakingOf,
            Play,
            NewOrLoad,
            ExitToDesktop
        };
    }

    public void ShowScreens(MenuSelection selection)
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
        int index = ReturnIndexOfSelection(selection);
        screens[index].SetActive(true);
        FadeIn(canvasGroup);
    }

    public void FadeIn(CanvasGroup obj)
    {
        // Set the alpha to 0 initially
        obj.alpha = 0f;
        // Use a Coroutine to gradually increase the alpha over time
        StartCoroutine(FadeCanvasGroup(obj, obj.alpha, 1f, fadeDuration));
    }
    public void FadeOut(CanvasGroup obj)
    {
        obj.alpha = 1f;
        StartCoroutine(FadeCanvasGroup(obj, obj.alpha, 0f, fadeDuration));
    }
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the final alpha value is set
        cg.alpha = targetAlpha;
    }

    public void ReturnToMenu()
    {
        foreach (GameObject screen in screens)
        {
            ShowSettingPages(0);
            ShowTheMakingOfPages(0);
            screen.SetActive(false);

        }
    }

    void ShowPages(bool Screen, int pageIndex)
    {
        currentPageIndex = pageIndex;
        var pages = Screen ? SettingsPages : TheMakingOfPages;
        var ImageGroup = Screen ? SettingButtonImage : TheMakingOfImage;
        var ButtonGroup = Screen ? SettingNextButtons : MakingOfNextButtons;
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
        foreach (Image img in ImageGroup)
        {
            img.color = buttonDefultColor;
        }
        ImageGroup[pageIndex].color = Color.white;
        pages[pageIndex].SetActive(true);
        FadeIn(pages[pageIndex].GetComponent<CanvasGroup>());

        ButtonGroup[0].interactable = pageIndex != 0;
        ButtonGroup[1].interactable = pageIndex != pages.Length - 1;
    }
    public void ShowSettingPages(int pageIndex)
    {
        ShowPages(true, pageIndex);
    }
    public void ShowTheMakingOfPages(int pageIndex)
    {
        ShowPages(false, pageIndex);
    }

    public void ShowNextSetting(int pages)
    {
        currentPageIndex += pages;
        ShowPages(true, currentPageIndex);
    }
    public void ShowNextMakingOf(int pages)
    {
        currentPageIndex += pages;
        ShowPages(false, currentPageIndex);
    }

    public int ReturnIndexOfSelection(MenuSelection selection)
    {
        return (int)selection;
    }

}
