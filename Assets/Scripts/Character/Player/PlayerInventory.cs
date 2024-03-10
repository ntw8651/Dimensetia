using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update
    //�����غ��ϱ�... �κ��丮�� �����Ϸ���... ��...����? �� script�� �ֹߵ��� �ʳ�?�� �ƴ϶� ������Ʈ ��ü�� �����ϸ� �ȴ����
    //� �� �ϳ� ����Դµ� �̰� �װž� �� �����ۿ��� ���Ÿ� ���� ���⸸ ���� �����ϴ°ž� Ȯ����... �̰� ���� �� ��� �����ֱ�� �� ������� ���� ���λ��� �����ֱ⵵ ����
    public List<Item> items = new List<Item>();

    //FOR DEBUG
    public List<Item> debugAddItems = new List<Item>();
    

    void Start()
    {
        //FOR DEBUG
        foreach (var i in debugAddItems)
        {
            AddItem(i);
        }
        
    }

    public void AddItem(Item item)
    {
        item = Instantiate(item);
        if (item.type == Item.Type.RangedWeapon)
        {
            item.rangedWeaponInfo = Instantiate(item.rangedWeaponInfo);
        }
        else if (item.type == Item.Type.MeleeWeapon)
        {
            item.meleeWeaponInfo = Instantiate(item.meleeWeaponInfo);
        }
        item.isDropped = false;
        item.isEquipped = false;
        
        items.Add(item);
    }

    public void DropItem()
    {
        
    }

    public void SortItem()
    {
        items.Sort();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }


}
