using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private Animator animator;
    private Vector3 moveTowards;
    private Vector3 init;
    // Start is called before the first frame update
    void Start()
    {
        init = transform.position;
        animator = GetComponent<Animator>();
        //Every ten seconds this function is called to change the walking direction of animal
        InvokeRepeating("getNewMovePos",0,11f);
    }

    void getNewMovePos()
    {
        //randomizes a new  Vector3 in specific range
        moveTowards = new Vector3(init.x+Random.Range(-15f,5f),transform.position.y,init.z+Random.Range(0f,25f));
    }

    // Update is called once per frame
    void Update()
    {
        //if the distance from each vector3 is significant
        if (Vector3.Distance(moveTowards,transform.position) > 1f)
        {
            Debug.DrawLine(transform.position, moveTowards);
            //start rotating towards new position and moving towards, based on time deltaTime
            transform.position = Vector3.MoveTowards(transform.position,moveTowards,Time.deltaTime*1.2f);
            Vector3 newRot = Vector3.RotateTowards(transform.forward,moveTowards,Time.deltaTime*0.9f,0);
            transform.rotation = Quaternion.LookRotation(newRot);
            

            //begin animation
            animator.SetBool("isWalking",true);

        }
        else animator.SetBool("isWalking",false);
    }
}
