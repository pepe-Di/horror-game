using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable/EnemyStateController", fileName = "New EnemyStateController")]
public class EnemyState : ScriptableObject
{
    // Start is called before the first frame update
    public enemyState state;
    public void ChangeState(enemyState state)
    {
        if (this.state != state)
        {
            this.state = state;
        }
    }
    public enum enemyState
    {
        Idle, //рандомно перемещается
        Search, //услышал звук, идет по направлению точки где был последний звук
        Triggered, //игрок в поле зрения, преследует
        Wait //игрок вышел из поля зрения, переход в идл
    }
}
