using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(LineOfSight))]
public class FOVEditor : Editor
{
   private void OnSceneGUI() 
   {
        LineOfSight LOS = (LineOfSight)target;

        //* Handles the color of the circle
        Handles.color = Color.white;

        //* This draws the circle with the view radius as the angle around the enemy
        Handles.DrawWireArc(LOS.transform.position, Vector3.up, Vector3.forward, 360, LOS.viewRadius);

        //* Calculates the angle for the handles 
        Vector3 viewAngleA = LOS.DirFromAngle (-LOS.viewAngle/2 , false);
        Vector3 viewAngleB = LOS.DirFromAngle (LOS.viewAngle/2 , false);

        //* Draws the gizmo for the FOV of the enemy
        Handles.DrawLine(LOS.transform.position , LOS.transform.position + viewAngleA * LOS.viewRadius);
        Handles.DrawLine(LOS.transform.position , LOS.transform.position + viewAngleB * LOS.viewRadius);

        //* This draws a line towards the player
        Handles.color = Color.red;
        foreach(Transform visibleTarget in LOS.visbleTargets)
        {
            Handles.DrawLine(LOS.transform.position , visibleTarget.transform.position);
        }
   }
}
