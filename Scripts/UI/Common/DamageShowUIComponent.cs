using GJC.Helper;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageShowUIComponent : UIComponent,IRelease
{
    public Unit owner;
    public float damageNum;
    private TMP_Text text;
    private Vector2 delta;
    public override void Init()
    {
        base.Init();
        if (text == null)
            text = GetComponentInChildren<TMP_Text>();
        text.text = damageNum.ToString();
        delta = Vector2.zero;
        GameObjectPool.Instance.Release(this.gameObject,3.0f);
    }

    protected override void _Update()
    {
        base._Update();
        if (owner == null) return;
        var camera = GameMainEngine.Instance.player.playerCamera;
        delta += new Vector2(Time.deltaTime*5.0f, Time.deltaTime * 50.0f);
        transform.position = camera.WorldToScreenPoint(owner.damageUIShowPos.position) +
                             new Vector3(delta.x, delta.y+UIInfos.PlayerUIHeigh , 0);
    }

    public void OnRelease()
    {
        owner = null;
    }
}