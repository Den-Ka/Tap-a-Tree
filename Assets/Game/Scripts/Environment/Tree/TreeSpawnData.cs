using Tap_a_Tree.Player.Resources;
using UnityEngine;

namespace Tap_a_Tree
{
    public struct TreeSpawnData
    {
        public Sprite Sprite { get; set; }
        public Resource[] Drop { get; set; }
        public int Health { get; set; }
    }
}