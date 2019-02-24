using UnityEngine;
using UnityEngine.AI;

public class PlayerController : InputController
{
    public float walkSpeed = 4;
    public float runSpeed = 8;

    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;

    private Animator animator;
    private NavMeshAgent agent;

    private float currentSpeed;
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;


    void Start()
    {
        base.OnStart();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Move();
        KeyboardActions();

    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
    }

    private Quaternion FaceTargetRotation(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        return Quaternion.LookRotation(lookPos);
    }

    private void Move()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                }
            }
        }

        if (currentDestination)
        {
            actionExecuting = true;

            Quaternion rotationOld = transform.rotation;
            Quaternion rotationNew = Quaternion.Lerp(transform.rotation, FaceTargetRotation(pointToFace), 0.01f);
            Vector3 lookPos = pointToFace - transform.position;
       
            Vector2 player2D = transform.position;
            player2D.x = transform.forward.x;
            player2D.y = transform.forward.z;
            Vector2 destinationPoint = pointToFace;
            destinationPoint.x = (pointToFace - transform.position).x;
            destinationPoint.y = (pointToFace - transform.position).z;
            if ((rotationOld != rotationNew) || !agent.isStopped)
            {
                FaceTarget(pointToFace);
                animator.SetFloat("Speed", 0.2f);
            }
            else
            {
                currentUsingItem.TriggerAnimations();
                currentDestination.InteractMyself();
                currentDestination = null;
                actionExecuting = false;
                animator.SetFloat("Speed", 0f);
            }

        }
        if (PlayerCanMove())
        {
            ManualMove();
        }
            
    }

    private void ManualMove()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        float animtionSpeedPercent = ((running) ? 1 : 0.2f) * inputDir.magnitude;
        if (animator)
        {
            animator.SetFloat("Speed", animtionSpeedPercent, speedSmoothTime, Time.deltaTime);
        }
    }

    public void SetCurrentPickingObject(PickableObject pickable)
    {
        pickingObject = pickable;
    }

}
