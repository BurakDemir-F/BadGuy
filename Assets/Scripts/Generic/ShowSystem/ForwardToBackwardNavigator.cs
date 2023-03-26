namespace Generic.ShowSystem
{
    public class ForwardToBackwardNavigator : Navigator
    {
        public override InputButton Navigate(SelectionInputType inputType)
        {
            switch (inputType)
            {
                case SelectionInputType.Forward:
                    MoveBackward();
                    break;
                case SelectionInputType.Backward:
                    MoveForward();
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