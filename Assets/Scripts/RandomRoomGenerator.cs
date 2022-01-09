using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoomGenerator : MonoBehaviour
{
    public int depth = 5;
    public int currentDepth = 5;
    public GameObject room;
    public GameObject[] roomBlockers;
    public GameObject[] roomLayouts;
    public GameObject loreRoom;
    public GameObject exitRoom;
    public static GameObject exitRoomStatic;
    public static GameObject loreRoomStatic;
    public bool isStarter = false;

    public void Start()
    {
        if (isStarter)
        {
            loreRoomStatic = loreRoom;
            exitRoomStatic = exitRoom;
            generate(0, -1);
        }
    }

    public void setup(GameObject room, GameObject[] roomBlockers, int currentDepth, GameObject[] roomLayouts)
    {
        this.currentDepth = currentDepth;
        this.room = room;
        this.roomBlockers = roomBlockers;
        this.roomLayouts = roomLayouts;
    }

    public void generate(int ignoreSide, int roomIgnore)
    {
        int[] sides = new int[4];
        GameObject[] rooms = new GameObject[4];
        int[] layouts = new int[4];

        if (Random.Range(1, depth) <= currentDepth)
        {
            Collider2D coll = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-8, 14), (Vector2)transform.position + new Vector2(8, 6));
            if (coll == null)
            {
                GameObject uRoom = Instantiate(room, (Vector2)transform.position + new Vector2(0, 10), Quaternion.identity);
                if (currentDepth == 1 && exitRoomStatic != null)
                {
                    Instantiate(exitRoomStatic, (Vector2)transform.position + new Vector2(0, 10), Quaternion.identity);
                    exitRoomStatic = null;
                }
                else if (currentDepth == 1 && loreRoomStatic != null)
                {
                    Instantiate(loreRoomStatic, (Vector2)transform.position + new Vector2(0, 10), Quaternion.identity);
                    loreRoomStatic = null;
                }
                else {
                    layouts[0] = getRoomLayout(roomIgnore);
                    Instantiate(roomLayouts[layouts[0]], (Vector2)transform.position + new Vector2(0, 10), Quaternion.identity); 
                }

                rooms[0] = uRoom;
                sides[0] = 3;
            }
            else
            {
                if (ignoreSide != 1)
                    Instantiate(roomBlockers[0], transform);
            }
        }
        else { if (ignoreSide != 1) Instantiate(roomBlockers[0], transform); }
        if (Random.Range(1, depth) <= currentDepth)
        {
            Collider2D coll = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(10, -4), (Vector2)transform.position + new Vector2(26, 4));
            if (coll == null)
            {
                GameObject uRoom = Instantiate(room, (Vector2)transform.position + new Vector2(18, 0), Quaternion.identity);
                if (currentDepth == 1 && exitRoomStatic != null)
                {
                    Instantiate(exitRoomStatic, (Vector2)transform.position + new Vector2(18, 0), Quaternion.identity);
                    exitRoomStatic = null;
                }
                else if (currentDepth == 1 && loreRoomStatic != null)
                {
                    Instantiate(loreRoomStatic, (Vector2)transform.position + new Vector2(18, 0), Quaternion.identity);
                    loreRoomStatic = null;
                }
                else
                {
                    layouts[1] = getRoomLayout(roomIgnore);
                    Instantiate(roomLayouts[layouts[1]], (Vector2)transform.position + new Vector2(18, 0), Quaternion.identity);
                }
                rooms[1] = uRoom;
                sides[1] = 4;
            }
            else
            {
                if (ignoreSide != 2)
                    Instantiate(roomBlockers[1], transform);
            }
        }
        else { if (ignoreSide != 2) Instantiate(roomBlockers[1], transform); }
        if (Random.Range(1, depth) <= currentDepth)
        {
            Collider2D coll = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-8, -14), (Vector2)transform.position + new Vector2(8, -6));
            if (coll == null)
            {
                GameObject uRoom = Instantiate(room, (Vector2)transform.position + new Vector2(0, -10), Quaternion.identity);
                if (currentDepth == 1 && exitRoomStatic != null)
                {
                    Instantiate(exitRoomStatic, (Vector2)transform.position + new Vector2(0, -10), Quaternion.identity);
                    exitRoomStatic = null;
                }
                else if (currentDepth == 1 && loreRoomStatic != null)
                {
                    Instantiate(loreRoomStatic, (Vector2)transform.position + new Vector2(0, -10), Quaternion.identity);
                    loreRoomStatic = null;
                }
                else
                {
                    layouts[2] = getRoomLayout(roomIgnore);
                    Instantiate(roomLayouts[layouts[2]], (Vector2)transform.position + new Vector2(0, -10), Quaternion.identity);
                }
                rooms[2] = uRoom;
                sides[2] = 1;
            }
            else
            {
                if (ignoreSide != 3)
                    Instantiate(roomBlockers[2], transform);
            }
        }
        else { if (ignoreSide != 3) Instantiate(roomBlockers[2], transform); }
        if (Random.Range(1, depth) <= currentDepth)
        {
            Collider2D coll = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-26, 4), (Vector2)transform.position + new Vector2(-10, -4));
            if (coll == null)
            {
                GameObject uRoom = Instantiate(room, (Vector2)transform.position + new Vector2(-18, 0), Quaternion.identity);
                if (currentDepth == 1 && exitRoomStatic != null)
                {
                    Instantiate(exitRoomStatic, (Vector2)transform.position + new Vector2(-18, 0), Quaternion.identity);
                    exitRoomStatic = null;
                }
                else if (currentDepth == 1 && loreRoomStatic != null)
                {
                    Instantiate(loreRoomStatic, (Vector2)transform.position + new Vector2(-18, 0), Quaternion.identity);
                    loreRoomStatic = null;
                }
                else
                {
                    layouts[3] = getRoomLayout(roomIgnore);
                    Instantiate(roomLayouts[layouts[3]], (Vector2)transform.position + new Vector2(-18, 0), Quaternion.identity);
                }
                rooms[3] = uRoom;
                sides[3] = 2;
            }
            else
            {

                if (ignoreSide != 4)
                    Instantiate(roomBlockers[3], transform);
            }
        }
        else { if (ignoreSide != 4) Instantiate(roomBlockers[3], transform); }

        if (currentDepth == 0) return;
        for (int i = 0; i < 4; i++)
        {
            if (rooms[i] != null)
            {
                RandomRoomGenerator generator = (RandomRoomGenerator)rooms[i].AddComponent(typeof(RandomRoomGenerator));

                generator.setup(room, roomBlockers, currentDepth - 1, roomLayouts);
                generator.generate(sides[i], layouts[i]);
            }
        }
    }

    public int getRoomLayout(int ignore)
    {
        int n = Random.Range(0, roomLayouts.Length);

        if (n == ignore && n == roomLayouts.Length - 1) return n - 1;
        if (n == ignore) return n + 1;
        return n;
    }
}
