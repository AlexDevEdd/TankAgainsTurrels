using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;

    private Bullet _newBullet;

    public void ToShot()
    {
        _newBullet = _bulletPrefab.SetBullet(_bulletPrefab, _bulletSpawn);
        var rigidBody = _newBullet.GetComponent<Rigidbody>();
        rigidBody.velocity = _newBullet.transform.TransformDirection(Vector3.forward * _newBullet.BulletSpeed);
    }
}
