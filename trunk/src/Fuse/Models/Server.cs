using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	public interface IServer
	{
		object Id { get; }
		string Name { get; set; }
		string Url { get; set; }
	}

	public partial class ServerConnection : Base<ServerConnection>
	{


	}

	public partial class Server : Base<Server> 
	{

		public EntityList<ServerConnection> Connections
		{
			get;
			set;
		}

		public Server()
		{

			
		}
	}
}
