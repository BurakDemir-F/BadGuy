namespace Generic.ShowSystem
{
    public class LeftToRightNavigator : Navigator
    {
        
        public override InputButton Navigate(SelectionInputType inputType)
        {
            switch (inputType)
            {
                case SelectionInputType.Forward:
                    MoveForward();
                    break;
                case SelectionInputType.Backward:
                    MoveBackward();
                    break;
                case SelectionInputType.Left:
                    MoveBackward();
                    break;
                case SelectionInputType.Right:
                    MoveForward();
                    break;
            }

            return CurrentButton;
        }
    }
}