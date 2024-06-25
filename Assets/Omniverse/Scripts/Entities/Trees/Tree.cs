using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.Entities.Trees
{
    public class Tree : Entity<TreeDesc>
    {
        public Tree(TreeDesc desc, int factionID): base(desc, factionID)
        {
        }
    }
}
