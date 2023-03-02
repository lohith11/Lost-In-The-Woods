using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStateManager))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyStateManager Smanager = (EnemyStateManager) target;
        //FieldOfView Smanager = (FieldOfView)target;
        //* Handlees the color of the circle
        Handles.color = Color.white;

        //* This draws the circle with the view radius as the angle around the enemy
        Handles.DrawWireArc(Smanager.transform.position, Vector3.up, Vector3.forward, 360, Smanager.radius);

        //* Calculates the angle for the handles 
        Vector3 viewAngle01 = DirectionFromAngle(Smanager.transform.eulerAngles.y, -Smanager.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(Smanager.transform.eulerAngles.y, Smanager.angle / 2);

        //* Draws the gizmo for the FOV of the enemy
        Handles.color = Color.yellow;
        Handles.DrawLine(Smanager.transform.position, Smanager.transform.position + viewAngle01 * Smanager.radius);
        Handles.DrawLine(Smanager.transform.position, Smanager.transform.position + viewAngle02 * Smanager.radius);

        
        //* This draws a line towards the player
        if (Smanager.playerRef)
        {
            Handles.color = Color.green;
            Handles.DrawLine(Smanager.transform.position, Smanager.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}