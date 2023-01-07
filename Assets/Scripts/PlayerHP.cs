using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private Image _hitImage;
    private AudioSource _audio;

    private float lastHitTime = 0;

    public int HP;
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        UpdateHPGauge();
    }

    // Update is called once per frame
    void Update()
    {
        var hitEffectStrength = 1f - Time.time + lastHitTime;
        if (hitEffectStrength < 0)
            hitEffectStrength = 0;
        _hitImage.color = new Color(hitEffectStrength, 0, 0);
    }

    public void DoDamage(int damage)
    {
        lastHitTime = Time.time;
        // TODO: 피격 연출
        _audio.PlayOneShot(_hitSound);
        HP -= damage;
        UpdateHPGauge();
        if(HP <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void UpdateHPGauge()
    {
        _slider.value = HP;
    }
}
