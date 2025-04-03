using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    private float speed;
    private float lifetime = 5f; // Время жизни стрелы
    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.Complete += OnAnimationComplete;

    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "attacksh")
        {
            Destroy(gameObject);
        }
    }

    public void Move(float force)
    {
        Destroy(gameObject, lifetime);
        speed = force * 10f;
        Vector2 direction = transform.right;

        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

    }

    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        rb.isKinematic = true;
        transform.SetParent(other.transform);
        skeletonAnimation.AnimationState.SetAnimation(0, "attack", false);
        if (other.CompareTag("Enemy"))
            other.GetComponent<Enemy>().Die();
    }
}
