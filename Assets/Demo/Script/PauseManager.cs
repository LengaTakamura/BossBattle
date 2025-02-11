using UnityEngine;

public class PauseManager : MonoBehaviour
{
    /// <summary>true �̎��͈ꎞ��~�Ƃ���</summary>
    public static bool PauseFlg = false;

    void Update()
    {
        // ESC �L�[�������ꂽ��ꎞ��~�E�ĊJ��؂�ւ���
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PauseResume();
        }
    }

    /// <summary>
    /// �ꎞ��~�E�ĊJ��؂�ւ���
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
/// �|�[�Y�������Ǘ�����C���^�[�t�F�[�X
/// </summary>
public interface IPause
{
    /// <summary>�ꎞ��~�̂��߂̏�������������</summary>
    void Pause();

    /// <summary>�ĊJ�̂��߂̏�������������</summary>
    void Resume();
}

