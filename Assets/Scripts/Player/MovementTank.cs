using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementTank : MonoBehaviour
{
    [SerializeField] private Transform _turret;
    [SerializeField] private float _verticalMoveSpeed = 5f;
    [SerializeField] private float _horizontalMoveSpeed = 50f;

    private Vector3 _directionOffset = new Vector3(0, 0.5f, 0);
    private Rigidbody _rigidbody;
    private Vector3 _movement;
    private Quaternion _turnRotation;
    private float _turn;
    private float _moveVertical;
    private float _moveHorizontal;
    private Turrel closest;
    private Turrel[] turrels;
    private List<Turrel> _newTurrelsList = new List<Turrel>();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        turrels = FindObjectsOfType<Turrel>();
        foreach (var item in turrels)
        {
            _newTurrelsList.Add(item);
        }
    }

    void Update()
    {
        _moveVertical = Input.GetAxis("Vertical") * _verticalMoveSpeed;
        _moveHorizontal = Input.GetAxis("Horizontal") * _horizontalMoveSpeed;
    }

    public void Movement()
    {
        _movement = transform.forward * _moveVertical * _verticalMoveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + _movement);
    }
    public void Rotate()
    {
        _turn = _moveHorizontal * _horizontalMoveSpeed * Time.deltaTime;
        _turnRotation = Quaternion.Euler(0f, _turn, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * _turnRotation);
    }
    public void AutoTurretRotate()
    {
        closest = GetClosestTurrel(_turret.position);
        if (closest != null)
        {
            if (!closest.gameObject.activeInHierarchy)
            {
                turrels = FindObjectsOfType<Turrel>();
                foreach (var item in turrels)
                {
                    _newTurrelsList.Add(item);
                }
                closest = GetClosestTurrel(_turret.position);
            }

            var direction = closest.transform.position + _directionOffset;
            _turret.LookAt(direction);
        }
        else
            _turret.rotation = new Quaternion(0, 0, 0, 0);

    }

    private Turrel GetClosestTurrel(Vector3 point)
    {
        float minDistance = Mathf.Infinity;
        Turrel closestTurrel = null;
        foreach (var item in turrels)
        {
            if (!item.gameObject.activeInHierarchy)
                _newTurrelsList.Remove(item);
        }

        for (int t = 0; t < _newTurrelsList.Count; t++)
        {
            float distance = Vector3.Distance(point, _newTurrelsList[t].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTurrel = _newTurrelsList[t];
            }
        }

        return closestTurrel;
    }
}
