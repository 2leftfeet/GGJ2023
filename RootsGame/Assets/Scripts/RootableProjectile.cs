using UnityEngine;

public class RootableProjectile : Rootable
{
    public LayerMask groundMask;
    public float speed = 10f;
    public Vector2 direction = Vector2.right;
    
    private Rigidbody2D body;
    private bool isRooted;

    public GameObject gibs;
    
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;
    }

    private void Update()
    {
        if(!isRooted) 
        {
           transform.position += (Vector3)(direction * speed * Time.deltaTime);   
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if( (groundMask & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("hit ground");
            if(gibs)
            {
                Instantiate(gibs, transform.position, transform.rotation);
            }
            Destroy(this.gameObject);
        }
        
        if(other.GetComponent<PlayerMovement>())
        {
            Debug.Log("hit player");
            Destroy(this.gameObject);
            //do player hit stuff
        }
    }

    [ContextMenu("Root In Place")]
    public override void RootInPlace()
    {
        isRooted = true;
        GetComponent<Collider2D>().isTrigger = false;
        //Ground Layer
        gameObject.layer = 8;
    }

    public override Collider2D GetCollider2D()
    {
        return GetComponent<Collider2D>();
    }

    public override bool IsRooted()
    {
        return isRooted;
    }

    public override void Unroot()
    {
        isRooted = false;
        GetCollider2D().isTrigger = true;

        gameObject.layer = 0;

        if(rootEffect)
            rootEffect.ResetEffect();
    }
}
