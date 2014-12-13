using System;
using System.Collections.Generic;

public enum BehaviorState {
	SUCCESS,
	FAILURE,
	RUNNING
}

public class BehaviorTree {
	BehaviorComponent root;
	BehaviorState state;
	
	public BehaviorTree(BehaviorComponent r) {
		root = r;
	}
	
	public virtual BehaviorState Behave() {
		return root.Behave();
	}
}

public class BehaviorComponent {
	public List<BehaviorComponent> children;
	
	public BehaviorComponent(List<BehaviorComponent> c) {
		children = c;
	}
	
	public virtual BehaviorState Behave() {
		return BehaviorState.FAILURE;
	}
}

/*
 * A selector runs each task in order until one succeeds,
 * at which point it returns SUCCESS. If all tasks fail, a FAILURE
 * status is returned.  If a subtask is still RUNNING, then a RUNNING
 * status is returned and processing continues until either SUCCESS
 * or FAILURE is returned from the subtask.
 */
public class Selector : BehaviorComponent {
	public Selector(List<BehaviorComponent> c) : base(c) {
		children = c;
	}
	
	public override BehaviorState Behave() {
	    // loop through children
	    foreach (BehaviorComponent c in children) {
	    	BehaviorState status = c.Behave();
	    	
	    	if (status != BehaviorState.FAILURE) {
	    		return status;
	    	}
	    }
	    return BehaviorState.FAILURE;
	}
}

/*
 * A sequence runs each task in order until one fails,
 * at which point it returns FAILURE. If all tasks succeed, a SUCCESS
 * status is returned.  If a subtask is still RUNNING, then a RUNNING
 * status is returned and processing continues until either SUCCESS
 * or FAILURE is returned from the subtask.
 */
public class Sequence : BehaviorComponent {
	public Sequence(List<BehaviorComponent> c) : base(c) {
		children = c;
	}
	
	public override BehaviorState Behave() {
	    // loop through children
	    foreach (BehaviorComponent c in children) {
	    	BehaviorState status = c.Behave();
	    	
	    	if (status != BehaviorState.SUCCESS) {
	    		return status;
	    	}
	    }
	    return BehaviorState.SUCCESS;
	}
}

public class BehaviorAction : BehaviorComponent {
    private Func<BehaviorState> _Action;

    public BehaviorAction(Func<BehaviorState> action) : base(new List<BehaviorComponent>()) {
        _Action = action;
    }

    public override BehaviorState Behave() {
    	return _Action.Invoke();
    }
}
