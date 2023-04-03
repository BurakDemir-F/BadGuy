namespace InputRelated
{
    public interface IInputControlProvider
    {
        void EnableInput();
        void DisableInput();
        InputType InputType { get; }
    }

    public enum InputType
    {
        None,
        Movement,
        PlayerInteraction,
        UI,
        InGameUI,
        PoisonInput,
    }
}