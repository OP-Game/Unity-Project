using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Exploder.Demo
{
    public class DemoClickPartialExplode : MonoBehaviour
    {
        private ExploderObject Exploder;

        private void Start()
        {
            Application.targetFrameRate = 60;

            //
            // access exploder from singleton
            //
            Exploder = Utils.ExploderSingleton.Instance;
        }

        private void Update()
        {
            // we hit the mouse button
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Ray mouseRay;
                mouseRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                // we hit the object
                if (Physics.Raycast(mouseRay, out hitInfo))
                {
                    var obj = hitInfo.collider.gameObject;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Exploder.ExplodePartial(obj, mouseRay.direction, hitInfo.point, 1.0f, (ms, state) =>
                        {
                            if (state == ExploderObject.ExplosionState.ExplosionFinished)
                            {
                                Debug.Log("Explosion finished");
                            }
                        });
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Reset"))
            {
            }

#if TEST_SCENE_LOAD
        if (GUI.Button(new Rect(10, 50, 100, 30), "NextScene"))
        {
            UnityEngine.Application.LoadLevel(1);
        }
#endif
        }
    }
}
