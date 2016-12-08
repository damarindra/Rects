using UnityEngine;
using System.Collections;

public class MultidimensionGameObject {

    public GameObject[] mGameobject = new GameObject[4];
    public MultidimensionGameObject(int size)
    {
        mGameobject = new GameObject[size];
    }


    public GameObject this[int index]
    {
        get { return mGameobject[index]; }
        set { mGameobject[index] = value; }
    }

    public int Length
    {
        get { return mGameobject.Length; }
    }

    public GameObject[] GetSprite
    {
        get { return mGameobject; }
    }
    public GameObject[] SetSprite
    {
        set { mGameobject = value; }
    }
}
