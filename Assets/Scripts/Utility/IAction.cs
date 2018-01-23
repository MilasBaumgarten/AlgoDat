using System;
using System.Collections;

namespace AnimationQueue{
	public interface IAction
	{
		IEnumerator Run();
	}
}

