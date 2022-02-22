using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable<float>
{
    [SerializeField] private float _shotDelay = 2.0f;

    private PlayerHealth _health;
    private TurrelHealth[] _turrelsArray;
    private MovementTank _movementTank;
    private ShotDelayTextUI shotDelayText;
    private Shot _shot;
    private float _timer;
    private float _currentTimer;
    private CompositeDisposable _disposable = new CompositeDisposable();
    private List<TurrelHealth> _newTurrelList = new List<TurrelHealth>();
    public event Action OnVictory;
    private void Awake()
    {
        _turrelsArray = FindObjectsOfType<TurrelHealth>();
        _movementTank = GetComponent<MovementTank>();
        _shot = GetComponent<Shot>();
        _health = GetComponent<PlayerHealth>();
        shotDelayText = GetComponentInChildren<ShotDelayTextUI>();
        shotDelayText.Text.gameObject.SetActive(false);
    }
    private void Start()
    {
        foreach (var item in _turrelsArray)
        {
            item.OnTurrelDead += OnTurrelDead;
            _newTurrelList.Add(item);
        }
    }



    private void Update()
    {
        if (_newTurrelList.Count <= 0)
            OnVictory.Invoke();

        if ((_timer += Time.deltaTime) > _shotDelay)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _timer = 0.0f;
                _shot.ToShot();
                _currentTimer = _shotDelay;
                shotDelayText.Text.gameObject.SetActive(true);
                Observable.EveryUpdate().Subscribe(_ =>
                {
                    if (_currentTimer <= _shotDelay)
                    {
                        _currentTimer -= Time.deltaTime;
                        shotDelayText.UpdateTextDelay(_currentTimer);
                    }
                    if (_currentTimer <= 0)
                    {
                        _disposable.Clear();
                        _currentTimer = _shotDelay;
                        shotDelayText.Text.gameObject.SetActive(false);
                    }

                }).AddTo(_disposable);
            }
        }

        _movementTank.AutoTurretRotate();
    }

    private void FixedUpdate()
    {
        _movementTank.Movement();
        _movementTank.Rotate();
    }

    public void Damage(float damageAmount)
    {
        _health.CurrentHealths -= damageAmount;
        _health.HealthProgressorCommand.Execute();
    }
    private void OnTurrelDead()
    {
        _newTurrelList.RemoveAt(_newTurrelList.Count-1);
    }

    private void OnDisable() =>
        _disposable.Dispose();
}

