
namespace Surge
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;

	using Amplify;
	using Amplify.ActiveRecord;
	using Amplify.ActiveRecord.Data;


	public partial class CodeAccessoryDataContext : DataContext
	{


		private static MappingSource mappingSource = new AttributeMappingSource();

		partial void OnCreate();

		public CodeAccessoryDataContext() :
			base(global::Amplify.ActiveRecord.Properties.Settings.Default.CodeAccessoryConnectionString, mappingSource)
		{
			this.OnCreate();
		}

		public CodeAccessoryDataContext(string connection)
			: base(connection, mappingSource)
		{
			this.OnCreate();
		}

		public CodeAccessoryDataContext(System.Data.IDbConnection connection)
			: base(connection, mappingSource)
		{
			this.OnCreate();
		}

		public CodeAccessoryDataContext(string connection, MappingSource mappingSource)
			: base(connection, mappingSource)
		{
			this.OnCreate();
		}

		public CodeAccessoryDataContext(System.Data.IDbConnection connection, MappingSource mappingSource)
			: base(connection, mappingSource)
		{
			this.OnCreate();
		}

		public Table<Client> Clients
		{
			get
			{
				return this.GetTable<Client>();
			}
		}

		public Table<MileStone> MileStones
		{
			get
			{
				return this.GetTable<MileStone>();
			}
		}

		public Table<Module> Modules
		{
			get
			{
				return this.GetTable<Module>();
			}
		}

		public Table<ProjectHour> ProjectHours
		{
			get
			{
				return this.GetTable<ProjectHour>();
			}
		}

		public Table<ProjectLinkedId> ProjectLinkedIds
		{
			get
			{
				return this.GetTable<ProjectLinkedId>();
			}
		}

		public Table<Project> Projects
		{
			get
			{
				return this.GetTable<Project>();
			}
		}

		public Table<ProjectTask> ProjectTasks
		{
			get
			{
				return this.GetTable<ProjectTask>();
			}
		}

		public Table<TaskLinkedId> TaskLinkedIds
		{
			get
			{
				return this.GetTable<TaskLinkedId>();
			}
		}

		public Table<Task> Tasks
		{
			get
			{
				return this.GetTable<Task>();
			}
		}

		public Table<TimeLineEvent> TimeLineEvents
		{
			get
			{
				return this.GetTable<TimeLineEvent>();
			}
		}



	}


	[Table(Name = "amplify_Clients"), Serializable()]
	public partial class Client : Base<Client>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "Name" };
		private EntitySet<Project> projects;

		public Client()
			: base()
		{

			this.projects = new EntitySet<Project>(new Action<Project>(this.attachProjects), new Action<Project>(this.detachProjects));
		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column()]
		public System.String Name
		{
			get { return (System.String)this.GetProperty("Name"); }
			set { this.SetProperty("Name", value); }
		}


		[Association(Name = "Client_Projects", Storage = "projects", OtherKey = "ClientId")]
		public EntitySet<Project> Projects
		{
			get { return this.projects; }
			set { this.projects.Assign(value); }
		}

		private void attachProjects(Project entity)
		{
			this.NotifyPropertyChanging("Projects");
			entity.Client = this;
		}

		private void detachProjects(Project entity)
		{
			this.NotifyPropertyChanging("Projects");
			entity.Client = null;
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class Clients : ActsAsList<Client, Clients>
	{


	}

	[Table(Name = "amplify_MileStones"), Serializable()]
	public partial class MileStone : Base<MileStone>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "Name", "Description", "EndsOn", "StartsOn", "ParentId" };

		public MileStone()
			: base()
		{

		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column(CanBeNull = false)]
		public System.String Name
		{
			get { return (System.String)this.GetProperty("Name"); }
			set { this.SetProperty("Name", value); }
		}
		[Column()]
		public System.String Description
		{
			get { return (System.String)this.GetProperty("Description"); }
			set { this.SetProperty("Description", value); }
		}
		[Column()]
		public System.DateTime EndsOn
		{
			get { return (System.DateTime)this.GetProperty("EndsOn"); }
			set { this.SetProperty("EndsOn", value); }
		}
		[Column()]
		public System.DateTime StartsOn
		{
			get { return (System.DateTime)this.GetProperty("StartsOn"); }
			set { this.SetProperty("StartsOn", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? ParentId
		{
			get { return (System.Guid?)this.GetProperty("ParentId"); }
			set { this.SetProperty("ParentId", value); }
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class MileStones : ActsAsList<MileStone, MileStones>
	{


	}

	[Table(Name = "amplify_Modules"), Serializable()]
	public partial class Module : Base<Module>
	{

		private static readonly string[] primaryKeys = new string[] { };
		private static readonly string[] properties = new string[] { "Id", "Name" };

		public Module()
			: base()
		{

		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column()]
		public System.String Name
		{
			get { return (System.String)this.GetProperty("Name"); }
			set { this.SetProperty("Name", value); }
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class Modules : ActsAsList<Module, Modules>
	{


	}

	[Table(Name = "amplify_ProjectHours"), Serializable()]
	public partial class ProjectHour : Base<ProjectHour>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "UserId", "Hours", "StartedAt", "EndedAt", "IsBillable", "ProjectId", "ProjectTaskId", "Notes", "IsLocked" };

		public ProjectHour()
			: base()
		{

		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? UserId
		{
			get { return (System.Guid?)this.GetProperty("UserId"); }
			set { this.SetProperty("UserId", value); }
		}
		[Column(CanBeNull = false)]
		public System.Decimal Hours
		{
			get { return (System.Decimal)this.GetProperty("Hours"); }
			set { this.SetProperty("Hours", value); }
		}
		[Column()]
		public System.DateTime StartedAt
		{
			get { return (System.DateTime)this.GetProperty("StartedAt"); }
			set { this.SetProperty("StartedAt", value); }
		}
		[Column()]
		public System.DateTime EndedAt
		{
			get { return (System.DateTime)this.GetProperty("EndedAt"); }
			set { this.SetProperty("EndedAt", value); }
		}
		[Column(CanBeNull = false)]
		public System.Boolean IsBillable
		{
			get { return (System.Boolean)this.GetProperty("IsBillable"); }
			set { this.SetProperty("IsBillable", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? ProjectId
		{
			get { return (System.Guid?)this.GetProperty("ProjectId"); }
			set { this.SetProperty("ProjectId", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? ProjectTaskId
		{
			get { return (System.Guid?)this.GetProperty("ProjectTaskId"); }
			set { this.SetProperty("ProjectTaskId", value); }
		}
		[Column()]
		public System.String Notes
		{
			get { return (System.String)this.GetProperty("Notes"); }
			set { this.SetProperty("Notes", value); }
		}
		[Column(CanBeNull = false)]
		public System.Boolean IsLocked
		{
			get { return (System.Boolean)this.GetProperty("IsLocked"); }
			set { this.SetProperty("IsLocked", value); }
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class ProjectHours : ActsAsList<ProjectHour, ProjectHours>
	{


	}

	[Table(Name = "amplify_ProjectLinkedIds"), Serializable()]
	public partial class ProjectLinkedId : Base<ProjectLinkedId>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "ProjectId", "LinkedId", "LinkedTypeId" };
		private EntityRef<Project> project;

		public ProjectLinkedId()
			: base()
		{

			this.project = default(EntityRef<Project>);
		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? ProjectId
		{
			get { return (System.Guid?)this.GetProperty("ProjectId"); }
			set
			{
				this.SetProperty("ProjectId", value,
				  delegate(Object objValue)
				  {
					  if (this.project.HasLoadedOrAssignedValue)
						  throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				  });
			}

		}
		[Column()]
		public System.String LinkedId
		{
			get { return (System.String)this.GetProperty("LinkedId"); }
			set { this.SetProperty("LinkedId", value); }
		}
		[Column()]
		public System.Guid? LinkedTypeId
		{
			get { return (System.Guid?)this.GetProperty("LinkedTypeId"); }
			set { this.SetProperty("LinkedTypeId", value); }
		}


		[Association(Name = "Project_Project", Storage = "project", ThisKey = "ProjectLinkedIdId")]
		public Project Project
		{
			get { return this.project.Entity; }
			set
			{
				Project previousValue = this.project.Entity;
				if (((previousValue != value)
					|| (this.project.HasLoadedOrAssignedValue == false)))
				{
					this.NotifyPropertyChanging("Project");
					if ((previousValue != null))
					{
						this.project.Entity = null;
						previousValue.ProjectLinkedIds.Remove(this);
					}
					this.project.Entity = null;
					if (value != null)
					{
						value.ProjectLinkedIds.Add(this);
						this.ProjectId = value.Id;
					}
					else
					{
						this.ProjectId = default(Nullable<Guid>);
					}
					this.NotifyPropertyChanged("Project");
				}
			}
		}




		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class ProjectLinkedIds : ActsAsList<ProjectLinkedId, ProjectLinkedIds>
	{


	}

	[Table(Name = "amplify_Projects"), Serializable()]
	public partial class Project : Base<Project>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "ClientId", "Name", "Description", "IsParent", "ParentId" };
		private EntityRef<Client> client;
		private EntitySet<ProjectTask> projectTasks;
		private EntitySet<ProjectLinkedId> projectLinkedIds;

		public Project()
			: base()
		{

			this.client = default(EntityRef<Client>);
			this.projectTasks = new EntitySet<ProjectTask>(new Action<ProjectTask>(this.attachProjectTasks), new Action<ProjectTask>(this.detachProjectTasks));
			this.projectLinkedIds = new EntitySet<ProjectLinkedId>(new Action<ProjectLinkedId>(this.attachProjectLinkedIds), new Action<ProjectLinkedId>(this.detachProjectLinkedIds));
		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? ClientId
		{
			get { return (System.Guid?)this.GetProperty("ClientId"); }
			set
			{
				this.SetProperty("ClientId", value,
				  delegate(Object objValue)
				  {
					  if (this.client.HasLoadedOrAssignedValue)
						  throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				  });
			}

		}
		[Column(CanBeNull = false)]
		public System.String Name
		{
			get { return (System.String)this.GetProperty("Name"); }
			set { this.SetProperty("Name", value); }
		}
		[Column()]
		public System.String Description
		{
			get { return (System.String)this.GetProperty("Description"); }
			set { this.SetProperty("Description", value); }
		}
		[Column(CanBeNull = false)]
		public System.Boolean IsParent
		{
			get { return (System.Boolean)this.GetProperty("IsParent"); }
			set { this.SetProperty("IsParent", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? ParentId
		{
			get { return (System.Guid?)this.GetProperty("ParentId"); }
			set { this.SetProperty("ParentId", value); }
		}


		[Association(Name = "Client_Client", Storage = "client", ThisKey = "Id")]
		public Client Client
		{
			get { return this.client.Entity; }
			set
			{
				Client previousValue = this.client.Entity;
				if (((previousValue != value)
					|| (this.client.HasLoadedOrAssignedValue == false)))
				{
					this.NotifyPropertyChanging("Client");
					if ((previousValue != null))
					{
						this.client.Entity = null;
						previousValue.Projects.Remove(this);
					}
					this.client.Entity = null;
					if (value != null)
					{
						value.Projects.Add(this);
						this.ClientId = value.Id;
					}
					else
					{
						this.ClientId = default(Nullable<Guid>);
					}
					this.NotifyPropertyChanged("Client");
				}
			}
		}





		[Association(Name = "Project_ProjectTasks", Storage = "projectTasks", OtherKey = "ProjectId")]
		public EntitySet<ProjectTask> ProjectTasks
		{
			get { return this.projectTasks; }
			set { this.projectTasks.Assign(value); }
		}

		private void attachProjectTasks(ProjectTask entity)
		{
			this.NotifyPropertyChanging("ProjectTasks");
			entity.Project = this;
		}

		private void detachProjectTasks(ProjectTask entity)
		{
			this.NotifyPropertyChanging("ProjectTasks");
			entity.Project = null;
		}


		[Association(Name = "Project_ProjectLinkedIds", Storage = "projectLinkedIds", OtherKey = "ProjectId")]
		public EntitySet<ProjectLinkedId> ProjectLinkedIds
		{
			get { return this.projectLinkedIds; }
			set { this.projectLinkedIds.Assign(value); }
		}

		private void attachProjectLinkedIds(ProjectLinkedId entity)
		{
			this.NotifyPropertyChanging("ProjectLinkedIds");
			entity.Project = this;
		}

		private void detachProjectLinkedIds(ProjectLinkedId entity)
		{
			this.NotifyPropertyChanging("ProjectLinkedIds");
			entity.Project = null;
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class Projects : ActsAsList<Project, Projects>
	{


	}

	[Table(Name = "amplify_ProjectTasks"), Serializable()]
	public partial class ProjectTask : Base<ProjectTask>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "BugId", "Summary", "Description", "ProjectId", "TaskId", "AssignedTo", "UserId" };
		private EntityRef<Project> project;

		public ProjectTask()
			: base()
		{

			this.project = default(EntityRef<Project>);
		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column()]
		public System.String BugId
		{
			get { return (System.String)this.GetProperty("BugId"); }
			set { this.SetProperty("BugId", value); }
		}
		[Column(CanBeNull = false)]
		public System.String Summary
		{
			get { return (System.String)this.GetProperty("Summary"); }
			set { this.SetProperty("Summary", value); }
		}
		[Column()]
		public System.String Description
		{
			get { return (System.String)this.GetProperty("Description"); }
			set { this.SetProperty("Description", value); }
		}
		[Column()]
		public System.Guid? ProjectId
		{
			get { return (System.Guid?)this.GetProperty("ProjectId"); }
			set
			{
				this.SetProperty("ProjectId", value,
				  delegate(Object objValue)
				  {
					  if (this.project.HasLoadedOrAssignedValue)
						  throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				  });
			}

		}
		[Column(CanBeNull = false)]
		public System.Guid? TaskId
		{
			get { return (System.Guid?)this.GetProperty("TaskId"); }
			set { this.SetProperty("TaskId", value); }
		}
		[Column()]
		public System.String AssignedTo
		{
			get { return (System.String)this.GetProperty("AssignedTo"); }
			set { this.SetProperty("AssignedTo", value); }
		}
		[Column()]
		public System.Guid? UserId
		{
			get { return (System.Guid?)this.GetProperty("UserId"); }
			set { this.SetProperty("UserId", value); }
		}


		[Association(Name = "Project_Project", Storage = "project", ThisKey = "ProjectTaskId")]
		public Project Project
		{
			get { return this.project.Entity; }
			set
			{
				Project previousValue = this.project.Entity;
				if (((previousValue != value)
					|| (this.project.HasLoadedOrAssignedValue == false)))
				{
					this.NotifyPropertyChanging("Project");
					if ((previousValue != null))
					{
						this.project.Entity = null;
						previousValue.ProjectTasks.Remove(this);
					}
					this.project.Entity = null;
					if (value != null)
					{
						value.ProjectTasks.Add(this);
						this.ProjectId = value.Id;
					}
					else
					{
						this.ProjectId = default(Nullable<Guid>);
					}
					this.NotifyPropertyChanged("Project");
				}
			}
		}




		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class ProjectTasks : ActsAsList<ProjectTask, ProjectTasks>
	{


	}

	[Table(Name = "amplify_TaskLinkedIds"), Serializable()]
	public partial class TaskLinkedId : Base<TaskLinkedId>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "TaskId", "LinkedId", "LinkedTypeId" };

		public TaskLinkedId()
			: base()
		{

		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column()]
		public System.Guid? TaskId
		{
			get { return (System.Guid?)this.GetProperty("TaskId"); }
			set { this.SetProperty("TaskId", value); }
		}
		[Column()]
		public System.String LinkedId
		{
			get { return (System.String)this.GetProperty("LinkedId"); }
			set { this.SetProperty("LinkedId", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? LinkedTypeId
		{
			get { return (System.Guid?)this.GetProperty("LinkedTypeId"); }
			set { this.SetProperty("LinkedTypeId", value); }
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class TaskLinkedIds : ActsAsList<TaskLinkedId, TaskLinkedIds>
	{


	}

	[Table(Name = "amplify_Tasks"), Serializable()]
	public partial class Task : Base<Task>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "Name", "IsParent", "ParentId" };

		public Task()
			: base()
		{

		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column(CanBeNull = false)]
		public System.String Name
		{
			get { return (System.String)this.GetProperty("Name"); }
			set { this.SetProperty("Name", value); }
		}
		[Column(CanBeNull = false)]
		public System.Boolean IsParent
		{
			get { return (System.Boolean)this.GetProperty("IsParent"); }
			set { this.SetProperty("IsParent", value); }
		}
		[Column()]
		public System.Guid? ParentId
		{
			get { return (System.Guid?)this.GetProperty("ParentId"); }
			set { this.SetProperty("ParentId", value); }
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class Tasks : ActsAsList<Task, Tasks>
	{


	}

	[Table(Name = "amplify_TimeLineEvents"), Serializable()]
	public partial class TimeLineEvent : Base<TimeLineEvent>
	{

		private static readonly string[] primaryKeys = new string[] { "Id" };
		private static readonly string[] properties = new string[] { "Id", "Summary", "Description", "UserId", "CreatedAt", "ProjectId" };

		public TimeLineEvent()
			: base()
		{

		}

		#region properties


		protected override IEnumerable<string> PrimaryKeys
		{
			get { return primaryKeys; }
		}

		protected override IEnumerable<string> Properties
		{
			get { return properties; }
		}

		[Column(IsPrimaryKey = true, CanBeNull = false)]
		public System.Guid? Id
		{
			get { return (System.Guid?)this.GetProperty("Id"); }
			set { this.SetProperty("Id", value); }
		}
		[Column(CanBeNull = false)]
		public System.String Summary
		{
			get { return (System.String)this.GetProperty("Summary"); }
			set { this.SetProperty("Summary", value); }
		}
		[Column()]
		public System.String Description
		{
			get { return (System.String)this.GetProperty("Description"); }
			set { this.SetProperty("Description", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? UserId
		{
			get { return (System.Guid?)this.GetProperty("UserId"); }
			set { this.SetProperty("UserId", value); }
		}
		[Column(CanBeNull = false)]
		public System.DateTime CreatedAt
		{
			get { return (System.DateTime)this.GetProperty("CreatedAt"); }
			set { this.SetProperty("CreatedAt", value); }
		}
		[Column(CanBeNull = false)]
		public System.Guid? ProjectId
		{
			get { return (System.Guid?)this.GetProperty("ProjectId"); }
			set { this.SetProperty("ProjectId", value); }
		}

		#endregion

		#region static methods

		#endregion
	}

	[Serializable()]
	public partial class TimeLineEvents : ActsAsList<TimeLineEvent, TimeLineEvents>
	{


	}



}


