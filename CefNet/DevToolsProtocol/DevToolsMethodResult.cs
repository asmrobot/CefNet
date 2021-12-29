namespace CefNet
{
	internal struct DevToolsMethodResult
	{
		internal DevToolsMethodResult(int messageId, object result, bool success)
		{
			this.MessageID = messageId;
			this.Result = result;
			this.Success = success;
		}

		public int MessageID { get; }

		public bool Success { get; }

		public object Result { get; }
	}
}
