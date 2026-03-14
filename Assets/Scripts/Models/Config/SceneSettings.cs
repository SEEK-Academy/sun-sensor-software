namespace Assets.Scripts.Models.Config
{
    public record SceneSettings
    {
        public int CameraStartOrientation;
        public float CameraStartDistance;
        public EulerAngles CubesatStartRotation;

        internal static SceneSettings GetDefault()
        {
            return new SceneSettings
            {
                CameraStartOrientation = 1,
                CameraStartDistance = 70f,
                CubesatStartRotation = new EulerAngles()
            };
        }
    }
}
