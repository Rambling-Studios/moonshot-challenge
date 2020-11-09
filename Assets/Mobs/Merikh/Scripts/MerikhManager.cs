
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class MerikhManager : MonoBehaviour
{
    [SerializeField]
    GameObject Bads;

    [SerializeField]
    GameObject BadShooter;

    [SerializeField]
    GameObject BadShield;

    [SerializeField]
    GameObject BadCannon;

    [SerializeField]
    GameObject Moon;

    [SerializeField]
    GameObject TargetsGroup;

    void Start()
    {
        PubSub.Instance.Subscribe(Topics.MOVER_REACHED_THE_END, this.OnMoverReachedTheEnd);
        PubSub.Instance.Subscribe(Topics.BULLET_COLLISION, this.OnBulletCollided);
        _ = this.StartCoroutine(this.SpawnWorker());
    }

    void OnMoverReachedTheEnd(object data)
    {
        _ = this.StartCoroutine(this.DeathAnimation(data as OnMoverReachedTheEndArgs));
    }

    void OnBulletCollided(object data)
    {
        var obj = Instantiate((data as BulletCollisionArgs).OnCollisionParticle);
        obj.transform.position = (data as BulletCollisionArgs).Pos;
        Destroy(obj, 2);
    }


    IEnumerator DeathAnimation(OnMoverReachedTheEndArgs data)
    {
        if (data.GameObject.TryGetComponent<ShooterWeapon>(out var shooterWeapon))
        {
            shooterWeapon.CanShoot = false;
            shooterWeapon.DestroyAllBullets();
        }
        data.GameObject.GetComponent<Mover>().enabled = false;
        data.GameObject.GetComponent<Aimer>().enabled = false;
        data.GameObject.transform.Find("RocketPowerOne").GetComponentInChildren<ParticleSystem>().Stop();
        data.GameObject.transform.Find("RocketPowerTwo").GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(3);
        var entity = data.Target.GetComponent<Entitie>();
        entity.TakeDamage(data.DamageToInflict);
        for (var i = 0; i < data.GameObject.transform.childCount; i += 1)
        {
            var child = data.GameObject.transform.GetChild(i);
            if (child.TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                meshRenderer.enabled = false;
            }
        }
        data.GameObject.GetComponent<ParticleSystem>().Play();
        data.GameObject.transform.Find("RocketPowerOne").GetComponentInChildren<ParticleSystem>().Stop();
        data.GameObject.transform.Find("RocketPowerTwo").GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2);
        Destroy(data.GameObject);

    }

    void OnDestroy()
    {
        PubSub.Instance.Unsubscribe(Topics.MOVER_REACHED_THE_END, this.OnMoverReachedTheEnd);
        PubSub.Instance.Unsubscribe(Topics.BULLET_COLLISION, this.OnBulletCollided);
    }

    IEnumerator SpawnWorker()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            var toSpawnType = (BadType)Enum.GetValues(typeof(BadType)).GetValue(new System.Random().Next(Enum.GetValues(typeof(BadType)).Length));
            // var toSpawnType = (BadType)Enum.GetValues(typeof(BadType)).GetValue(1);
            var toSpawn = toSpawnType == BadType.SHOOTER ? this.BadShooter : toSpawnType == BadType.SHIELD ? this.BadShield : this.BadCannon;
            var bad = Instantiate(toSpawn);
            bad.gameObject.name = $"BAD_{Enum.GetName(typeof(BadType), toSpawnType)}_{Guid.NewGuid()}";
            var vector2 = UnityEngine.Random.insideUnitCircle.normalized * 200;
            bad.transform.position = new Vector3(vector2.x, 0, vector2.y);
            bad.transform.parent = this.Bads.transform;
            bad.transform.LookAt(this.Moon.transform.GetChild(0).transform.position);
            bad.GetComponent<ShootTargeter>().TargetsGroup = (toSpawnType == BadType.CANNON || toSpawnType == BadType.SHIELD) ? this.Moon : this.TargetsGroup;
            bad.GetComponent<Mover>().Target = this.Moon.transform.GetChild(0).transform;
        }
    }

    internal class OnMoverReachedTheEndArgs
    {
        public int DamageToInflict;
        public GameObject GameObject;
        public Transform Target;
    }

    internal class BulletCollisionArgs
    {
        public GameObject OnCollisionParticle;
        public Vector3 Pos;
    }

    internal enum BadType
    {
        SHOOTER,
        CANNON,
        SHIELD
    }
}
