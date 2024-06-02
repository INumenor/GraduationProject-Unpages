using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBomb : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject explosionPrefab;
    public Transform PotBottom;
    public Transform PotTop;
    void Start()
    {
        BombTween();
      
    }

    void Explode()
    {
        if (Runner.IsSharedModeMasterClient)
        {
            GameObject Explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity); //1
            Explosion.GetComponent<BombExplosion>().Init(GameService.Instance.playerAction.bombManager.bombSize);
            GetComponent<MeshRenderer>().enabled = false; //2
            transform.Find("Collider").gameObject.SetActive(false); //3
        }
        if (Object.HasStateAuthority)
        {
            Destroy(gameObject, .3f);
            GameService.Instance.playerAction.bombManager.bombCounter++;
        }
    }
    public void BombTween()
    {
        PotBottom.DORotate(PotBottom.localRotation.eulerAngles + new Vector3(0, 0, 8),0.2f).SetLoops(12).OnComplete(() => PotTop.DOMoveY(10, 0.4f));
        PotTop.DORotate(PotTop.localRotation.eulerAngles + new Vector3(0, 0, -8),0.2f).SetLoops(12);
        Invoke("Explode", 3f);
    }
}
