using System.Collections;
using UnityEngine;
public class UseTools
{
    static public float RealVector2Angle(Vector2 _in) //Like, why do you use weird logic, dear Vector2.Angle(...)?
    {
        float _out;
        _out = Mathf.Rad2Deg * Mathf.Atan2(_in.y, _in.x);
        return _out;
    }
}