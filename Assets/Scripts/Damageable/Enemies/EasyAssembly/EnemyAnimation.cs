using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    protected Animator animator;
    protected Vector2 animationAxis;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public AnimatorStateInfo GetAnimatorStateInfo(){
        return animator.GetCurrentAnimatorStateInfo(0);
    }

    public void UpdateAnimationAxis(Vector3 positionMoved){
        
        Vector3 localDirection = transform.InverseTransformDirection(positionMoved).normalized;

        Vector2 animationAxisObjective =  new Vector2(localDirection.x*-1, localDirection.z*-1);
        animationAxis = Vector2.Lerp(animationAxis, animationAxisObjective, 0.1f);

    }


    public void SetAnimationAxis(){
        animator.SetFloat("y", animationAxis.y);
        animator.SetFloat("x", animationAxis.x);
    }
}
