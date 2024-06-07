using UnityEngine;

public class Wardrobe : GimmickBase
{
    [SerializeField] Transform hideSpot;
    [SerializeField] Transform _releaseSpot;
    public override void PressedY()
    {
        if (HorrorPlayer.player._freeze)
        {
            HorrorPlayer.player.transform.position = _releaseSpot.position;
            HorrorPlayer.player._freeze = !HorrorPlayer.player._freeze;
        }
        else
        {
            HorrorPlayer.player.transform.position = hideSpot.position;
            HorrorPlayer.player._freeze = !HorrorPlayer.player._freeze;
        }
    }
}
