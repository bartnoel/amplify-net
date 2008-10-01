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
		private static Action<object, string, object> s_setValue;

		public static object GetPropertyValue(object source, string propertyName)
		{
			return source.GetType().GetProperty(propertyName).GetValue(source, null);
		}

		public static object SetPropertyValue(object source, string propertyName, object value)
		{
			source.GetType().GetProperty(propertyName).SetValue(source, value, null);
			return value;
		}

		public static Func<object, string, object> GetValue
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

		public static Action<object, string, object> SetValue
		{
			get
			{
				if (s_setValue == null)
				{
					Expression<Action<object, string, object>> tmpExpression = (source, propertyName, value) => SetPropertyValue(source, propertyName, value);
					var expression = new ExpressionVisitor().Compile(tmpExpression);
					s_setValue = expression.Compile();
				}
				return s_setValue;
			}
		}

		public Expression<Action<S, PN, V>> Compile<S, PN, V>(Expression<Action<S, PN, V>> source)
		{
			var lambda = Visit(source) as LambdaExpression;
			return lambda as Expression<Action<S, PN, V>>;
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
			{
				return Expression.Property(m.Arguments[0], arg1.Value as string);
			}
			if (m.Method.DeclaringType == typeof(ExpressionVisitor) && m.Method.Name == "SetPropertyValue" && (arg1 = m.Arguments[1] as ConstantExpression) != null)
			{
				return Expression.Property(m.Arguments[0], arg1.Value as string);
			}
			return base.VisitMethodCall(m);
		}

	
	}
}
