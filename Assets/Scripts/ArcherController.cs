using Spine;
using Spine.Unity;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    Bone bow, body;
    [SerializeField] Arrow arrow;
    [SerializeField] Transform arrowStartPoint;
    public GameObject trajectoryPointPrefab; 
    public int pointsCount = 30; 
    float force;
    bool isAttack;

    void Start()
    {
        force = 0f;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        bow = skeletonAnimation.Skeleton.FindBone("gun");
        body = skeletonAnimation.Skeleton.FindBone("plevis");

        skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;


        Vector3 boneWorldPosition = body.GetWorldPosition(skeletonAnimation.transform);
        Vector3 direction = mousePosition - boneWorldPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (force < 2)
                force += Time.deltaTime;

            if (!isAttack)
            {
                isAttack = true;
                skeletonAnimation.AnimationState.SetAnimation(0, "attack_start", false);
            }

            bow.Rotation = angle;
            body.Rotation = angle;
            DrawTrajectory(); 
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "attack_finish", false);
            Arrow arrowInstance = Instantiate(arrow, arrowStartPoint.position, arrowStartPoint.rotation);
            arrowInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            arrowInstance.Move(force);
            force = 0;
            ClearDrawTrajectory();
        }
    }

    public void DrawTrajectory()
    {
        ClearDrawTrajectory();

        Vector2 startPosition = arrowStartPoint.position;


        float angle = bow.Rotation * Mathf.Deg2Rad; 
        Vector2 startVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * force * 10f; 

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i * 0.1f;
            Vector2 position = startPosition + startVelocity * t + 0.5f * Physics2D.gravity * t * t;

            
            GameObject point = Instantiate(trajectoryPointPrefab, position, Quaternion.identity);
            point.transform.SetParent(arrowStartPoint); 
        }
    }

    void ClearDrawTrajectory()
    {
        foreach (Transform child in arrowStartPoint)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "attack_finish")
        {
            isAttack = false;
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        }
    }

    private void OnDestroy()
    {
        skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;
    }


}

