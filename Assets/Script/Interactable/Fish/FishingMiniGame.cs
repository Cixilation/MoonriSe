using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FishingMiniGame : MonoBehaviour
{
    [SerializeField] private Transform topPivot;
    [SerializeField] private Transform bottomPivot;
    [SerializeField] private GameObject fish;

    private float fishPosition;
    private float fishDestination;
    private float fishTimer;

    private float timerMultiplier = 3f;
    private float fishSpeed;
    private float smoothMotion = 1f;

    [SerializeField] private Transform hook;
    private float hookPosition;

    private float hookSize = 0.2f;
    private float fishSize = 1f;
    private float hookPower = 0.1f;
    private float hookPullVelocity;

    private float hookPullPower = 0.01f;
    private float hookGravityPower = 0.005f;
    private float hookProgressDegradationPower = 0.01f;

    [SerializeField] private Slider progressSlider;

    private float failTimer = 10f;
    private float failTimerCountdown;

    
    private RectTransform fishTransform;
    private RectTransform hookTransform;
    
    public static FishingMiniGame instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        failTimerCountdown = failTimer;
        fishTimer = Random.value * timerMultiplier;
        fishTransform = fish.GetComponent<RectTransform>();
        hookTransform = hook.GetComponent<RectTransform>();
        fishPosition = 0.5f;
    }
    
    public bool isFishing = false;

    private void OnEnable()
    {
        progressSlider.value = 0;
        isFishing = true;
        GotTheFish = false;
        fishDestination = Random.value;
        fishPosition = fishDestination;
        fish.transform.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);

    }

    private void OnDisable()
    {
        isFishing = false;
    }


    // private void Resize()
    // {
    //     if (!fish) return;
    //     RectTransform rectTransform = fish.GetComponent<RectTransform>();
    //     if (rectTransform == null) return;
    //
    //     float ySize = rectTransform.rect.height;
    //     Vector3 scale = fish.transform.localScale;
    //     float distance = Vector3.Distance(topPivot.position, bottomPivot.position);
    //     scale.y = distance / ySize * fishSize;
    //     rectTransform.localScale = scale;
    // }
    
    private void Update()
    {
        if (isFishing)
        {
            SafeZone();
            Hook();
            if (GotTheFish == false)
            {
                ProgressCheck();
            }
        }
        
    }
    public bool GotTheFish = false;

    private void SafeZone()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer <= 0f)
        {
            fishTimer = Random.value * timerMultiplier;
            fishDestination = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.transform.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }
    
    private void ProgressCheck()
    {
        Vector2 safecenter = fishTransform.anchoredPosition;
        float safeHalfHeight = fishTransform.sizeDelta.y / 2f;
        float safeTop = safecenter.y + safeHalfHeight;
        float safeBottom = safecenter.y - safeHalfHeight;
        
        Vector2 hookcenter = hookTransform.anchoredPosition;
        float hookHalfHeight = hookTransform.sizeDelta.y / 2f;
        float hookTop = hookcenter.y + hookHalfHeight - 20;
        float hookBottom = hookcenter.y - hookHalfHeight + 20;
        
       if (hookTop <= safeTop && safeBottom <= hookBottom)
        {
            progressSlider.value += hookPower * Time.deltaTime;
            failTimerCountdown = failTimer;
        }
        else
        {
            progressSlider.value -= hookProgressDegradationPower * Time.deltaTime;
            failTimerCountdown -= Time.deltaTime;
            if (failTimerCountdown <= 0f)
            {
                failTimerCountdown = failTimer;
            }
        }

        if (progressSlider.value >= 1f)
        {
            GameData.Instance.GameFish.quantity += 1;
            GotTheFish = true;
            PlayerData.Instance.canWalk = true;
        }

        progressSlider.value = Mathf.Clamp01(progressSlider.value);
    }
    
    private void Hook()
    {
        if (Input.GetMouseButton(0))
        {
            if (hookPosition >= 1f)
            {
                hookPosition = 1f;
                hookPullVelocity = 0f;
            }
            else
            {
                hookPullVelocity += hookPullPower * Time.deltaTime;
            }
        }
        else
        {
            if (hookPosition <= 0f)
            {
                hookPosition = 0f;
                hookPullVelocity = 0f;
            }
            else
            {
                hookPullVelocity -= hookGravityPower * Time.deltaTime;
            }
        }
        hookPosition += hookPullVelocity;
        hookPosition = Mathf.Clamp(hookPosition, 0f, 1f);
        hook.position = Vector3.Lerp(bottomPivot.position - new Vector3(0,40f,0), topPivot.position  + new Vector3(0,40f,0), hookPosition);
    }

}
