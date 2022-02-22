using System;
using UniRx;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private const float _maxHealth = 100f;
    private const float _minHealth = 0f;

    [SerializeField] private float _health = 100F;
    [SerializeField] private float _healthRegenAmount = 5f;
    [SerializeField] private float _timeBetweenRegeneration = 5f;

    private SliderProgressor _sliderProgressor;
    private CompositeDisposable _healthBarDisposable = new CompositeDisposable();
    private CompositeDisposable _regeneratingDisposable = new CompositeDisposable();
    public BoolReactiveProperty IsDead = new BoolReactiveProperty();
    public ReactiveCommand HealthProgressorCommand = new ReactiveCommand();
    public float CurrentHealths { get => _health; set => _health = value; }
   
    public event Action OnLose;

    private void Awake() =>
        _sliderProgressor = GetComponentInChildren<SliderProgressor>();

    private void Start()
    {
        _health = 100f;
        _sliderProgressor.UpdateBar(_health);
        IsDead.Value = false;
        HealthProgressorCommand.Subscribe(_ =>
        {
            HealthProgress();

        }).AddTo(_healthBarDisposable);
    }

    private void HealthProgress()
    {
        _regeneratingDisposable.Clear();
        _sliderProgressor.UpdateBar(_health);

        if (_health <= _minHealth)
        {
            IsDead.Value = true;
            DeathPlayer();
        }

        Observable.Timer(TimeSpan.FromSeconds(_timeBetweenRegeneration)).Subscribe(_ =>
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (_health <= 100)
                    Regenerating();
                else
                {
                    _health = _maxHealth;
                    _sliderProgressor.UpdateBar(_health);
                    _regeneratingDisposable.Clear();
                }

            }).AddTo(_regeneratingDisposable);

        }).AddTo(_regeneratingDisposable);
       
    }

    private void DeathPlayer()
    {
        OnLose?.Invoke();
        gameObject.SetActive(false);
    }

    public void Regenerating()
    {
        _health += _healthRegenAmount * Time.deltaTime;
        _sliderProgressor.UpdateBar(_health);
    }
    private void OnDisable()
    {
        _healthBarDisposable.Dispose();
        _regeneratingDisposable.Dispose();
    }
}
