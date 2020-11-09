using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterWeapon : MonoBehaviour
{
    [SerializeField]
    Transform BulletSpawnOne;

    [SerializeField]
    Transform BulletSpawnTwo;

    [SerializeField]
    GameObject BulletPrefab;

    [SerializeField]
    float BulletSpeed = 200f;

    [SerializeField]
    float BulletLifeTime = 2;

    [SerializeField]
    ShootTargeter ShootTargeter;

    [SerializeField]
    float BulletCooldown = 0.1f;

    public bool CanShoot = true;

    private List<GameObject> Bullets;

    void Start()
    {
        this.Bullets = new List<GameObject>();
        this.ShootTargeter = this.gameObject.GetComponent<ShootTargeter>();
        _ = this.StartCoroutine(this.StartShootCoroutine());
    }

    private IEnumerator StartShootCoroutine()
    {
        while (this.CanShoot)
        {
            if (this.ShootTargeter.GetTarget() == null)
            {
                yield return null;
            }
            else
            {
                var spawns = new List<Transform>() { this.BulletSpawnOne, this.BulletSpawnTwo };

                spawns.ForEach(spawn =>
                {
                    if (!spawn) return;

                    var bullet = Instantiate(this.BulletPrefab);
                    Physics.IgnoreCollision(
                        bullet.transform.GetChild(0).GetComponent<Collider>(),
                        this.gameObject.GetComponent<Collider>()
                    );
                    bullet.transform.position = spawn.position;
                    var rotation = bullet.transform.rotation.eulerAngles;
                    bullet.transform.position = spawn.position;
                    bullet.transform.rotation = Quaternion.Euler(rotation.x, this.transform.eulerAngles.y, rotation.z);
                    this.Bullets.Add(bullet);
                    _ = this.StartCoroutine(this.BulletMover(bullet, (this.ShootTargeter.GetTarget().position - bullet.transform.position).normalized));
                    _ = this.StartCoroutine(this.BulletCleanupCoroutine(bullet));
                });

                yield return new WaitForSeconds(this.BulletCooldown);
            }
        }
    }

    private IEnumerator BulletMover(GameObject bullet, Vector3 direction)
    {
        while (this.Bullets.Exists(b => b == bullet) && bullet != null)
        {
            yield return new WaitForFixedUpdate();
            try
            {
                bullet.transform.position += direction * this.BulletSpeed * Time.fixedDeltaTime;
            }
            catch { }
        }
    }

    private IEnumerator BulletCleanupCoroutine(GameObject bullet)
    {
        yield return new WaitForSeconds(this.BulletLifeTime);

        if (this.Bullets.Exists(b => b == bullet) && bullet != null)
        {
            try
            {
                _ = this.Bullets.Remove(bullet);
                Destroy(bullet);
            }
            catch { }
        }
    }

    public void DestroyAllBullets()
    {
        this.Bullets.ForEach(b => Destroy(b));
        this.Bullets.Clear();
    }
}
