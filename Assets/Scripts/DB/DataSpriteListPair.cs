using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSpriteListPair
{
    private PartData _part;
    private List<Sprite> _spriteVariants;

    public DataSpriteListPair(PartData part, List<Sprite> spriteVariants)
    {
        _part = part;
        _spriteVariants = spriteVariants;
    }
}
