using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : EntityComponent
{
    public string HorizontalInput = "Horizontal";
    public string VerticalInput = "Vertical";
    private Player p;

    private void Start()
    {
        p = GetComponent<Player>();
    }

    void Movement()
    {
        float xInput = Input.GetAxisRaw(HorizontalInput);
        float yInput = Input.GetAxisRaw(VerticalInput);
        Vector3 movement = new Vector3(xInput, 0.0f, yInput);
        float angle;
        if (xInput != 0.0f || yInput != 0.0f)
        {
            angle = Mathf.Atan2(yInput, xInput) * Mathf.Rad2Deg;
            if ((angle < 45 && angle >= 0) || (angle >= -45 && angle <= 0)) // droite
            {
                p.LeftAnim.SetActive(false);
                p.FaceAnim.SetActive(false);
                p.RightAnim.SetActive(true);
                p.ImoAnim.SetActive(false);
            }
            else if (angle < -135 || angle > 135) // gauche
            {
                p.LeftAnim.SetActive(true);
                p.FaceAnim.SetActive(false);
                p.RightAnim.SetActive(false);
                p.ImoAnim.SetActive(false);
            }
            else //face
            {
                p.LeftAnim.SetActive(false);
                p.FaceAnim.SetActive(true);
                p.RightAnim.SetActive(false);
                p.ImoAnim.SetActive(false);
            }
        }
        else
        {
            p.ImoAnim.SetActive(true);
            p.LeftAnim.SetActive(false);
            p.FaceAnim.SetActive(false);
            p.RightAnim.SetActive(false);
        }
        transform.Translate(movement * p.speed * Time.deltaTime, Space.World);
        if ((xInput > 0.1f || xInput < -0.1f) || (yInput > 0.1f || yInput < -0.1f))
            transform.rotation = Quaternion.LookRotation(movement);
    }

    void Update()
    {
        if (!p.isStun)
        {
            Movement();
        }
    }
}
