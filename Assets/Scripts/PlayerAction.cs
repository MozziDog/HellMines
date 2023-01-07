using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]

public class PlayerAction : MonoBehaviour
{
    public Image _blockDestroyGauge;

    [Space(10)]
    public GameObject _gun;
    public GameObject _shovel;

    [Space(10)]
    public float _fireDelay;
    public float _actionDelay;

    [Space(10)]
    [SerializeField] private AudioClip _gunSound;
    [SerializeField] private AudioClip _shovelSound;
    [SerializeField] private AudioClip _flagSound;

    [SerializeField] private float _lastFireTime = 0;
    [SerializeField] private float _lastActionTime = 0;

    private StarterAssetsInputs _input;
    private AudioSource _audio;
    private ParticleSystem _gun_flameParticle;
    private ParticleSystem _gun_smokeParticle;
    private ParticleSystem _shovel_flameParticle;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _input.OnChangeWeapon += OnChangeWeapon;
        _audio = GetComponent<AudioSource>();
        var _gun_particles = _gun.GetComponentsInChildren<ParticleSystem>();
        _gun_flameParticle = _gun_particles[0];
        _gun_smokeParticle = _gun_particles[1];
        _shovel_flameParticle = _shovel.GetComponentInChildren<ParticleSystem>();
    }

    public void OnChangeWeapon(int weapon)
    {
        if (weapon == 1)
        {
            _gun.SetActive(true);
            _shovel.SetActive(false);
        }
        else
        {
            _gun.SetActive(false);
            _shovel.SetActive(true);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.fire == true && fireable())
        {
            if (_input.weapon == 1)
                fireWeapon1();
            else
                fireWeapon2();
        }
        if (_input.action == true && fireable())
        {
            setFlagToBlock();
        }
        RefreshGauge();
    }

    private bool fireable()
    {
        float delayEndTime = 0;
        if (_input.weapon == 1)
            delayEndTime = _lastFireTime + _fireDelay;
        else
            delayEndTime = _lastActionTime + _actionDelay;

        if(delayEndTime < Time.time)
        {
            if(_input.weapon == 1)
                _lastFireTime = Time.time;
            else
                _lastActionTime = Time.time;
            return true;
        }
        else return false;
    }

    private void fireWeapon1()
    {
        RaycastHit hit;
        // TODO: 공격 애니메이션 적용
        _gun_flameParticle.Stop();
        _gun_smokeParticle.Stop();
        _gun_flameParticle.Play();
        _gun_smokeParticle.Play();
        _audio.PlayOneShot(_gunSound);

        if (RaycastScreenCenter(out hit, LayerMask.GetMask("Enemy")))
        {
            Debug.Log("hit: " + hit.collider.name);
            if(hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().DoDamage(100);
            }
        }
    }

    private bool RaycastScreenCenter(out RaycastHit hit, LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
        return Physics.Raycast(ray, out hit, 1000, layerMask);
    }

    private void fireWeapon2()
    {
        RaycastHit hit;
        // TODO: 공격 애니메이션 적용
        if (RaycastScreenCenter(out hit, LayerMask.GetMask("TileBlock")))
        {
            if(_shovel_flameParticle != null)
            {
                _shovel_flameParticle.Stop();
                _shovel_flameParticle.Play();
            }
            _audio.PlayOneShot(_shovelSound);
            if(hit.collider.CompareTag("Block"))
            {
                var block = hit.collider.GetComponent<TileBlock>();
                block.DoDamage(10);
                DisplayBlockDestroyGauge(((float)block.hp) / TileBlock.maxHp);
            }
        }
    }

    private void setFlagToBlock()
    {
        RaycastHit hit;
        if(RaycastScreenCenter(out hit, LayerMask.GetMask("TileBlock")))
        {
            if(hit.collider.CompareTag("Block"))
            {
                var block = hit.collider.GetComponent<TileBlock>();
                block.SetFlag();
                _audio.PlayOneShot(_flagSound);
            }
        }
    }

    void DisplayBlockDestroyGauge(float gauge)
    {
        _blockDestroyGauge.fillAmount = gauge;
    }

    void RefreshGauge()
    {
        Color color = _blockDestroyGauge.color;
        float alpha = 0.5f * (1 - (Time.time - _lastActionTime) / 2);
        _blockDestroyGauge.color = new Color(color.r, color.g, color.b, alpha > 0 ? alpha : 0);
    }

}
