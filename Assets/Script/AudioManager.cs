using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip peacefulMusic;
    [SerializeField] private AudioClip tenseMusic;
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 15f;

    private bool isTenseMusicPlaying = false;

    void Update()
    {
        CheckPlayerRange();
    }

    private void CheckPlayerRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.position, detectionRange);
        bool enemyNearby = false;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemyNearby = true;
                break;
            }
        }

        if (enemyNearby && !isTenseMusicPlaying)
        {
            PlayMusic(tenseMusic);
            isTenseMusicPlaying = true;
        }
        else if (!enemyNearby && isTenseMusicPlaying)
        {
            PlayMusic(peacefulMusic);
            isTenseMusicPlaying = false;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
}