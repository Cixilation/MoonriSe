using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator instance;
    public GameObject damagePopUpPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        Debug.Log(color);
        double damage = Math.Ceiling((double)int.Parse(text));
        string roundedDamage = damage.ToString();
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f) + 1f, Random.Range(0f, 0.25f));
        var popup = Instantiate(damagePopUpPrefab, position + randomness, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = roundedDamage;
        temp.color = color;
        Destroy(popup, 1f);
    }
}
