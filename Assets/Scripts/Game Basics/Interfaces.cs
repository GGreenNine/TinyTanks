namespace Game_Basics
{
    public interface IController
    {
        bool IsShooting { get; }
        bool ChangeWeapon { get; }
        float GetTurnAxis { get; }
        float GetForwardAxis { get; }
    }
}