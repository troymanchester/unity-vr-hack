/************************************************************************************
Copyright : Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.

Licensed under the Oculus Utilities SDK License Version 1.31 (the "License"); you may not use
the Utilities SDK except in compliance with the License, which is provided at the time of installation
or download, or which otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at
https://developer.oculus.com/licenses/utilities-1.31

Unless required by applicable law or agreed to in writing, the Utilities SDK distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
ANY KIND, either express or implied. See the License for the specific language governing
permissions and limitations under the License.
************************************************************************************/

using System;
using UnityEngine;

/// <summary>
/// An object that can be grabbed and thrown by OVRGrabber.
/// </summary>
public class OVRGrabbableGrow : OVRGrabbable
{

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

    void Awake()
    {
        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if (collider == null)
            {
				throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }
    }

    /*protected virtual void Start()
    {
        m_grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
    }*/

    void OnDestroy()
    {
        if (m_grabbedBy != null)
        {
            // Notify the hand to release destroyed grabbables
            m_grabbedBy.ForceRelease(this);
        }
    }
}
