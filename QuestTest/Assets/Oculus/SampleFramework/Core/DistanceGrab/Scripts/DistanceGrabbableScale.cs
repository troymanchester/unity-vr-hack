/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using System;
using UnityEngine;
using OVRTouchSample;

namespace OculusSampleFramework
{
    public class DistanceGrabbableScale : DistanceGrabbable
    {
        //public string m_materialColorField;

        GrabbableCrosshair m_crosshair;
        GrabManager m_crosshairManager;
        Renderer m_renderer;
        MaterialPropertyBlock m_mpb;

        /*public bool InRange
        {
            get { return m_inRange; }
            set
            {
                m_inRange = value;
                RefreshCrosshair();
            }
        }*/
        bool m_inRange;

        /*public bool Targeted
        {
            get { return m_targeted; }
            set
            {
                m_targeted = value;
                RefreshCrosshair();
            }
        }*/
        bool m_targeted;

        protected override void Start()
        {
            base.Start();
            m_crosshair = gameObject.GetComponentInChildren<GrabbableCrosshair>();
            m_renderer = gameObject.GetComponent<Renderer>();
            m_crosshairManager = FindObjectOfType<GrabManager>();
            m_mpb = new MaterialPropertyBlock();
            RefreshCrosshair();
            m_mpb.SetColor(m_materialColorField, Color.white);
            m_renderer.SetPropertyBlock(m_mpb);
        }

        void RefreshCrosshair()
        {
            if (m_crosshair)
            {
                if (isGrabbed) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else if (!InRange) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else m_crosshair.SetState(Targeted ? GrabbableCrosshair.CrosshairState.Targeted : GrabbableCrosshair.CrosshairState.Enabled);
            }
            if (m_materialColorField != null)
            {
                m_renderer.GetPropertyBlock(m_mpb);
                if (isGrabbed || !InRange) m_mpb.SetColor(m_materialColorField, Color.white);
                else if (Targeted) m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorHighlighted);
                else m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorInRange);
                m_renderer.SetPropertyBlock(m_mpb);
            }
        }

        /// <summary>
        /// Notifies the object that it has been grabbed.
        /// </summary>
        public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            Transform tf = gameObject.GetComponent<Transform>();
            m_grabbedBy = hand;
            m_grabbedCollider = grabPoint;
            rb.isKinematic = true;
            tf.localScale += new Vector3(0, 0, tf.localScale.z * 10);
        }

        /// <summary>
        /// Notifies the object that it has been released.
        /// </summary>
        public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            Transform tf = gameObject.GetComponent<Transform>();
            rb.isKinematic = m_grabbedKinematic;
            rb.velocity = linearVelocity;
            rb.angularVelocity = angularVelocity;
            transform.localScale -= new Vector3(0, 0, (float)(tf.localScale.z - (tf.localScale.z * 0.1F)));
            m_grabbedBy = null;
            m_grabbedCollider = null;
        }

        /*public void SetColor(Color focusColor)
        {
            m_mpb.SetColor(m_materialColorField, focusColor);
            m_renderer.SetPropertyBlock(m_mpb);
        }

        public void ClearColor()
        {
            m_mpb.SetColor(m_materialColorField, Color.white);
            m_renderer.SetPropertyBlock(m_mpb);
        }*/
    }
}
