using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 direction;
    [Space]
    [SerializeField] private bool isCrit;
    [SerializeField] private int damage;
    [Space]
    [SerializeField, Range(5f, 15f)] private float speed;
    [SerializeField, Range(5f, 10f)] private float lifespan;
    [Space]
    [SerializeField] private ProjectileDamageType damageType;
    [Space]
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private LayerMask impactLayerMask;

    private IDamageSourceSO damageSource;
    private Rigidbody2D _rigidbody2D;

    public static event EventHandler<OnProjectileEventArgs> OnProjectileImpact;
    public static event EventHandler<OnProjectileEventArgs> OnProjectileLifespanEnd;

    public event EventHandler<OnProjectileEventArgs> OnProjectileSet;

    public class OnProjectileEventArgs : EventArgs
    {
        public int id;
        public IDamageSourceSO damageSource;
        public Vector2 direction;
        public int damage;
        public bool isCrit; 
        public float speed;
        public float lifespan;  
        public ProjectileDamageType damageType;
        public LayerMask targetLayerMask;
        public LayerMask impactLayerMask;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(LifespanCoroutine());
    }

    private void Update()
    {
        HandleMovement();
    }

    public void SetProjectile(IDamageSourceSO damageSource, Vector2 direction, int damage, bool isCrit, float speed, float lifespan, ProjectileDamageType damageType, LayerMask targetLayerMask, LayerMask impactLayerMask)
    {
        this.damageSource = damageSource;
        this.direction = direction;
        this.damage = damage;
        this.isCrit = isCrit;
        this.speed = speed;
        this.lifespan = lifespan;   
        this.damageType = damageType;
        this.targetLayerMask = targetLayerMask;
        this.impactLayerMask = impactLayerMask;

        OnProjectileSet?.Invoke(this, new OnProjectileEventArgs { id = id, damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask });
    }

    private IEnumerator LifespanCoroutine()
    {
        yield return new WaitForSeconds(lifespan);

        EndLifespan();
    }

    private void HandleMovement()
    {
        _rigidbody2D.velocity = direction * speed;
    }

    private void ImpactProjectile()
    {
        OnProjectileImpact?.Invoke(this, new OnProjectileEventArgs { id = id , damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask});
        Destroy(gameObject);
    }

    private void EndLifespan()
    {
        OnProjectileLifespanEnd?.Invoke(this, new OnProjectileEventArgs { id = id, damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask });
        Destroy(gameObject);
    }
}
