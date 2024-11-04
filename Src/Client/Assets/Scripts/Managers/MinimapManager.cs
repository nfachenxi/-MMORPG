using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public UIMinimap minimap;
        public Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get { return this.minimapBoundingBox; }
        }
        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                    return null;
                return User.Instance.CurrentCharacterObject.transform;
            }
        }

        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMiniMap(Collider minimapBoundingBox)
        {
            this.minimapBoundingBox = minimapBoundingBox;
            if (this.minimap != null)
                minimap.UpdateMap();
        }


    }
}
