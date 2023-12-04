using System.Collections;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public int i, j;
    public Items item;
    public int speed = 5;
    public bool cantClick;
    public void SetItem(Items newItem)
    {
        item = newItem;
        SetSprite();
        SetName();
    }
    void SetSprite()
    {
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
    void SetName()
    {
        this.name = item.name;
    }
    public void SetPosition(int new_i, int new_j)
    {
        i = new_i;
        j = new_j;
    }
    public IEnumerator DropDown()
    {
        cantClick = true;
        while(transform.localPosition.y > 0)
        {
            yield return new WaitForSeconds(0.001f);
            transform.position -= new Vector3(0, 1, 0) * speed * Time.deltaTime;
        }
        transform.localPosition = Vector3.zero;
        cantClick = false;
    }
}
