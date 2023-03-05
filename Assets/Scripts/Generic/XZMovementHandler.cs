using System;
using System.ComponentModel;
using AYellowpaper;
using UnityEngine;

namespace Generic
{
    public class XZMovementHandler : MovementInput
    {
        [SerializeField] private InterfaceReference<ISpeedProvider> _speedProvider;

        protected float Speed;
        protected float RotateSpeed;
        protected Vector3 MoveDirectionOnXZ;
        protected bool IsMoving;
        
        public event Action MovementStarted;
        public event Action MovementEnd;

        protected override void Start()
        {
            base.Start();
            SetSpeed(_speedProvider.Value);
        }

        protected virtual void Update()
        {
            DetectTargetLocationChanges();
        }

        private void DetectTargetLocationChanges()
        {
            if(MoveVector.x == 0 & MoveVector.y == 0)
            {
                ChangeMovementStatus(false);
                return;
            }

            MoveDirectionOnXZ = new Vector3(MoveVector.x, 0f, MoveVector.y);
            ChangeMovementStatus(true);
            OnMovementDirectionChanged();
        }

        protected virtual void OnMovementDirectionChanged() {}

        private void ChangeMovementStatus(bool isMovingNow)
        {
            if (IsMoving && !isMovingNow)
            {
                IsMoving = false;
                MovementEnd?.Invoke();
            }

            if (!IsMoving && isMovingNow)
            {
                IsMoving = true;
                MovementStarted?.Invoke();
            }
        }
        
        protected void UpdateOnMoveVector()
        {
            if(MoveVector.x == 0f && MoveVector.y == 0)
                return;
            
            Move(MoveVector);
        }

        protected void UpdateOnAxis()
        {
            if(HorizontalAxis == 0 && VerticalAxis == 0)
                return;
            
            var directionVec = new Vector3(HorizontalAxis,VerticalAxis);
            Move(directionVec);
        }

        protected void Move()
        {
            Move(MoveVector);
        }
        
        protected void Move(Vector2 direction)
        {
            var changeInXY = direction * (Speed * Time.deltaTime);
            var changeInXZ = new Vector3(changeInXY.x, 0f, changeInXY.y);
            var myTransform = transform;
            myTransform.position += changeInXZ;
        }

        protected void Rotate()
        {
            var myTransform = transform;
            var currentRotation = myTransform.rotation;
            var toRotation = Quaternion.LookRotation(MoveDirectionOnXZ);
            myTransform.rotation = Quaternion.RotateTowards(currentRotation, toRotation, RotateSpeed * Time.deltaTime);
        }

        protected void SetSpeed(ISpeedProvider speedProvider)
        {
            Speed = speedProvider.MoveSpeed;
            RotateSpeed = speedProvider.RotateSpeed;
        }
    }
}