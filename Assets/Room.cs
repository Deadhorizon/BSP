using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro.EditorUtilities;
using UnityEngine;

public class Room
{
    public int left, right, top, bottom;
    Sprite sprite;
    public Room(int _left, int _right, int _top, int _bottom)
    {
        left = _left;
        right = _right;
        top = _top;
        bottom = _bottom;
    }

    protected int GetWidth()
    {
        return right - left + 1;
    }
    protected int GetHeight()
    {
        return top - bottom  + 1;
    }

    public virtual GameObject CreateAndDrawRoom()
    {
        GameObject room = new GameObject("Room");
        Color someColor = Random.ColorHSV();
        for(int i = left;  i < right; i++)
        {
            for(int j = bottom; j < top; j++)
            {
                GameObject el = new GameObject("el");
                el.transform.position = new Vector3(i, j, 0);
                el.transform.localScale = Vector3.one;
                el.transform.SetParent(room.transform);

                SpriteRenderer render = el.AddComponent<SpriteRenderer>();
                render.sprite = DungeonManager.instance.sprite;
                render.color = someColor;
            }
        }
        return room;
    }
}

public class SplitRoom : Room
{
    public float maxHeight = DungeonManager.instance.maxHeight;
    public float minHeight = DungeonManager.instance.minHeight;
    public float minWidth = DungeonManager.instance.maxWidth;
    public float maxWidth = DungeonManager.instance.minHeight;

    public  int borders = 2;
    public int walls = 1;
    public int coridoresOffset = 1;
    public int minCorridorThickness = 2;

    private bool horizontalSplit;
    private bool verticalSplit;

    private SplitRoom leftRoom;
    private SplitRoom rightRoom;

   

    public SplitRoom(int left, int right, int top, int bottom) : base(left, right, top, bottom)
    {
        horizontalSplit = false;
        verticalSplit = false;

        leftRoom = null;
        rightRoom = null;
    }

    public bool IsLeaf()
    {
        return (horizontalSplit == false) && (verticalSplit == false); 
    }

    public void Split()
    {
        float rand = Random.value;
        if(rand < 0.5f && GetWidth() >= 1.25f*minWidth)
        {
            VerticalSplit();
            return;
        }
        else if(GetHeight() >= 1.25f * minHeight)
        { 
            HorizontalSplit();
            return;
        }
        if(GetWidth() > maxWidth)
        {
            VerticalSplit();
            return;
        }
        if(GetHeight() > maxHeight)
        {
            HorizontalSplit();
            return;
        }
    }

    private void VerticalSplit()
    {
        verticalSplit = true;
        SplitRoom leftEl = new SplitRoom(left ,right - GetWidth() / 2, top, bottom);
        SplitRoom rightEl = new SplitRoom(left + GetWidth() / 2, right, top, bottom);
        leftRoom = leftEl;
        rightRoom = rightEl;
        
        leftRoom.Split();
        rightRoom.Split();
    }

    private void HorizontalSplit()
    {
        horizontalSplit = true;
        SplitRoom topEl = new SplitRoom(left, right, top,bottom + GetHeight() / 2);
        SplitRoom bottomEl = new SplitRoom(left, right,top - GetHeight() / 2, bottom);
        leftRoom = topEl;
        rightRoom = bottomEl;
        leftRoom.Split();
        rightRoom.Split();

    }

    public void Trim()
    {
        left += borders;
        right -= borders;
        top -= borders;
        bottom += borders;
        if(leftRoom != null)
        {
            leftRoom.Trim();
        }
        if(rightRoom != null)
        {
            rightRoom.Trim();
        }    
    }


    public List<int> GetRightConnections()
    {
        List<int> conns = new List<int>();

        if(!IsLeaf())
        {
            if(rightRoom != null)
            {
                conns.AddRange(rightRoom.GetRightConnections());
            }
            if(horizontalSplit && leftRoom != null)
            {
                conns.AddRange(rightRoom.GetRightConnections());
            }
        }
        else
        {
            for(int i = bottom + borders; i <= top - borders; i++)
            {
                conns.Add(i);
            }
        }
        return conns;
    }
    public List<int> GetLeftConnections()
    {
        List<int> conns = new List<int>();

        if (!IsLeaf())
        {
            if (leftRoom != null)
            {
                conns.AddRange(leftRoom.GetLeftConnections());
            }
            if (horizontalSplit && rightRoom != null)
            {
                conns.AddRange(leftRoom.GetLeftConnections());
            }
        }
        else
        {
            for (int i = bottom + borders; i <= top - borders; i++)
            {
                conns.Add(i);
            }
        }
        return conns;
    }
    public List<int> GetTopConnections()
    {
        List<int> conns = new List<int>();

        if (!IsLeaf())
        {
            if (leftRoom != null)
            {
                conns.AddRange(leftRoom.GetTopConnections());
            }
            if (horizontalSplit && rightRoom != null)
            {
                conns.AddRange(leftRoom.GetTopConnections());
            }
        }
        else
        {
            for (int i = left + borders; i <= right - borders; i++)
            {
                conns.Add(i);
            }
        }
        return conns;
    }
    public List<int> GetBottomConnections()
    {
        List<int> conns = new List<int>();

        if (!IsLeaf())
        {
            if (leftRoom != null)
            {
                conns.AddRange(leftRoom.GetBottomConnections());
            }
            if (horizontalSplit && rightRoom != null)
            {
                conns.AddRange(leftRoom.GetBottomConnections());
            }
        }
        else
        {
            for (int i = left + borders; i <= right - borders; i++)
            {
                conns.Add(i);
            }
        }
        return conns;
    }
    public override GameObject CreateAndDrawRoom()
    {
        if(IsLeaf())
        {
            return base.CreateAndDrawRoom();
        }
        else
        {
            leftRoom.CreateAndDrawRoom();
            rightRoom.CreateAndDrawRoom();
            return null;
        }
    }
}