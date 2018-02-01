using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    //人物组件
    Rigidbody rigib;
    Renderer render;
    Animator animator;
    Collision collisionRet;
    
    public float speed = 10f;//移动速度
    int jumpCount = 0;//跳跃次数
    bool onGround;//是否在地上
    public float jumpForce;
    public float doubleJumpForce;

    public bool isAlice = true;

    public ParticleSystem dieParticle;//死亡特效
    public AudioClip dieSound;//死亡音乐
    public AudioSource groundSound;
    public enum TransColor
    {
        Red,
        Green,
        Undefine,
    }

    TransColor colorCurrent;
  

    public void Awake()
    {
        //获取人物组件
        rigib = GetComponent<Rigidbody>();
        render = GetComponentInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        //collisionRet = GetComponent<Collision>();
        render.material.color = Color.red;
        colorCurrent = TransColor.Red;
    }

    public bool GroundCheck()//在地面的检测
    {
        Collider[] colliders =  Physics.OverlapSphere(transform.position,0.03f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
        //if (Physics.Raycast(transform.position, Vector3.down, 100f))
        //{
        //    Debug.Log(1);
        //    return false;
        //}
        //else
        //{
        //    return true;
        //}
        
    }
    void FixedUpdate()
    {
        if (!isAlice) return;
        if (collisionRet != null)//有碰撞物
        {
            if (collisionRet.gameObject.CompareTag("BuildRed"))
            {
                if (colorCurrent != TransColor.Red)
                {
                    Die();
                }
            }
            else if (collisionRet.gameObject.CompareTag("BuildGreen"))
            {
                if (colorCurrent != TransColor.Green)
                {
                    Die();
                }
            }
            else if (collisionRet.gameObject.CompareTag("BuildUnfind"))
            {
                Die();
            }
        }
        onGround = GroundCheck();
        animator.SetBool("OnGround", onGround);
    }

    public void Move()//人物移动方法
    {
        if (!isAlice) return;
        var vel = rigib.velocity;
        vel.z = (Vector3.forward * speed).z;
        rigib.velocity = vel;
    }
    public void Jump()//人物跳跃方法
    {
        if (!isAlice) return;
        if (jumpCount < 2)
        {
            if (jumpCount == 0)
            {
                rigib.velocity = new Vector3(rigib.velocity.x, 0, rigib.velocity.z);//y轴速度清0，防止有向下的速度影响
                rigib.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            }
            else if (jumpCount == 1)
            {
                rigib.velocity = new Vector3(rigib.velocity.x, 0, rigib.velocity.z);//y轴速度清0，防止有向下的速度影响
                rigib.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            }
        }
        jumpCount++;
    }
    public void ChangeColor()//改变人物颜色
    {
        if (!isAlice) return;

        if (colorCurrent == TransColor.Red)
        {
            colorCurrent = TransColor.Green;
            render.material.color = Color.green;
        }
        else if (colorCurrent == TransColor.Green)
        {
            colorCurrent = TransColor.Red;
            render.material.color = Color.red;
        }

        animator.SetTrigger("ChangeColor");
    }
    public void Die()//人物死亡方法
    {
        isAlice = false;
        render.enabled = false;
        rigib.velocity = Vector3.zero;
        groundSound.Stop();
        dieParticle.Play();
        AudioSource.PlayClipAtPoint(dieSound, transform.position);
        Invoke("Restart", 1);//隔一秒执行Restart()
    }
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public void OnCollisionEnter(Collision coll)
    {
        jumpCount = 0;
        collisionRet = coll;
    }
    private void OnCollisionStay(Collision coll)
    {
        collisionRet = coll;
    }
    private void OnCollisionExit(Collision coll)
    {
        collisionRet = null;
    }
}
