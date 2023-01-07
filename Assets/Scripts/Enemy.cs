using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Move,
    Attack
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    public int _hp;
    public float _attackRangeSqr;
    public float _attackAccuracy;
    public int _attackDamage;
    public float _attackDelay;
    public float _moveSpeed;

    [Space(10)]
    [SerializeField] private ParticleSystem _particle1;
    [SerializeField] private ParticleSystem _particle2;

    [Space(10)]
    [SerializeField] private AudioClip _spawnSound;
    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _hitSound;


    private GameObject _player;
    private Rigidbody _rb;
    private Animator _anim;
    private AudioSource _audio;

    private float _lastAttackTime;
    private EnemyState _enemyState = EnemyState.Move;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        if (_audio == null)
        {
            _audio = GetComponentInChildren<AudioSource>();
        }
        Debug.Assert(_player != null);
    }

    public void OnSpawn()
    {
        _hp = 100;
        if(_audio == null)
        {
            _audio = GetComponent<AudioSource>();
        }
        _audio.PlayOneShot(_spawnSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceToPlayer() < _attackRangeSqr)
        {
            Attack();
        }
        else
            Move();
    }

    private float DistanceToPlayer()
    {
        return (_player.transform.position-transform.position).sqrMagnitude;
    }

    private void Attack()
    {
        if( _enemyState == EnemyState.Move )
        {
            _enemyState = EnemyState.Attack;
            _anim.SetTrigger("Attack");
            _audio.Stop();
        }
        LookAt2D(_player.transform);
        if (_lastAttackTime + _attackDelay < Time.time)
        {
            _audio.PlayOneShot(_attackSound);
            _particle1.Stop();
            _particle2.Stop();
            _particle1.Play();
            _particle2.Play();
            // TODO: 공격 모션 취하기
            // 탄퍼짐 구현하는 대신 명중율을 랜덤으로 설정
            _rb.velocity = Vector3.zero;
            if (Random.Range(0, 1f) < _attackAccuracy)
            {
                _player.GetComponent<PlayerHP>().DoDamage(_attackDamage);
            }
            _lastAttackTime = Time.time;
        }
    }

    private void Move()
    {
        if(!_audio.isPlaying)
        {
            _audio.clip = _moveSound;
            _audio.loop = true;
            _audio.Play();
        }
        if (_enemyState == EnemyState.Attack)
        {
            _enemyState = EnemyState.Move;
            _anim.SetTrigger("Move");
        }
        LookAt2D(_player.transform);
        if(_rb.velocity.sqrMagnitude < _moveSpeed)
        _rb.AddForce(transform.forward * _moveSpeed, ForceMode.VelocityChange);
    }

    private void LookAt2D(Transform target)
    {
        transform.LookAt(target);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void DoDamage(int damage)
    {
        // TODO: 데미지 리액션
        _audio.PlayOneShot(_hitSound);
        _hp -= damage;
        if (_hp <= 0)
            Die();
    }

    private void Die()
    {
        StartCoroutine(DoDie());
    }

    private IEnumerator DoDie()
    {
        if (_audio.isPlaying)
        {
            _audio.Stop();
            _audio.loop = false;
        }
        _audio.PlayOneShot(_hitSound);
        // TODO: 죽음 연출
        var particle = GetComponent<ParticleSystem>();
        particle.Stop();
        particle.Play();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
