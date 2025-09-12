using System.Collections;
using TMPro;
using UnityEngine;

public class ErrorShowing : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel;
    private TextMeshProUGUI errorText;
    private CanvasGroup canvasGroup;
    private RectTransform panelRectTransform;
    private static ErrorShowing instance;
    private Coroutine currentCoroutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Transform errorTextTransform = errorPanel.transform.Find("ErrorText");
        errorText = errorTextTransform.GetComponent<TextMeshProUGUI>();
        panelRectTransform = errorPanel.GetComponent<RectTransform>();
        canvasGroup = errorPanel.GetComponent<CanvasGroup>();
        errorPanel.SetActive(false);
    }

    public static void ShowError(string error, Vector2 mousePosition, float duration)
    {
        if (instance && instance.errorText)
        {
            instance.errorText.text = error;
            instance.MoveToMousePosition(mousePosition);

            if (instance.currentCoroutine != null)
            {
                instance.StopCoroutine(instance.currentCoroutine);
            }

            instance.currentCoroutine = instance.StartCoroutine(instance.ShowAndFadeError(duration));
        }
    }

    private void MoveToMousePosition(Vector2 mousePosition)
    {
        Vector2 screenPoint = mousePosition;
        screenPoint.x -= 785.094f / 8 + 150f;
        screenPoint.y += 264.65f / 8 + 30f;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRectTransform.parent as RectTransform,
            screenPoint,
            null,
            out Vector2 localPoint
        );
        panelRectTransform.localPosition = localPoint;
    }

    private IEnumerator ShowAndFadeError(float fadeDuration)
    {
        errorPanel.SetActive(true);
        canvasGroup.alpha = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; 
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        errorPanel.SetActive(false);
        currentCoroutine = null;
    }
}
