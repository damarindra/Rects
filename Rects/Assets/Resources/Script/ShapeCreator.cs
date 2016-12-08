using UnityEngine;
using System.Collections;

public class ShapeCreator : MonoBehaviour{

    public GameObject rectangle;
    [HideInInspector]
    public Color[] colorRectangle = new Color[6];
    public MultidimensionalColor[] colorRectArray;
    [HideInInspector]
    public GameObject[] gameobjectRectangle = new GameObject[6];
	public MultidimensionGameObject[] gameobjectRectangleArray = new MultidimensionGameObject[1] { new MultidimensionGameObject(6)};        //if you want add theme, change the size array

    [HideInInspector]
    public Color backgroundColor;
    public Color[] backgroundColorArray;
    [HideInInspector]public Color textColor;
    public Color[] textColorArray;


    public enum Direction
    {
        horizontal,vertical
    }
    public enum LetterType
    {
        type1, type2, type3, type4
    }
    public enum RectType {
        color, sprite
    }
    [HideInInspector]
    public RectType rectType = RectType.color;

    public void InitializeSprite()
    {
        gameobjectRectangleArray[0].SetSprite = new GameObject[6] {
            Resources.Load("Prefabs/Theme/Shapes/Circle", typeof(GameObject)) as GameObject,
            Resources.Load("Prefabs/Theme/Shapes/Triangle", typeof(GameObject)) as GameObject,
            Resources.Load("Prefabs/Theme/Shapes/6angle", typeof(GameObject)) as GameObject,
            Resources.Load("Prefabs/Theme/Shapes/Cross", typeof(GameObject)) as GameObject,
            Resources.Load("Prefabs/Theme/Shapes/Plus", typeof(GameObject)) as GameObject,
            Resources.Load("Prefabs/Theme/Shapes/Star", typeof(GameObject)) as GameObject
        };
    }
    protected virtual void Awake()
    {
        InitializeSprite();
    }

    public GameObject Box(int size)
    {
        Vector2 positionToInstantiate = Vector2.zero;
        Transform boxParent = new GameObject("Box").transform;
        Vector2 centerPos =  new Vector2(positionToInstantiate.x + (0.5f * (size)), positionToInstantiate.y += (0.5f * (size)));
        Color clr;
        GameObject rectToInstantiate;
        clr = colorRectangle[Random.Range(0, colorRectangle.Length)];
        if (rectType == RectType.color)
        {
            rectToInstantiate = rectangle;
        }
        else
        {
            rectToInstantiate = gameobjectRectangle[Random.Range(0, gameobjectRectangle.Length)];
        }
        boxParent.position = centerPos;
        int x = 0;
        while (x < size)
        {
            int y = 0;
            while (y < size)
            {
                GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                instance.name = "Box" + x + y;
                if (rectType == RectType.color)
                    instance.GetComponent<SpriteRenderer>().color = clr;
                instance.transform.SetParent(boxParent);
                positionToInstantiate.x += 1;
                y++;
                if (y == size)
                {
                    positionToInstantiate.x = 0;
                    positionToInstantiate.y += 1;
                }
            }
            x++;
        }
        return boxParent.gameObject;
    }

    public GameObject Box(int size, Vector2 pos)
    {
        Vector2 positionToInstantiate = pos;
        Transform boxParent = new GameObject("Box").transform;
        Vector2 centerPos = new Vector2(positionToInstantiate.x + (0.5f * (size)), positionToInstantiate.y += (0.5f * (size)));
        Color clr;
        GameObject rectToInstantiate;
        clr = colorRectangle[Random.Range(0, colorRectangle.Length)];
        if (rectType == RectType.color)
        {
            rectToInstantiate = rectangle;
        }
        else
        {
            rectToInstantiate = gameobjectRectangle[Random.Range(0, gameobjectRectangle.Length)];

        }
        boxParent.position = centerPos;
        int x = 0;
        while (x < size)
        {
            int y = 0;
            while (y < size)
            {
                GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                instance.name = "Box" + x + y;
                if (rectType == RectType.color)
                {
                    instance.GetComponent<SpriteRenderer>().color = colorRectangle[Random.Range(0, colorRectangle.Length)];
                instance.GetComponent<SpriteRenderer>().color = clr;
                }

                instance.transform.SetParent(boxParent);
                positionToInstantiate.x += 1;
                y++;
                if (y == size)
                {
                    positionToInstantiate.x = pos.x;
                    positionToInstantiate.y += 1;
                }
            }
            x++;
        }
        return boxParent.gameObject;
    }

    public GameObject Stick(int size, Direction dir)
    {
        Vector2 positionToInstantiate = Vector2.zero;
        Transform stickParent = new GameObject("Stick").transform;
        Color clr;
        GameObject rectToInstantiate;
        clr = colorRectangle[Random.Range(0, colorRectangle.Length)];
        if (rectType == RectType.color)
        {
            rectToInstantiate = rectangle;
        }
        else
        {
            rectToInstantiate = gameobjectRectangle[Random.Range(0, gameobjectRectangle.Length)];
        }
        Vector2 centerPos;
        if (dir == Direction.horizontal)
            centerPos = new Vector2(positionToInstantiate.x + (0.5f * (size)), positionToInstantiate.y);
        else
            centerPos = new Vector2(positionToInstantiate.x, positionToInstantiate.y += (0.5f * (size - 1))); stickParent.position = centerPos;
        int x = 0;
        while (x < size)
        {
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "Stick" + x;
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(stickParent);
            if (dir == Direction.horizontal)
                positionToInstantiate.x += 1;
            else if (dir == Direction.vertical)
                positionToInstantiate.y += 1;
            x++;
        }
        return stickParent.gameObject;
    }

