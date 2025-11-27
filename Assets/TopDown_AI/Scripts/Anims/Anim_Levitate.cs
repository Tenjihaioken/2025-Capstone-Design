using UnityEngine;

public class Anim_Levitate : MonoBehaviour
{
    public Vector2 time;
    public Vector2 direction;
    public iTween.EaseType easetype = iTween.EaseType.easeInOutSine;
    public bool isLocal = false;

    private Vector3 startPos;

    void Start()
    {
        startPos = isLocal ? transform.localPosition : transform.position;

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", startPos.x,
            "to", startPos.x + direction.x,
            "time", time.x,
            "looptype", iTween.LoopType.pingPong,
            "easetype", easetype,
            "onupdate", "UpdateX"
        ));

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", startPos.y,
            "to", startPos.y + direction.y,
            "time", time.y,
            "looptype", iTween.LoopType.pingPong,
            "easetype", easetype,
            "onupdate", "UpdateY"
        ));
    }

    void UpdateX(float val)
    {
        Vector3 pos = isLocal ? transform.localPosition : transform.position;
        pos.x = val;
        if (isLocal)
            transform.localPosition = pos;
        else
            transform.position = pos;
    }

    void UpdateY(float val)
    {
        Vector3 pos = isLocal ? transform.localPosition : transform.position;
        pos.y = val;
        if (isLocal)
            transform.localPosition = pos;
        else
            transform.position = pos;
    }
}




