using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Linq
{
	using System.Linq.Expressions;
	using System.Reflection;

	public class ExpressionVisitor : System.Linq.Expressions.ExpressionVisitor 
	{
		private static Func<object, string, object> s_visitValue;

		public static object GetPropertyValue(object source, string propertyName)
		{
			return source.GetType().GetProperty(propertyName).GetValue(source, null);
		}

		public static Func<object, string, object> VistValue
		{
			get
			{
				if (s_visitValue == null)
				{
					Expression<Func<object, string, object>> tmpExpression = (source, propertyName) => GetPropertyValue(source, propertyName);
					var expression = new ExpressionVisitor().Compile(tmpExpression);
					s_visitValue = expression.Compile();
				}
				return s_visitValue;
			}
		}

		public Expression<Func<S, PN, R>> Compile<S, PN, R>(Expression<Func<S, PN, R>> source)
		{
			var lambda = Visit(source) as LambdaExpression;
			return lambda as Expression<Func<S, PN, R>>;
		}

		protected override Expression VisitMethodCall(MethodCallExpression m)
		{
			ConstantExpression arg1;
			if (m.Method.DeclaringType == typeof(ExpressionVisitor) && m.Method.Name == "GetPropertyValue" && (arg1 = m.Arguments[1] as ConstantExpression) != null)
				return Expression.Property(m.Arguments[0], arg1.Value as string);
			return base.VisitMethodCall(m);
		}
	}
}
