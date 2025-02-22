using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    /// <summary>true の時は一時停止とする</summary>
    public static bool PauseFlg = false;

    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        PauseResume();
    }
    void Update()
    {
        // Tab キーが押されたら一時停止・再開を切り替える
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PauseResume();
        }
    }

    /// <summary>
    /// 一時停止・再開を切り替える
    /// </summary>
    void PauseResume()
    {
        PauseFlg = !PauseFlg;

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Pause");

        foreach (var o in objects)
        {
            IPause i = o.GetComponent<IPause>();

            if (PauseFlg)
            {
                i?.Pause();     
            }
            else
            {
                i?.Resume();   
            }
        }
    }
}

/// <summary>
/// ポーズ処理を管理するインターフェース
/// </summary>
public interface IPause
{
    /// <summary>一時停止のための処理を実装する</summary>
    void Pause();

    /// <summary>再開のための処理を実装する</summary>
    void Resume();
}

