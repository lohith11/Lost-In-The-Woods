using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlindBrute))]
public class BossFOV : Editor
{
    private void OnSceneGUI()
    {
        BlindBrute brute = (BlindBrute)target;
        //* Handlees the color of the circle
        Handles.color = Color.white;

        //* This draws the circle with the view radius as the angle around the enemy
        Handles.DrawWireArc(brute.transform.position, Vector3.up, Vector3.forward, 360, brute.radius);
        Handles.color = Color.blue;
        Handles.DrawWireArc(brute.transform.position, Vector3.up, Vector3.forward, 360, brute.attackRadius);
        Handles.color = Color.black;
        Handles.DrawWireArc(brute.transform.position, Vector3.up, Vector3.forward, 360, brute.attackRadius);

        //* Calculates the angle for the handles 
        Vector3 viewAngle01 = DirectionFromAngle(brute.transform.eulerAngles.y, -brute.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(brute.transform.eulerAngles.y, brute.angle / 2);

        //* Draws the gizmo for the FOV of the enemy
        Handles.color = Color.yellow;

        Handles.DrawLine(brute.transform.position, brute.transform.position + viewAngle01 * brute.radius);
        Handles.DrawLine(brute.transform.position, brute.transform.position + viewAngle02 * brute.radius);


        //* This draws a line towards the player

        if (brute.playerStateMachine)
        {
            Handles.color = Color.green;
            Handles.DrawLine(brute.transform.position, brute.playerStateMachine.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


}