using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Animator animator;
    public Animator animatorCabeza;


    [SerializeField] public float SpeedMovement;
    [SerializeField] public float CooldownAtack;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimientocuerpo();
        MovimientoCabeza();
    }



    public void Movimientocuerpo() {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);


        transform.position = transform.position + movement * SpeedMovement;
    }

    public void MovimientoCabeza() {

        Vector3 movementx = Input.mousePosition;
        movementx = Camera.main.ViewportToWorldPoint(movementx);

       
        animatorCabeza.SetFloat("Horizontal", movementx.x);
        animatorCabeza.SetFloat("Vertical", movementx.y);

        

        //transform.position = transform.position + movementx * Time.deltaTime;



    }
}
