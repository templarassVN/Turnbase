using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    Animator mAnimator;
    static bool to_replace = false;
    bool _isActive = false;
    [SerializeField]
    public GameObject selected, icon;
    [SerializeField]
    SpriteRenderer image;
    [SerializeField]
    Sprite[] icon_list;
    
    public static float size;
    public Vector2 pos;
    int type;
    // Start is called before the first frame update


    public void Setpos(int x, int y)
    {
        pos = new Vector2(x,y);
    }
    void Start()
    {
        type = Random.Range(0, icon_list.Length);
        size = image.bounds.size.x;
        mAnimator = GetComponent<Animator>();
        selected.SetActive(false);
        icon.GetComponent<SpriteRenderer>().sprite = icon_list[type];
    }

    public bool isActive
    {
        get { return _isActive; }
        set { isActive = value; }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        //Debug.Log("Mouse is over GameObject.");
    }

    private void OnMouseDown()
    {
        
        if (!GameGrid.selected_tile)
        {
            GameGrid.selected_tile = this;
        } else
        {
            GameGrid.toreplace_tile = this;
        }
        _isActive = !_isActive;
        selected.SetActive(_isActive);
        mAnimator.SetBool("Active", _isActive);
    }

    public Sprite Icon
    {
        get { return icon.GetComponent<SpriteRenderer>().sprite; }
        set { icon.GetComponent<SpriteRenderer>().sprite = value; }
    }

    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public void Deactive()
    {
        _isActive = false;
        selected.SetActive(_isActive);
        mAnimator.SetBool("Active", _isActive);
    }
}
