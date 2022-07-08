using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StackController : Singleton<StackController>
{
    [SerializeField] private Transform playerVisual, rootPos;
    [SerializeField] private BoxCollider stackTileCollider;
    private float DeltaPosY => stackTileCollider.size.y;
    [SerializeField] private List<GameObject> tiles;
    private int cnt = 0;
    private void Start()
    {
        //CanvasController.Instance.UpdateStackIndicatorText(tiles.Count);
    }

    public void CollectTile(GameObject newTile)
    {
        AddTile(newTile);
        cnt++;
        RefreshElementPos(playerVisual.transform, false);
    }

    private void AddTile(GameObject newTile)
    {
        newTile.tag = "Untagged";
        var peakTile = tiles.Last().transform;
        var newTilePos = peakTile.position;
        newTilePos.y += 0.3f;
        newTile.transform.position = newTilePos;
        newTile.transform.SetParent(rootPos);
        tiles.Add(newTile);
        CanvasController.Instance.UpdateStackIndicatorText(cnt);
    }

    public bool RemoveTile(Vector3 tileNewPos)
    {
        if (tiles.Count > 1)
        {
            // remove a tile from stack
            var tileToBeRemove = tiles.First();
            tileToBeRemove.tag = "Untagged";// to prevent re-collecting
            tileToBeRemove.transform.SetParent(null);
            tileToBeRemove.transform.position = tileNewPos;
            tileToBeRemove.isStatic = true;
            tiles.RemoveAt(0);
            // refresh player's visual and all tiles' positions that in the stack
            tiles.ForEach(x => RefreshElementPos(x.transform, true));
            RefreshElementPos(playerVisual.transform, true);
            // update UI
            //CanvasController.Instance.UpdateStackIndicatorText(tiles.Count);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void RefreshElementPos(Transform element, bool lower)
    {
        var newPos = element.position;

        if (lower)
            newPos.y -= 0.3f;
        else
            newPos.y += 0.3f;

        element.position = newPos;
    }
}

