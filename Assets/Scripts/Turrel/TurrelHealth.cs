using System;
using UniRx;
using UnityEngine;

public class TurrelHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100F;

    private SliderProgressor _sliderProgressor;
    private CompositeDisposable _healthTurrelDisposable = new CompositeDisposable();

    public ReactiveCommand HealthTurrelProgressorCommand = new ReactiveCommand();
    public float CurrentHealth { get => _health; set => _health = value; }
    public event Action OnTurrelDead;

    private void Awake() =>
        _sliderProgressor = GetComponentInChildren<SliderProgressor>();

    private void Start()
    {     
        HealthTurrelProgressorCommand.Subscribe(_ =>
        {
            HealthProgress();
        }).AddTo(_healthTurrelDisposable);
    }

    private void HealthProgress()
    {
        _sliderProgressor.UpdateBar(_health);        
        if (_health <= 0f)
            Destroy();
    }

    private void Destroy()
    { 
        gameObject.SetActive(false);
        OnTurrelDead?.Invoke();
    }
    

    private void OnDisable() =>    
        _healthTurrelDisposable.Dispose();
   
}

