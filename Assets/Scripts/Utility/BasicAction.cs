using System;
using Model;
using System.Collections;

namespace AnimationQueue{
	public abstract class BasicAction : IAction
	{
		protected ObjectMapper mapper = ObjectMapper.Instance;

		public abstract IEnumerator Run();
	}
}

