﻿using UnityEngine;

public static class Vector3Ext {
    public static Vector3 WithX(this Vector3 vector, float value) {
        return new Vector3(value, vector.y, vector.z);
    }
    
    public static Vector3 WithY(this Vector3 vector, float value) {
        return new Vector3(vector.x, value, vector.z);
    }
    
    public static Vector3 WithZ(this Vector3 vector, float value) {
        return new Vector3(vector.x, vector.y, value);
    }
}