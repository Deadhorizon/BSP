using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public float maxHeight;
    public float minHeight;
    public float minWidth;
    public float maxWidth;
    public Sprite sprite;
    public static DungeonManager instance;

    public int left;
    public int right;
    public int bottom;
    public int top;
    public void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SplitRoom room = new SplitRoom(left, right, top, bottom);
        room.maxHeight = maxHeight;
        room.maxWidth = maxWidth;
        room.minHeight = minHeight;
        room.minWidth = minWidth;
        room.Split();
        room.Trim();
        room.CreateAndDrawRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
