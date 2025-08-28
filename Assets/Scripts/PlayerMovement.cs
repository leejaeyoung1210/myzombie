using UnityEngine;

//public static class Tags
//{
//    public static readonly string Player = "Player"; 
//}
public class PlayerMovement : MonoBehaviour
{
    //�����޼��忡�� �׸���x 

    public static readonly int hashMove = Animator.StringToHash("Move");

    public float moveSpeed = 5f;
    public float rotateSpeed = 180;
    private Rigidbody rb;
    private PlayerInput input;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();    
    }

    private void FixedUpdate()
    {//�������� �ð����� �ٸ� �ð� ��ŸŽ
     //input.Roate*rotateSpeed* Time.deltaTime
        //var findGo = GameObject.FindWithTag(Tags.Player);

        var rotation = Quaternion.Euler(
           0f, input.Roate * rotateSpeed * Time.deltaTime, 0f);

        rb.MoveRotation(rb.rotation*rotation);
           
        var distance = input.Move * moveSpeed *Time.deltaTime;
        
        rb.MovePosition(transform.position + distance * transform.forward);


        //animator.SetFloat(10, input.Move);
        //animator.SetFloat("Move", input.Move);
        animator.SetFloat(hashMove, input.Move);

    }


}
