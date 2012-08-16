using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ExtensionAttribute : Attribute
	{
	}
}