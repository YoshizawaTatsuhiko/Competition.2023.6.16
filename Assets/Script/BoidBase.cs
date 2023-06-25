using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class BoidBase : MonoBehaviour
{
    public Parameter Param { get; set; }
    public Vector3 CenterOfSimulationArea { get; set; }
    public Vector3 Position { get; protected set; }
    public Vector3 Velocity { get; protected set; }
    public LayerMask LayerMask { get; set; }

    /// <summary>自身の移動に使う加速度</summary>
    protected Vector3 Accel = Vector3.zero;
}
