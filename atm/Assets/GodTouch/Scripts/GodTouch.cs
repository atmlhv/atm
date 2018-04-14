using UnityEngine;

namespace GodTouches
{
    /// <summary>
    /// GodTouch
    /// </summary>
    public static class GodTouch
    {
        static readonly int[] LayerOrder = new int[] {2048, 1024, 512, 256, 32 };

        /// <summary>
        /// Androidフラグ
        /// </summary>
        static readonly bool IsAndroid = Application.platform == RuntimePlatform.Android;
        /// <summary>
        /// iOSフラグ
        /// </summary>
        static readonly bool IsIOS = Application.platform == RuntimePlatform.IPhonePlayer;
        /// <summary>
        /// エディタフラグ
        /// </summary> 
        static readonly bool IsEditor = !IsAndroid && !IsIOS;

        /// <summary>
        /// デルタポジション判定用・前回のポジション
        /// </summary>
        static Vector3 prebPosition;

        /// <summary>
        /// タッチ情報を取得(エディタとスマホを考慮)
        /// </summary>
        /// <returns>タッチ情報</returns>
        public static GodPhase GetPhase()
        { 
            if (IsEditor)
            {
                if (Input.GetMouseButtonDown(0)) 
                {
                    prebPosition = Input.mousePosition;
                    return GodPhase.Began;
                }
                else if (Input.GetMouseButton(0))     
                {
                    return GodPhase.Moved;
                }
                else if (Input.GetMouseButtonUp(0)) 
                {
                    return GodPhase.Ended;
                }
            }
            else 
            {
                if (Input.touchCount > 0) return (GodPhase)((int)Input.GetTouch(0).phase);
            } 
            return GodPhase.None;
        }

        /// <summary>
        /// タッチポジションを取得(エディタとスマホを考慮)
        /// </summary>
        /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
        public static Vector3 GetPosition()
        { 
            if (IsEditor)
            {
                if (GetPhase() != GodPhase.None) return Input.mousePosition;
            }
            else
            {
                if (Input.touchCount > 0) return Input.GetTouch(0).position;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// タッチデルタポジションを取得(エディタとスマホを考慮)
        /// </summary>
        /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
        public static Vector3 GetDeltaPosition()
        { 
            if (IsEditor)
            {
                var phase = GetPhase();
                if(phase != GodPhase.None)
                {
                    var now = Input.mousePosition;
                    var delta = now - prebPosition;
                    prebPosition = now; 
                    return delta;
                }
            } 
            else
            {
                if (Input.touchCount > 0) return Input.GetTouch(0).deltaPosition;
            }
            return Vector3.zero;
        }

		public static GameObject TappedObject(){
			Ray ray = Camera.main.ScreenPointToRay(GodTouch.GetPosition());
            RaycastHit2D hit;


            for ( int i = 0; i < LayerOrder.Length; i++)
            {
                hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity, LayerOrder[i]);
                if (hit.collider)
                {
                    return hit.collider.gameObject;
                }
            }
            /*
			RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f,  Mathf.Infinity, 32);

			if (hit.collider) {
                Debug.Log("Canvas");
                Debug.Log(hit.collider.gameObject);
				return hit.collider.gameObject;
			} else {
            */
                hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity);
            if (hit.collider)
            {
                Debug.Log(hit.collider.gameObject);
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
		}

        public static bool ObjectAny(GameObject obj)
        {
            Ray ray = Camera.main.ScreenPointToRay(GodTouch.GetPosition());
            RaycastHit2D hit;


            for (int i = 0; i < LayerOrder.Length; i++)
            {
                hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity, LayerOrder[i]);
                if (hit.collider)
                {
                    if (hit.collider.gameObject == obj) return true;
                }
            }
         /*
            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity);
            if (hit.collider)
            {
                if (hit.collider.gameObject == obj) return true;
            }
            */
            return false;
        }

        public static bool ObjectAnyWF(GameObject obj,int ii)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(ii).position);
            RaycastHit2D hit;


            for (int i = 0; i < LayerOrder.Length; i++)
            {
                hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity, LayerOrder[i]);
                if (hit.collider)
                {
                    if (hit.collider.gameObject == obj) return true;
                }
            }
            /*
               hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity);
               if (hit.collider)
               {
                   if (hit.collider.gameObject == obj) return true;
               }
               */
            return false;
        }

        public static GameObject TappedObjectWF(int ii)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(ii).position);
            RaycastHit2D hit ;


            for (int i = 0; i < LayerOrder.Length; i++)
            {
                hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity, LayerOrder[i]);
                if (hit.collider)
                {
                    return hit.collider.gameObject;
                }
            }
            /*
			RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f,  Mathf.Infinity, 32);

			if (hit.collider) {
                Debug.Log("Canvas");
                Debug.Log(hit.collider.gameObject);
				return hit.collider.gameObject;
			} else {
            */
            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction * -1f, Mathf.Infinity);
            if (hit.collider)
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }

        public static int FingerIndex(int fi)
        {
            if (IsEditor)
            {
                return 0;
            }
            else
            {
                for (int ii = 0; ii < Input.touchCount; ii++)
                {
                    if(Input.GetTouch(ii).fingerId == fi)
                    {
                        return ii;
                    }
                }
                return 0;
            }
        }
    }

    /// <summary>
    /// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
    /// </summary>
    public enum GodPhase 
    {
        None       = -1,
        Began      = 0, 
        Moved      = 1, 
        Stationary = 2, 
        Ended      = 3, 
        Canceled   = 4 
    }
}