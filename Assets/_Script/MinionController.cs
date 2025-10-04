using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class MinionController : MonoBehaviour
{
    [SerializeField] private float _controllRudius = 5f;
    [SerializeField] private LayerMask _minionMask;
    [SerializeField] private GameObject _circlePrefab;//円のエフェクト
    [SerializeField] private float _circleDuration = 0.5f;//表示時間
    private Vector3 _lastWorldPos;//Ruturn時のマウスのポジションを記憶（右クリック）
    [SerializeField] private List<MinionBase> _followingMinions = new List<MinionBase>();//フォロー中のミニオンリスト
    /// <summary>
    /// 指定したポジションに円を出し周囲のミニオンをフォローモードに変えるメソッド
    /// </summary>
    /// <param name="worldPos"></param>
    public void Return(Vector3 worldPos)
    {
        ShowCircle(worldPos);
        _lastWorldPos = worldPos;
        Collider[] hits = Physics.OverlapSphere(worldPos, _controllRudius, _minionMask);
        foreach (Collider hit in hits)
        {
            Debug.Log(hit.name);
            //ヒットしたオブジェトのミニオンベースを取得
            MinionBase minion = hit.GetComponent<MinionBase>();
            //ミニオンベースを持ってるかつステートがフォロー中以外の場合実行
            if (minion != null && minion.baseState != MinionBase.BaseState.FollowMode)
            {
                Debug.Log("フォロー開始");
                minion.SetFollow();//フォローさせる

                if (!_followingMinions.Contains(minion))//フォローリストのなかにこのオブジェクトが保存されているか
                {
                    _followingMinions.Add(minion);//フォロー中リストに追加
                }
            }
        }
    }
    /// <summary>
    /// フォロー中のミニオンを一体だけ指定ポジションまで移動させるメソッド（左クリック）
    /// </summary>
    /// <param name="worldPos"></param>
    public void GoTo(Vector3 worldPos)
    {
        _followingMinions.RemoveAll(NullCheck);//リスト内にヌルがあるとリストを詰める。
        foreach (var minion in _followingMinions)
        {
            if(minion != null && minion.baseState == MinionBase.BaseState.FollowMode)
            {
                //移動のメソッド
                minion.CommandMove(worldPos);
                _followingMinions.Remove(minion);//そのミニオンをリストから削除
                break; // 一体だけ移動させるため一回で終わり
            }
        }

    }
    /// <summary>
    ///　ヌルなのかをcheckする
    /// </summary>
    /// <param name="minion">フォロー中のコライダー</param>
    /// <returns>真偽チェック</returns>
    private bool NullCheck(MinionBase minion)
    {
        return minion == null;
    }
    /// <summary>
    /// マウスのクリックした箇所に円を表示する
    /// </summary>
    /// <param name="pos">右クリックしたときのマウスのポジション</param>
    private void ShowCircle(Vector3 pos)
    {
        if (_circlePrefab == null) return;
        //ポインタの位置に円を生成
        GameObject circle = Instantiate(_circlePrefab, pos + Vector3.up * 0.05f, Quaternion.identity);
        circle.transform.localScale = Vector3.zero;//最初は見えない
        Vector3 targetScale = new Vector3(_controllRudius * 2f, 0.001f, _controllRudius * 2f);
        //大きくする
        circle.transform.DOScale(targetScale, _circleDuration).SetEase(Ease.OutQuart).OnComplete(() => Destroy(circle));
    }
    // デバッグ用に円を表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_lastWorldPos, _controllRudius);
    }
}