using UnityEngine;

public class RootableProjectile : MonoBehaviour, IRootable
{
    public LayerMask groundMask;
    public float speed = 10f;
    public Vector2 direction = Vector2.right;
    
    private Rigidbody2D body;
    private bool isRooted;
    
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if( (groundMask & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("hit ground");
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
    public void RootInPlace()
    {
        isRooted = true;
        GetComponent<Collider2D>().isTrigger = false;
        //Ground Layer
        gameObject.layer = 8;
    }

    public Collider2D GetCollider2D()
    {
        return GetComponent<Collider2D>();
    }

    public bool IsRooted()
    {
        return isRooted;
    }
}
