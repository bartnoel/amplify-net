using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Amplify.Diagnostics.Logging
{
	[Serializable]
	public class StackTraceInfo
	{

		public StackTraceInfo()
		{
			this.ClassName = Unknown;
			this.MethodName = Unknown;
			this.FileName = Unknown;
			this.LineNumber = Unknown;
		}

		public StackTraceInfo(string className, string methodName, string fileName, string lineNumber)
			:this()
		{
			if (className != null) 
				this.ClassName = className;

			if (methodName != null)
				this.MethodName = methodName;

			if (fileName != null)
				this.FileName = fileName;

			if (lineNumber != null)
				this.LineNumber = lineNumber;
		}

		public StackTraceInfo(Type typeToTrace)
			:this()
		{
			if (typeToTrace != null)
			{
				try
				{
					int index = 0;
					StackTrace stackTrace = new StackTrace(true);

					index = GetIndexForFrame(typeToTrace, index, stackTrace);
					index = GetIndexForFrame(typeToTrace, index, stackTrace);
					
					if (index < stackTrace.FrameCount)
					{
						// now frameIndex is the first 'user' caller frame
						StackFrame frame = stackTrace.GetFrame(index);

						if (frame != null)
						{
							MethodBase method = frame.GetMethod();

							if (method != null)
							{
								this.MethodName = method.Name;
								if (method.DeclaringType != null)
									this.ClassName = method.DeclaringType.FullName;
							}
							this.FileName = frame.GetFileName();
							this.LineNumber = frame.GetFileLineNumber().ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
						}
					}
				}
				catch(System.Security.SecurityException ex)
				{
					//TODO: log error
				}
			}
		}

		

		public string ClassName { get; protected set; }

		public string MethodName { get; protected set; }

		public string FileName { get; protected set; }

		public string LineNumber { get; protected set; }

		private const string Unknown = "N/A";


		private static int GetIndexForFrame(Type typeToTrace, int index, StackTrace stackTrace)
		{
			while (index < stackTrace.FrameCount)
			{
				StackFrame frame = stackTrace.GetFrame(index);
				if (frame != null && frame.GetMethod().DeclaringType == typeToTrace)
					break;

				index++;
			}
			return index;
		}

		public override string ToString()
		{
			if (	this.ClassName != Unknown &&
					this.MethodName != Unknown &&
					this.FileName != Unknown &&
					this.LineNumber != Unknown)
			{
				return string.Format("{0}.{1} in file {2} on line ({3})",
					this.ClassName, this.MethodName, this.FileName, this.LineNumber);
			}
			return "";
		}

	}
}
