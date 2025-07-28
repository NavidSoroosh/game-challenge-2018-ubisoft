using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class CharacterThrow : EntityComponent
{
    public string HorizontalInput = "HorizontalRightP1";
    public string VerticalInput = "VerticalRightP1";
    public float ThrowStrenght = 1.0f;
    public GameObject Hitpoint;
    public float UpdateRate = 0.1f;

    private LineRenderer m_lr;
    private bool m_isAiming = false;
    private Vector3 currentPos;
    private Vector3 hitpos;
    private Vector3 m_Force;
    private float lastUpdate;
    private Inventory m_inventory;

    private void Start()
    {
        m_lr = GetComponent<LineRenderer>();
        m_lr.useWorldSpace = true;
        m_inventory = GetComponent<Inventory>();
    }

    void UpdateTrajectory(Vector3 startPos, Vector3 direction, float speed, float timePerSegmentInSeconds, float maxTravelDistance)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 lastPos = startPos;

        positions.Add(startPos);

        float traveledDistance = 0.0f;
        while (traveledDistance < maxTravelDistance)
        {
            traveledDistance += speed * timePerSegmentInSeconds;
            bool hasHitSomething = TravelTrajectorySegment(currentPos, direction, speed, timePerSegmentInSeconds, positions);
            if (hasHitSomething)
            {
                break;
            }
            lastPos = currentPos;
            currentPos = positions[positions.Count - 1];
            direction = currentPos - lastPos;
            direction.Normalize();
        }

        BuildTrajectoryLine(positions);
    }

    bool TravelTrajectorySegment(Vector3 startPos, Vector3 direction, float speed, float timePerSegmentInSeconds, List<Vector3> positions)
    {
        var newPos = startPos + direction * speed * timePerSegmentInSeconds + Physics.gravity * timePerSegmentInSeconds;

        RaycastHit hitInfo;
        var hasHitSomething = Physics.Linecast(startPos, newPos, out hitInfo);
        if (hasHitSomething)
        {
            hitpos = hitInfo.point;
            newPos = hitInfo.point;
        }
        positions.Add(newPos);

        return hasHitSomething;
    }

    void BuildTrajectoryLine(List<Vector3> positions)
    {
        m_lr.positionCount = positions.Count;
        for (var i = 0; i < positions.Count; ++i)
        {
            m_lr.SetPosition(i, positions[i]);
        }
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw(HorizontalInput);
        float yInput = Input.GetAxisRaw(VerticalInput);
        Vector3 movement = new Vector3(xInput, 2.0f, -yInput);
        m_lr.positionCount = 0;
        currentPos = transform.position;
        if ((xInput > 0.2f || xInput < -0.2f) || (yInput > 0.2f || yInput < -0.2f))
        {
            if (m_inventory.CurrentItem() != null)
            {
                m_lr.enabled = true;
                float speed = movement.magnitude * ThrowStrenght;
                UpdateTrajectory(transform.position, movement, speed, 0.05f, 100.0f);
                Hitpoint.SetActive(true);
                Hitpoint.transform.position = hitpos;
                m_isAiming = true;
                if (Time.time - lastUpdate > UpdateRate)
                {
                    m_Force = movement * speed;
                    lastUpdate = Time.time;
                }
            }
        }
        else
        {
            m_lr.enabled = false;
            Hitpoint.SetActive(false);
            hitpos = Vector3.zero;
            if (m_isAiming) // Add CD
            {
                if (m_inventory.CurrentItem() != null)
                {
                    var proj = Instantiate(m_inventory.CurrentItem(), transform.position, transform.rotation);
                    proj.GetComponent<Rigidbody>().AddForce(m_Force * 8);
                    m_inventory.RemoveItem();
                }
            }
            m_isAiming = false;
        }
    }
}
