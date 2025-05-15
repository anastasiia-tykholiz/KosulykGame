using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float _damage = 5f;

    [SerializeField] private Transform _firstTransform;
    [SerializeField] private Transform _secondTransform;

    private Transform _target;

    private Animator _animator;

    private Stats _stats;
    private bool _canDecreaseHealth;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _target = _firstTransform;
    }

    private void Update()
    {
        DetermineMovementDirection();
        Move();
        TryDecreaseHealth();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_target.position.x, transform.position.y), speed * Time.deltaTime);
    }
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    private void DetermineMovementDirection()
    {
        float distanceToLeft = Vector2.Distance(transform.position, _firstTransform.position);
        float distanceToRight = Vector2.Distance(transform.position, _secondTransform.position);

        if (transform.position.x == _target.position.x)
        {
            if (distanceToLeft > distanceToRight)
            {
                _target = _firstTransform;
                Flip();
            }
            else if (distanceToLeft < distanceToRight)
            {
                _target = _secondTransform;
                Flip();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Stats stats))
        {
            _stats = stats;
            _canDecreaseHealth = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Stats stats))
        {
            _canDecreaseHealth = false;
        }
    }
    private void TryDecreaseHealth()
    {
        if (_canDecreaseHealth == true)
        {
            _stats.DecreaseHealth(_damage);
        }
    }
}
