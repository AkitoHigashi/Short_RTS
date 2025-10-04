using UnityEngine;

public class Worker : MinionBase
{
    public enum WorkerState { Collecting }
    protected override void OnArrivedTarget()
    {
        //資源エリアの中にいるのかを判定してアニメーションを再生
        //いないときはidelモードに移行
    }
}
