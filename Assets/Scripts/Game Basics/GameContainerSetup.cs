using CryoDI;
using MiniBehaviours;

namespace Game_Basics
{
    public class GameContainerSetup : UnityStarter
    {
        protected override void SetupContainer(CryoContainer container)
        {
            base.SetupContainer(container);
            container.RegisterType<IController, TankInput, TankShooter>();
            container.RegisterType<IController, TankInput, TankMovement>();
        }
    }
}