    public GameObject Stick(int size, Direction dir, Vector2 pos)
    {
        Vector2 positionToInstantiate = pos;
        Transform stickParent = new GameObject("Stick").transform;
        Color clr;
        GameObject rectToInstantiate;
        clr = colorRectangle[Random.Range(0, colorRectangle.Length)];
        if (rectType == RectType.color)
        {
            rectToInstantiate = rectangle;
        }
        else
        {
            rectToInstantiate = gameobjectRectangle[Random.Range(0, gameobjectRectangle.Length)];

        }
        Vector2 centerPos;
        if (dir == Direction.horizontal)
            centerPos = new Vector2(positionToInstantiate.x + (0.5f * (size)), positionToInstantiate.y);
        else
            centerPos = new Vector2(positionToInstantiate.x, positionToInstantiate.y += (0.5f * (size - 1)));
        stickParent.position = centerPos;
        int x = 0;
        while (x < size)
        {
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "Stick" + x;
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(stickParent);
            if (dir == Direction.horizontal)
                positionToInstantiate.x += 1;
            else if (dir == Direction.vertical)
                positionToInstantiate.y += 1;
            x++;
        }
        return stickParent.gameObject;
    }

    public GameObject LetterL(int size, LetterType type)
    {
        size -= 1;
        Vector2 positionToInstantiate = Vector2.zero;
        Transform letterLParent = new GameObject("LetterL").transform;
        Vector2 centerPos = new Vector2(positionToInstantiate.x + (0.5f * (size)), positionToInstantiate.y += (0.5f * (size)));
        Color clr;
        GameObject rectToInstantiate;
         clr = colorRectangle[Random.Range(0, colorRectangle.Length)];
        if (rectType == RectType.color)
        {
            rectToInstantiate = rectangle;
        }
        else
        {
            rectToInstantiate = gameobjectRectangle[Random.Range(0, gameobjectRectangle.Length)];
        }
        letterLParent.position = centerPos;
        if (type == LetterType.type1)
        {
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.x += 1;
                    else
                        positionToInstantiate.y += 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if (rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        else if (type == LetterType.type2)
        {
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.y += 1;
                    else
                        positionToInstantiate.x += 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if (rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        else if (type == LetterType.type3)
        {
            positionToInstantiate.y += size;
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.y -= 1;
                    else
                        positionToInstantiate.x += 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if(rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        else if (type == LetterType.type4)
        {
            positionToInstantiate.y += size;
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.x += 1;
                    else
                        positionToInstantiate.y -= 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if(rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        return letterLParent.gameObject;
    }

    public GameObject LetterL(int size, LetterType type, Vector2 pos)
    {
        size -= 1;
        Vector2 positionToInstantiate = pos;
        Transform letterLParent = new GameObject("LetterL").transform;
        Vector2 centerPos = new Vector2(positionToInstantiate.x + (0.5f * (size)), positionToInstantiate.y += (0.5f * (size)));
        Color clr;
        GameObject rectToInstantiate;
        clr = colorRectangle[Random.Range(0, colorRectangle.Length)];
        if (rectType == RectType.color)
        {
            rectToInstantiate = rectangle;
        }
        else
        {
            rectToInstantiate = gameobjectRectangle[Random.Range(0, gameobjectRectangle.Length)];
        }
        letterLParent.position = centerPos;

        if (type == LetterType.type1)
        {
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if(rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.x += 1;
                    else
                        positionToInstantiate.y += 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if(rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        else if (type == LetterType.type2)
        {
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.y += 1;
                    else
                        positionToInstantiate.x += 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if(rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        else if (type == LetterType.type3)
        {
            positionToInstantiate.y += size;
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.y -= 1;
                    else
                        positionToInstantiate.x += 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if(rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        else if (type == LetterType.type4)
        {
            positionToInstantiate.y += size;
            GameObject instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
            instance.name = "LetterL Core";
            if (rectType == RectType.color)
                instance.GetComponent<SpriteRenderer>().color = clr;
            instance.transform.SetParent(letterLParent);
            int x = 0;
            while (x < 2)
            {
                int y = 0;
                while (y < size)
                {
                    if (x == 0)
                        positionToInstantiate.x += 1;
                    else
                        positionToInstantiate.y -= 1;
                    instance = Instantiate(rectToInstantiate, positionToInstantiate, Quaternion.identity) as GameObject;
                    instance.name = "LetterL" + x + y;
                    if(rectType == RectType.color)
                        instance.GetComponent<SpriteRenderer>().color = clr;
                    instance.transform.SetParent(letterLParent);
                    y++;
                }
                x++;
            }
        }
        return letterLParent.gameObject;
    }
}
