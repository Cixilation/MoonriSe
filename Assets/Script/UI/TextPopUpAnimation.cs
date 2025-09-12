using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 originalPosition;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        originalPosition = transform.position;
    }


    void Update()
    {
        tmp.color = new Color(1,1,1,opacityCurve.Evaluate(time));
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, scaleCurve.Evaluate(time));
        transform.position = originalPosition + new Vector3(0, heightCurve.Evaluate(time) * 1f, 0);
        time += Time.deltaTime;
    }
}
