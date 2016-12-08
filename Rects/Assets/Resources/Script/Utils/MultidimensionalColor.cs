using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MultidimensionalColor
{
    public Color[] mColor = new Color[4];

    public Color this[int index]
    {
        get { return mColor[index]; }
        set { mColor[index] = value; }
    }

    public int Length
    {
        get { return mColor.Length; }
    }

    public Color[] GetColor
    {
        get { return mColor; }
    }
}
