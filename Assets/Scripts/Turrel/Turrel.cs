
using UniRx;
using UnityEngine;

public class Turrel : MonoBehaviour , IDamageable<float>
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _turrelHead;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _angle = 360f;
    [SerializeField] private float _distanse = 10f;
    [SerializeField] private float _shotDelay = 3.0f;

    private ShotDelayTextUI shotDelayText;
    private TurrelHealth _turrelHealth;
    private TargetChecker _targetChecker;
    private PlayerHealth _playerHealth;
    private Shot _shot;
    private bool IsInRadiusZone = false;
    private PlayerVision _playerVision;
    private float _timer;
    private float _currentTimer;
    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _targetChecker = new TargetChecker();
        _shot = GetComponent<Shot>();
        _turrelHealth = GetComponent<TurrelHealth>();
        _playerVision = GetComponent<PlayerVision>();
        _playerHealth = _target.gameObject.GetComponent<PlayerHealth>();
        shotDelayText = GetComponentInChildren<ShotDelayTextUI>();
        shotDelayText.Text.gameObject.SetActive(false);
    }

  
    void Update()
    {
        if (_turrelHealth.CurrentHealth <= 0) return;

        IsInRadiusZone = _targetChecker.CheckAvailableTarget(_turrelHead, _target, _angle, _distanse);

        if (Vector3.Distance(transform.position, _playerVision.Target.position) < _playerVision.Distance)
        {
            if (_playerVision.RayToScan())
            {
                if ((_timer += Time.deltaTime) > _shotDelay)
                {
                    _timer = 0.0f;
                    if (_playerHealth.IsDead.Value)
                        return;

                    if (IsInRadiusZone)
                    {
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
            }
        }     
    }

    public void Damage(float damageAmount)
    {
        _turrelHealth.CurrentHealth -= damageAmount;
        _turrelHealth.HealthTurrelProgressorCommand.Execute();
    }

    private void OnDisable() =>
        _disposable.Dispose();
}
