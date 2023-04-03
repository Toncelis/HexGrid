public abstract class FlowState {
    protected GridController grid;
    protected GameFlow flow;

    protected FlowState(GameFlow flowController, GridController gridController) {
        flow = flowController;
        grid = gridController;
    }
    
    public virtual void OnTileClick(HexController hex) {
        
    }

    public virtual void OnCharClick(CharController character) {
        
    }

    public virtual void OnConfirmClick() {
        
    }

    public virtual void OnRightClick() {
        
    }

    public virtual void OnAbilityClick() {
        
    }

    public virtual void ExitState() {
        
    }

    public virtual void EnterState() {
        
    }
};