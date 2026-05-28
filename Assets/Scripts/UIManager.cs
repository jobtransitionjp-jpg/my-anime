using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private Slider volumeSlider;
    private Text volumeLabel;

    private void Awake()
    {
        CreateCanvas();
    }

    private void CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Isekai UI");
        canvasObject.transform.SetParent(transform, false);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObject.AddComponent<GraphicRaycaster>();

        CreateEventSystem();
        CreatePanel(canvasObject.transform);
    }

    private void CreateEventSystem()
    {
        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }

    private void CreatePanel(Transform parent)
    {
        GameObject panel = new GameObject("Isekai UI Panel");
        panel.transform.SetParent(parent, false);
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.35f);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.02f, 0.7f);
        panelRect.anchorMax = new Vector2(0.35f, 0.98f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        CreateTitle(panel.transform);
        CreateVolumeControl(panel.transform);
    }

    private void CreateTitle(Transform parent)
    {
        GameObject title = new GameObject("Isekai UI Title");
        title.transform.SetParent(parent, false);
        Text titleText = title.AddComponent<Text>();
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.text = "Isekai Controls";
        titleText.fontSize = 20;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.UpperLeft;

        RectTransform titleRect = title.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.05f, 0.75f);
        titleRect.anchorMax = new Vector2(0.95f, 0.95f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;
    }

    private void CreateVolumeControl(Transform parent)
    {
        GameObject labelObject = new GameObject("Volume Label");
        labelObject.transform.SetParent(parent, false);
        volumeLabel = labelObject.AddComponent<Text>();
        volumeLabel.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        volumeLabel.text = "Master Volume: 100%";
        volumeLabel.fontSize = 16;
        volumeLabel.color = Color.white;
        volumeLabel.alignment = TextAnchor.MiddleLeft;

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0.05f, 0.4f);
        labelRect.anchorMax = new Vector2(0.95f, 0.6f);
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        GameObject sliderObject = new GameObject("Volume Slider");
        sliderObject.transform.SetParent(parent, false);
        volumeSlider = sliderObject.AddComponent<Slider>();
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = 1f;
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        RectTransform sliderRect = sliderObject.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.05f, 0.1f);
        sliderRect.anchorMax = new Vector2(0.95f, 0.35f);
        sliderRect.offsetMin = Vector2.zero;
        sliderRect.offsetMax = Vector2.zero;

        CreateSliderBackground(sliderObject.transform);
        CreateSliderFill(sliderObject.transform);
        CreateHandle(sliderObject.transform);
    }

    private void CreateSliderBackground(Transform parent)
    {
        GameObject background = new GameObject("Background");
        background.transform.SetParent(parent, false);
        Image image = background.AddComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.2f);
        RectTransform rect = background.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private void CreateSliderFill(Transform parent)
    {
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(parent, false);
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0.1f, 0.25f);
        fillAreaRect.anchorMax = new Vector2(0.9f, 0.75f);
        fillAreaRect.offsetMin = Vector2.zero;
        fillAreaRect.offsetMax = Vector2.zero;

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = new Color(0.65f, 0.8f, 1f, 0.9f);

        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        volumeSlider.fillRect = fillRect;
    }

    private void CreateHandle(Transform parent)
    {
        GameObject handle = new GameObject("Handle");
        handle.transform.SetParent(parent, false);
        Image handleImage = handle.AddComponent<Image>();
        handleImage.color = new Color(0.9f, 0.9f, 1f, 1f);

        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(20f, 20f);

        volumeSlider.targetGraphic = handleImage;
        volumeSlider.handleRect = handleRect;
    }

    private void OnVolumeChanged(float value)
    {
        volumeLabel.text = $"Master Volume: {Mathf.RoundToInt(value * 100f)}%";
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            source.volume = value;
        }
    }
}
