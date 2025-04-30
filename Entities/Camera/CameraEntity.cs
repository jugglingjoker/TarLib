using Microsoft.Xna.Framework;
using System;
using TarLib.Extensions;

namespace TarLib.Entities {

    public class CameraEntity<TEntityWithCamera> : IGameEntity<CameraEntity<TEntityWithCamera>.States>
        where TEntityWithCamera : IEntityWithCamera {

        public enum States {
            Idle,
            Moving
        }

        private TEntityWithCamera Entity { get; }

        private ObservableVariable<float> x = new ObservableVariable<float>();
        public float X {
            get => x.Value;
            set {
                x.Value = Entity switch {
                    IEntityWithCameraXClamp xClampEntity => MathHelper.Clamp(value, Math.Min(xClampEntity.CameraXMin, xClampEntity.CameraXMax), Math.Max(xClampEntity.CameraXMin, xClampEntity.CameraXMax)),
                    IEntityWithCameraXWrap xWrapEntity => (value + (xWrapEntity.CameraXMax - xWrapEntity.CameraXMin) - xWrapEntity.CameraXMin) % (xWrapEntity.CameraXMax - xWrapEntity.CameraXMin) + xWrapEntity.CameraXMin,
                    _ => value,
                };
            }
        }

        private ObservableVariable<float> y = new ObservableVariable<float>();
        public float Y {
            get => y.Value;
            set {
                y.Value = Entity switch {
                    IEntityWithCameraYClamp yClampEntity => MathHelper.Clamp(value, Math.Min(yClampEntity.CameraYMin, yClampEntity.CameraYMax), Math.Max(yClampEntity.CameraYMin, yClampEntity.CameraYMax)),
                    IEntityWithCameraYWrap yWrapEntity => (value + (yWrapEntity.CameraYMax - yWrapEntity.CameraYMin) - yWrapEntity.CameraYMin) % (yWrapEntity.CameraYMax - yWrapEntity.CameraYMin) + yWrapEntity.CameraYMin,
                    _ => value,
                };
            }
        }

        private ObservableVariable<float> rotation = new ObservableVariable<float>();
        public float Rotation {
            get => rotation.Value;
            set {
                rotation.Value = Entity switch {
                    IEntityWithCameraRotationClamp rotationClampEntity => MathHelper.Clamp(value, rotationClampEntity.CameraRotationMin, rotationClampEntity.CameraRotationMax),
                    IEntityWithCameraRotationWrap rotationWrapEntity => (value + (rotationWrapEntity.CameraRotationMax - rotationWrapEntity.CameraRotationMin) - rotationWrapEntity.CameraRotationMin) % (rotationWrapEntity.CameraRotationMax - rotationWrapEntity.CameraRotationMin) + rotationWrapEntity.CameraRotationMin,
                    _ => MathHelper.WrapAngle(value),
                };
            }
        }

        private ObservableVariable<float> zoom = new ObservableVariable<float>(1);
        public float Zoom {
            get => zoom.Value;
            set {
                switch (Entity) {
                    case IEntityWithCameraZoomBreakpoints zoomEntityBreakpoints:
                        // TODO: add breakpoint logic
                        zoom.Value = value;
                        break;
                    default:
                        zoom.Value = MathHelper.Clamp(value, Entity.CameraMinimumZoom, Entity.CameraMaximumZoom);
                        break;
                }
                // Force recalculation of X and Y
                X = x.Value;
                Y = y.Value;
            }
        }

        private ObservableVariable<IEntityState<States>> state = new ObservableVariable<IEntityState<States>>();
        public IEntityState<States> State {
            get => state.Value;
            set => state.Value = value;
        }

        public Vector2 TiltVector => new Vector2(1, Entity.CameraTilt);
        public Vector2 Position {
            get => new Vector2(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        private float currentSpeed;
        private float currentAngle;

        public event EventHandler<(float oldValue, float newValue)> OnXChange;
        public event EventHandler<(float oldValue, float newValue)> OnYChange;
        public event EventHandler<(float oldValue, float newValue)> OnRotationChange;
        public event EventHandler<(float oldValue, float newValue)> OnZoomChange;
        public event EventHandler<(IEntityState<States> oldState, IEntityState<States> newState)> OnStateChange;

        public CameraEntity(TEntityWithCamera entity) {
            Entity = entity;

            x.OnChange += X_OnChange;
            y.OnChange += Y_OnChange;
            rotation.OnChange += Rotation_OnChange;
            zoom.OnChange += Zoom_OnChange;
            state.OnChange += State_OnChange;
        }

        private void X_OnChange(object sender, (float oldValue, float newValue) e) {
            OnXChange?.Invoke(this, e);
        }

        private void Y_OnChange(object sender, (float oldValue, float newValue) e) {
            OnYChange?.Invoke(this, e);
        }

        private void Rotation_OnChange(object sender, (float oldValue, float newValue) e) {
            OnRotationChange?.Invoke(this, e);
        }

        private void Zoom_OnChange(object sender, (float oldValue, float newValue) e) {
            OnZoomChange?.Invoke(this, e);
        }

        private void State_OnChange(object sender, (IEntityState<States> oldValue, IEntityState<States> newValue) e) {
            OnStateChange?.Invoke(this, e);
        }

        public void Update(float elapsedTime) {
            currentSpeed = MathHelper.Clamp(currentSpeed + Entity.CameraAcceleration, 0, Math.Min(Position.DistanceTo(Entity.CameraTarget), Entity.CameraMaximumSpeed));
            currentAngle = Position.AngleTo(Entity.CameraTarget);
            Position += Vector2Extensions.AngleToVector2(currentAngle) * currentSpeed;
        }

        public Vector2 DrawPositionToRealPosition(Vector2 drawPosition) {
            return Vector2.Transform((drawPosition - Entity.CameraCenter) / (Zoom * TiltVector * Entity.CameraPixelsPerUnit), Matrix.CreateRotationZ(-1 * Rotation)) + Position;
        }

        public Vector2 RealPositionToDrawPosition(Vector2 realPosition, bool ignoreTilt = false) {
            return Vector2.Transform(realPosition - Position, Matrix.CreateRotationZ(Rotation)) * Zoom * (ignoreTilt ? Vector2.One : TiltVector) * Entity.CameraPixelsPerUnit + Entity.CameraCenter;
        }

        public Vector2 RealPositionToDrawPosition(Point realPosition, bool ignoreTilt = false) {
            return RealPositionToDrawPosition(realPosition.ToVector2(), ignoreTilt);
        }
    }
}