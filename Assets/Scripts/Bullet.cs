using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletDamage = 50f;
    [SerializeField] private float _bulletSpeed = 50f;
    [SerializeField] ParticleSystem _bulletParticle;
    [SerializeField] GameObject _bulletModel;

    private Rigidbody _rigidbody;
    private Bullet _newBullet;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public float BulletSpeed { get => _bulletSpeed; }

    private void OnCollisionEnter(Collision collision)
    {    
       var hit = collision.contacts[0].otherCollider.gameObject.GetComponent<IDamageable<float>>();
        Debug.Log(collision.collider.name);
        if (hit != null)
        {
            hit.Damage(_bulletDamage);
            Debug.Log(collision.collider.name + "damage");
        }
        DestroyBullet();
    } 


    public Bullet SetBullet(Bullet prefab, Transform spawnPosition)
    {
        _newBullet = Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);
        return _newBullet;
    }
    private void DestroyBullet()
    {
        FreezeConstraints();
        _bulletModel.SetActive(false);
        _bulletParticle.gameObject.SetActive(true);
        Destroy(gameObject, 3f);
    }

    private void FreezeConstraints() =>
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;


    
}
