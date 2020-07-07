using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        public Vector3 m_LookAheadPos;

        private float startDistanceY;
        private float startDistanceZ;
        public  float zoomFactor = 0;
        public Vector3 zoomedDistance= new Vector3();


        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
            startDistanceY = (transform.position - target.position).y;
            startDistanceZ = target.position.z - transform.position.z;
        }


        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;
            float zMoveDelta = (target.position - m_LastTargetPosition).z;
            //Debug.Log(xMoveDelta + " :: " + Mathf.Sign(xMoveDelta));

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold || Mathf.Abs(zMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                Vector3 m_LookAheadPosX = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
                Vector3 m_LookAheadPosZ = lookAheadFactor * Vector3.back * Mathf.Sign(zMoveDelta);
                Vector3 m_lookAheadPosi = new Vector3(lookAheadFactor * Mathf.Sign(xMoveDelta), 0, Mathf.Sign( zMoveDelta));
                m_LookAheadPos = m_lookAheadPosi;
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            //Zoomfactor
            zoomFactor+= Input.GetAxis("Mouse ScrollWheel");
            zoomFactor = Mathf.Clamp(zoomFactor, -2.4f, 0.5f);
            zoomedDistance = new Vector3(0, startDistanceY, startDistanceZ) * (1- zoomFactor);


            //setting Position
            Vector3 aheadTargetPos = new Vector3(target.position.x, zoomedDistance.y, target.position.z - zoomedDistance.z) + m_LookAheadPos ;
            Vector3 aTPplusY = aheadTargetPos + new Vector3(0, target.position.y, 0);
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aTPplusY, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }
    }
}
