using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    protected Rigidbody2D myRigid;
    public SpriteRenderer mySprite;
    protected Animator myAnim;
    public Transform Arm;

    float hp, maxHp;
    float speed, jumpPower;
    protected bool isGround, isJump, isDoubleJump, isMove;

    public enum Weapons { Sword, Magic, Army, Ninja, Fighter };
    public Weapons Weapon;

    public Text NameText;
    public Image HpImage;

    public bool flip_X, isReady;

    Vector3 curPos;

    float count;

    public bool isDisable;

    ScoreBoard scoreBoard;
    int score;

    public PhotonView pv;
    bool isCollection;
    public void Set_Player(int hp, float speed, float jumpPower)
    {
        this.hp = hp;
        maxHp = hp;
        this.speed = speed;
        this.jumpPower = jumpPower;
    }
    [PunRPC]
    public void ScoreUp()
    {
        if (photonView.IsMine)
        {
            score++;
        }
    }

    public void Hit(float damage, PhotonView pv)
    {
        hp -= damage;
        if (hp <= 0)
        {
            photonView.RPC("Die", RpcTarget.AllBuffered);
            pv.RPC("ScoreUp", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Die()
    {
        
        if (photonView.IsMine)
        {
            GameManager.instance.dieWindow.SetActive(true);
            isDisable = true;
        }
        gameObject.SetActive(false);
    }

    void Start()
    {
        isJump = true;
        isMove = true;
        isCollection = false;
        count = 3.99f;
        score = 0;
        myRigid = GetComponent<Rigidbody2D>();
        myAnim = mySprite.GetComponent<Animator>();
        scoreBoard = GameManager.instance.SpawnScoreBoard(Weapon);
        GameManager.instance.Players.Add(this);
        if (photonView.IsMine)
        {
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.LookAt = transform;
            CM.Follow = transform;
            isReady = true;
            GameManager.instance.player = this;
            
        }

        if (Weapon == Weapons.Sword) Set_Player(14, 5, 16);
        else if (Weapon == Weapons.Magic) Set_Player(12, 5, 16);
        else if (Weapon == Weapons.Army) Set_Player(16, 5, 16);
        else if (Weapon == Weapons.Ninja) Set_Player(6, 7, 14);
        else  Set_Player(20, 4, 16);
    }

    void OnEnable()
    {
        if (photonView.IsMine)
        {
            hp = maxHp;
        }
    }

    void Update()
    {
        if (GameManager.instance.isEnd) return;

        NameText.text = photonView.IsMine ? PhotonNetwork.NickName : photonView.Owner.NickName;
        scoreBoard.UpdateInfo(score, photonView.Owner.NickName);
        if (GameManager.instance.isAllReady && PhotonNetwork.IsMasterClient && !GameManager.instance.isStart)
        {
            if(!GameManager.instance.isDisable) photonView.RPC("DisableMapWindow", RpcTarget.AllBuffered);

            count -= Time.deltaTime;
            if (count < 1)
            {
                GameManager.instance.countText.text = "Start";
                GameManager.instance.isStart = true;
                
            }
            else
            {
                GameManager.instance.countText.text = ((int)count).ToString();
            }
        }
        if (!GameManager.instance.isStart) return;

        Move();
        Jump();

        if (!photonView.IsMine)
        {
            if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
            else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        }
        else
        {
            if(Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0,-0.5f), 0.07f, (1 << LayerMask.NameToLayer("Ground"))+(1<<LayerMask.NameToLayer("Scaffolding"))))
            {
                isGround = true;
                isDoubleJump = false;
                isJump = false;

            }
            else
            {
                isGround = false;
            }
        }

        

        if(myRigid.velocity.y > 0)
        {
            gameObject.layer = 7;
        }
        else
        {
            gameObject.layer = 6;
        }

        HpImage.fillAmount = hp / maxHp;

        if(photonView.IsMine && score >= 30 && !isCollection)
        {
            photonView.RPC("Collection_Score", RpcTarget.AllBuffered);
            isCollection = true;
        }
    }

    protected void Move()
    {
        if (photonView.IsMine && isMove)
        {
            float h = Input.GetAxis("Horizontal");
            myRigid.velocity = new Vector2(h * speed, myRigid.velocity.y);

            if (Mathf.Abs(h) > 0.1f) myAnim.SetBool("isMove", true);
            else myAnim.SetBool("isMove", false);

            if (h < 0)
            {
                mySprite.flipX = true;
                Arm.rotation = Quaternion.Euler(Vector2.up * 180);
            }
            else if (h > 0)
            {
                mySprite.flipX = false;
                Arm.rotation = Quaternion.Euler(Vector2.zero);
            }
            
        }
        else
        {
            mySprite.flipX = flip_X;
            if (flip_X)
            {
                Arm.rotation = Quaternion.Euler(Vector2.up * 180);
            }
            else
            {
                Arm.rotation = Quaternion.Euler(Vector2.zero);
            }
        }
        
    }

    protected void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && photonView.IsMine && isMove)
        {
            myRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJump = true;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && Weapon == Weapons.Ninja && !isDoubleJump && photonView.IsMine)
        {
            myRigid.velocity = new Vector2(myRigid.velocity.x, 0);
            myRigid.AddForce(Vector2.up * (jumpPower), ForceMode2D.Impulse);
            isDoubleJump = true;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mySprite.flipX);
            stream.SendNext(hp);
            stream.SendNext(transform.position);
            stream.SendNext(isReady);
            stream.SendNext(count);
            stream.SendNext(isDisable);
            stream.SendNext(score);
        }
        else
        {
            this.flip_X = (bool)stream.ReceiveNext();
            this.hp = (float)stream.ReceiveNext();
            curPos = (Vector3)stream.ReceiveNext();
            isReady = (bool)stream.ReceiveNext();
            Count((float)stream.ReceiveNext());
            DisablePlayer((bool)stream.ReceiveNext());
            score = (int)stream.ReceiveNext();
        }
    }
    void Count(float count)
    {
        if (!GameManager.instance.isStart)
        {
            if (count < 1)
            {
                GameManager.instance.countText.text = "Start";
                GameManager.instance.isStart = true;
            }
            else
            {
                GameManager.instance.countText.text = ((int)count).ToString();
            }
        }
    }
    void DisablePlayer(bool isDisable)
    {

        gameObject.SetActive(!isDisable);
    }
    public void KnockBack(Vector2 dir)
    {
        myRigid.AddForce(dir * 15, ForceMode2D.Impulse);
        isMove = false;
        Invoke("Changed_IsMove", 0.2f);
    }

    void Changed_IsMove()
    {
        isMove = true;
    }

    [PunRPC]
    public void DisableMapWindow()
    {
        GameManager.instance.DisableMapWindow();
    }

    [PunRPC]
    public void Collection_Score()
    {
        if (PhotonNetwork.IsMasterClient && !GameManager.instance.isEnd)
        {
            GameManager.instance.isEnd = true;
            photonView.RPC("DisableGame", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void DisableGame()
    {
        GameManager.instance.End(Weapon, photonView.Owner.NickName);
    }
}
