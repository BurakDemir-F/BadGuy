using System;
using System.ComponentModel;
using AYellowpaper;
using UnityEngine;
using Utilities;

namespace Generic
{
    public class XZMovementHandler : MovementInput
    {
        [SerializeField] private InterfaceReference<ISpeedProvider> _speedProvider;

        protected float Speed;
        protected float RotateSpeed;
        protected float FallSpeed;
        protected float Gravity;
        protected Vector3 MoveDirectionOnXZ;
        protected bool IsMoving;
        
        public event Action MovementStarted;
        public event Action MovementEnd;

        protected override void Start()
        {
            base.Start();
            SetSpeed(_speedProvider.Value);
        }

        protected override void Update()
        {
            base.Update();
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if(MoveVector.x == 0 & MoveVector.y == 0)
            {
                ChangeMovementStatus(false);
                return;
            }

            MoveDirectionOnXZ = GetMovementDirection();
            ChangeMovementStatus(true);
            ApplyGravity();
            OnMovementDirectionChanged();
        }

        protected virtual Vector3 GetMovementDirection()
        {
            return new Vector3(CurrentMoveVector.x, 0f, CurrentMoveVector.y);
        }

        protected virtual void OnMovementDirectionChanged()
        {
            Move();
            Rotate();
        }

        protected virtual void ApplyGravity()
        {
            
        }
        
        protected void ChangeMovementStatus(bool isMovingNow)
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

        protected virtual void Move()
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

        protected virtual void Rotate()
        {
            var myTransform = transform;
            var currentRotation = myTransform.rotation;
            var toRotation = Quaternion.LookRotation(MoveDirectionOnXZ.SetY(0f));
            myTransform.rotation = Quaternion.RotateTowards(currentRotation, toRotation, RotateSpeed * Time.deltaTime);
        }

        protected void SetSpeed(ISpeedProvider speedProvider)
        {
            Speed = speedProvider.MoveSpeed;
            RotateSpeed = speedProvider.RotateSpeed;
            FallSpeed = speedProvider.FallSpeed;
            Gravity = speedProvider.Gravity;
        }
    }
}