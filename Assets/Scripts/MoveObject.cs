using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private float vel = 2;
    [SerializeField] private float radio = 0.4f;
    [SerializeField] private Vector2 puntoMov;
    [SerializeField] private Vector2 offsetMov;

    [SerializeField] private LayerMask obstaculo;
    private bool moviendo = false;
    private Vector2 input;

    private void Start()
    {
        puntoMov = transform.position;
    }
    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        if (moviendo)
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoMov, vel * Time.deltaTime);
            if(Vector2.Distance(transform.position, puntoMov) == 0)
            {
                moviendo = false;
            }
        }

        if((input.x != 0 || input.y != 0) && !moviendo)
        {
            Vector2 puntoEvaluar = new Vector2(transform.position.x, transform.position.y) + offsetMov + input;
            if (!Physics2D.OverlapCircle(puntoEvaluar, radio, obstaculo))
            {
                moviendo = true;
                puntoMov += input;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoMov + offsetMov, radio);
    }
}
