using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class LightingController : MonoBehaviour
{
    [Header("Lighting Settings")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightPreset Preset;
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [Header("Skybox Settings")]
    [SerializeField] private Material DaySkybox;
    [SerializeField] private Material NightSkybox;

    [SerializeField] private GameObject zombie;
    
    //Note : To make the day longer just change the time if the Day Duration;   
    private float DayDuration = 24f * 50;
    private float TransitionDuration = 2f;

    private bool dayOneNightZero;
    private Material currentSkyboxMaterial;

    private void Start()
    {
        if (DaySkybox != null && NightSkybox != null)
        {
            currentSkyboxMaterial = new Material(DaySkybox);
            RenderSettings.skybox = currentSkyboxMaterial;
            StartCoroutine(TransitionSkybox(NightSkybox));
        }
    }

    private void Update()
    {
        if (DirectionalLight == null)
        {
            DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        }
        if (Preset == null) return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * (24f / DayDuration);
            TimeOfDay %= 24f;

            UpdateLighting(TimeOfDay / 24f);
            HandleSkyboxTransition();
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }
    
    private bool canSpawn = true;
    private void HandleSkyboxTransition()
    {

        if (TimeOfDay < 6.4f || TimeOfDay >= 17f)
        {
            if (Random.value <= 0.7f && canSpawn) 
            {
                StartCoroutine(SpawnAfterDelay(10f));
            }

            if (dayOneNightZero)
            {
                dayOneNightZero = !dayOneNightZero;
                StartCoroutine(TransitionSkybox(NightSkybox));
            }
        }
        else
        {
            if (!dayOneNightZero)
            {
                dayOneNightZero = !dayOneNightZero;
                StartCoroutine(TransitionSkybox(DaySkybox));
            }
        }
    }
    private IEnumerator SpawnAfterDelay(float delay)
    {
        canSpawn = false; 
        yield return new WaitForSeconds(delay);
        if (Random.value <= 0.2f)
        {
            MapBuilder.instance.PlaceObjectOnGrid(zombie, MapBuilder.instance.RandomIndex(),
                MapBuilder.instance.RandomIndex());
        }

        canSpawn = true; 
    }
    private IEnumerator TransitionSkybox(Material targetSkybox)
    {
        Material initialSkybox = RenderSettings.skybox;
        RenderSettings.skybox = targetSkybox;

        float progress = 0f;
        while (progress < 2f)
        {
            progress += Time.deltaTime / TransitionDuration;
            RenderSettings.skybox.Lerp(initialSkybox, targetSkybox, progress);

            DynamicGI.UpdateEnvironment();
            yield return null;
        }

        RenderSettings.skybox = targetSkybox;
        DynamicGI.UpdateEnvironment();
    }

    private void OnValidate()
    {
        if (DirectionalLight != null) return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
