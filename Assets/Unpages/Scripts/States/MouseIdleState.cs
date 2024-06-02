public class MouseIdleState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
        CharacterIdle();
        mouseStateManager.isCatch = false;
        mouseStateManager.mouseGrabbleObject = null;
        mouseStateManager.mouseAgent.speed = 10;
    }

    public void ExitState()
    {
        CharacterDontIdle();
    }

    public void UpdateState()
    {
        if (mouseStateManager.expiredFood.Count > 0 && !mouseStateManager.mouseGrabbleObject)
        {
            mouseStateManager.ChangeState(new MouseStealFoodState());
        }
    }
    public void CharacterIdle()
    {
        mouseStateManager.networkMouseAI.isIdle = true;
        mouseStateManager.networkMouseAI.isRunning = false;
        mouseStateManager.networkMouseAI.isJumping = false;
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isIdle", true);
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isRunning", false);
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isJumping", false);
    }
    public void CharacterDontIdle()
    {
        mouseStateManager.networkMouseAI.isIdle = false;
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isIdle", false);
    }
}
