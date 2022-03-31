using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    int _no_x = 8, _no_y = 8;
    [SerializeField]
    float _x0 = 0, _y0 = 0;
    [SerializeField]
    GameObject init_tile;

    GameObject[,] Tiles_object;
    
   
    [SerializeField]
    int[] tracking;

    bool Checked_lr, Checked_bu;

    public static Tiles selected_tile;
    
    public static Tiles toreplace_tile;
    public static bool allow_select = true;
    static Vector2[] DIRECTION = new Vector2[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down };

    float _speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Tiles_object = new GameObject[_no_x, _no_y];
        tracking = new int[_no_x];
        for (int i = 0; i < _no_x; i++)
        {
            for (int j = 0; j < _no_y; j++)
            {
                Vector3 pos = new Vector3(_x0 + i * Tiles.size, _y0 + j * Tiles.size, 0);
                Tiles_object[i, j] = Instantiate(init_tile, pos, Quaternion.identity);
                Tiles_object[i, j].GetComponent<Tiles>().Setpos(i, j);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            if(toreplace_tile != null)
                if (CheckValid()) 
                {
                    StartCoroutine(MoveTiles( (Vector3)selected_tile.pos, (Vector3)toreplace_tile.pos));
                } else
                {
                    
                    if(selected_tile != null)
                        selected_tile.Deactive();
                    if (toreplace_tile!= null)
                        toreplace_tile.Deactive();
                    toreplace_tile = selected_tile = null;
                }
        }

        if (Input.GetKeyDown(KeyCode.C))
            DetectBottomUp();
    }

    bool CheckValid()
    {
        if (selected_tile == null || toreplace_tile == null)
            return false;
        
        foreach (Vector2 dir in DIRECTION)
        {
            if(selected_tile.pos + dir == toreplace_tile.pos)
                return true;
        }
        return false;
    }

    void SwapTile()
    {
        if (selected_tile == null || toreplace_tile == null)
            return;
        Sprite temp_sp = selected_tile.Icon;
        selected_tile.Icon = toreplace_tile.Icon;
        toreplace_tile.Icon = temp_sp;
        int temp_t = selected_tile.Type;
        selected_tile.Type = toreplace_tile.Type;
        toreplace_tile.Type = temp_t;
    }

    IEnumerator MoveTiles(Vector3 select_pos, Vector3 toreplace_pos)
    {
        while (selected_tile.icon.transform.position != toreplace_pos &&
            toreplace_tile.icon.transform.position != select_pos)
        {
            selected_tile.icon.transform.position =
                Vector3.MoveTowards(selected_tile.icon.transform.position, toreplace_pos, Time.deltaTime*_speed);

            toreplace_tile.icon.transform.position = 
                Vector3.MoveTowards(toreplace_tile.icon.transform.position, select_pos, Time.deltaTime * _speed);
            yield return null;
        }
        Debug.Log("1");
        SwapTile();
        selected_tile.icon.transform.position = select_pos;
        toreplace_tile.icon.transform.position = toreplace_pos;
        selected_tile.Deactive();
        toreplace_tile.Deactive();
        toreplace_tile = selected_tile = null;
    }

    void ReInitTracking(int [] tracking)
    {
        
        for (int i = 0; i < _no_x; i++)
            tracking[i] = 1;
    }

    void DetectMatch()
    {
        DetectBottomUp();
    }

    

    void DetectLeftRight()
    {
        int x_axis, y_axis;
        for (y_axis = 0; y_axis < _no_x; y_axis++)
        {
            ReInitTracking(tracking) ;
            for (x_axis = 1; x_axis < _no_x; x_axis++)
            {
                if (Tiles_object[x_axis-1, y_axis] == null)
                    continue;
                

            }
            //Debug.Log(string.Join(",",tracking));
        }
    }

    IEnumerator Col_TileFalling(int x3, int y3, GameObject[] Tiles_fall)
    {
        
        while (Tiles_fall[Tiles_fall.Length - 1].transform.position != new Vector3(x3, _no_y - 1, 0) * Tiles.size)
        {
            for (int i = 0; i <= Tiles_fall.Length - 1; i++)
            {
                Tiles_fall[i].transform.position =
                    Vector3.MoveTowards(Tiles_fall[i].transform.position,
                    new Vector3(x3, y3 + i - 2, 0) * Tiles.size, Time.deltaTime * _speed);
            }
            yield return null;
        }
        Debug.Log("a"); 
        for (int i = 0; i <= Tiles_fall.Length - 1; i++)
        {
            Tiles_object[x3, y3 - 2 + i] = Tiles_fall[i];
            Tiles_object[x3, y3 - 2 + i].GetComponent<Tiles>().Setpos(x3, y3 - 2 + i);
        }
    }
    void DetectBottomUp()
    {
        int x_axis, y_axis;

        for (x_axis = 0; x_axis < _no_x; x_axis++)
        {
            ReInitTracking(tracking);
            StartCoroutine(ScanColX(x_axis, tracking));
            //Debug.Log(string.Join(",",tracking));
        } 
    }

    IEnumerator ScanColX(int x, int[] col_track)
    {
        int[] temp_tracking = new int[_no_x];
        for (int i = 0; i < _no_x; i++)
            temp_tracking[i] = 1;
        for (int i = 1; i < _no_y; i++)
        {
            if (Tiles_object[x, i].GetComponent<Tiles>().Type ==
                        Tiles_object[x, i - 1].GetComponent<Tiles>().Type)
                temp_tracking[i] += temp_tracking[i - 1];
            if (temp_tracking[i] == 3)
            {
                yield return StartCoroutine(DeleteOnColX(x, i));
                yield return new WaitForSeconds(1f);
                i = 0;
                ReInitTracking(temp_tracking);
            }
        }
    }

    IEnumerator DeleteOnColX(int x, int y)
    {
        GameObject[] move_tiles;
        int index = 0;
        int no_of_tile = _no_x - 1 - y;
        //Count no of tile upward
        move_tiles = new GameObject[no_of_tile + 3];

        for (int i = y + 1; i < _no_x; i++)
        {
            move_tiles[index++] = Tiles_object[x, i];
        }
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = new Vector3(x, _no_y + i * Tiles.size, 0);
            move_tiles[index++] = Instantiate(init_tile, pos, Quaternion.identity);
        }
        for (int i = 0; i < 3; i++)
        {
            Destroy(Tiles_object[x, y - i]);
        }
        yield return StartCoroutine(Col_TileFalling(x, y, move_tiles));
    }
}
