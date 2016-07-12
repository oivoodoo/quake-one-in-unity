using System;

using UnityEngine;

public class trigger_teleport : trigger
{
    [SerializeField]
    info_teleport_destination m_destination;

    protected override void OnCharacterEnter(CharacterController character)
    {
        Rigidbody rigidbody = character.GetComponent<Rigidbody>();
        rigidbody.Sleep();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        character.transform.position = m_destination.transform.position + 0.5f * character.height * Vector3.up;
        rigidbody.WakeUp();
    }

    protected override void DrawGizmos(bool selected)
    {
        base.DrawGizmos(selected);

        if (selected && m_destination != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, m_destination.transform.position);
        }
    }

    public info_teleport_destination destination
    {
        get { return m_destination; }
        set { m_destination = value;  }
    }

}
