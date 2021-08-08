using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public List<Sprite> sprites;

    [ContextMenu("Change Sprite")]
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites.OrderBy( x => Random.Range(0, 1f)).First();
    }
}
