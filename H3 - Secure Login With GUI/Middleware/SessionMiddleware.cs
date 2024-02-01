namespace H3___Secure_Login_With_GUI.Middleware
{
	public class SessionMiddleware
	{
		private readonly RequestDelegate _next;
		
		public SessionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		// Set a unique session ID for all visiting users
		public async Task InvokeAsync(HttpContext context)
		{
			if (string.IsNullOrEmpty(context.Session.GetString("SessionID")))
			{
				context.Session.SetString("SessionID", Guid.NewGuid().ToString());
			}

			await _next(context);
		}
	}
}
