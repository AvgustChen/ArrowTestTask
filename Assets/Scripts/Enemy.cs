using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;


    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "death")
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "death", false);
    }

    private void OnDestroy()
    {
        skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;
    }


}

