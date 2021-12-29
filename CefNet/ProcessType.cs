namespace CefNet
{
	/// <summary>
	/// The type of process.
	/// </summary>
	public enum ProcessType
	{
		/// <summary>
		/// The browser process.
		/// </summary>
		Main = 0,

		/// <summary>
		/// The render process.
		/// </summary>
		Renderer = 1,

		/// <summary>
		/// The zygote process.
		/// </summary>
		Zygote = 2,

		/// <summary>
		/// The GPU process.
		/// </summary>
		Gpu = 3,

		/// <summary>
		/// The utility process.
		/// </summary>
		Utility = 4,

		/// <summary>
		/// The PPAPI plugin process.
		/// </summary>
		PPApi = 5,

		/// <summary>
		/// The PPAPI plugin broker process.
		/// </summary>
		PPApiBroker = 6,

		/// <summary>
		/// The Native Client loader process.
		/// </summary>
		NaClLoader = 7,

		/// <summary>
		/// Any other process.
		/// </summary>
		Other = int.MaxValue
	}
}
