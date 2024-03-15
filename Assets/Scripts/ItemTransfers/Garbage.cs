using DG.Tweening;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    public void TakeItem(Item item)
    {
        item.transform.SetParent(null);

        Vector3 targetPos = transform.position;

        item.transform.DOLocalJump(targetPos, 2f, 1, .25f).SetEase(Ease.InOutQuad);
        item.transform.DOLocalRotate(Vector3.zero, .25f).SetEase(Ease.InSine).OnComplete(() => ItemObjectPoolManager.Instance.ReturnObject(item.Type,item));
    }
}
