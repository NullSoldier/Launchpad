﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LaunchPad
{
	public enum JobObjectInfoType
	{
		AssociateCompletionPortInformation = 7,
		BasicLimitInformation = 2,
		BasicUIRestrictions = 4,
		EndOfJobTimeInformation = 6,
		ExtendedLimitInformation = 9,
		SecurityLimitInformation = 5,
		GroupInformation = 11
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct SECURITY_ATTRIBUTES
	{
		public int nLength;
		public IntPtr lpSecurityDescriptor;
		public int bInheritHandle;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct JOBOBJECT_BASIC_LIMIT_INFORMATION
	{
		public Int64 PerProcessUserTimeLimit;
		public Int64 PerJobUserTimeLimit;
		public Int16 LimitFlags;
		public UInt32 MinimumWorkingSetSize;
		public UInt32 MaximumWorkingSetSize;
		public Int16 ActiveProcessLimit;
		public Int64 Affinity;
		public Int16 PriorityClass;
		public Int16 SchedulingClass;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct IO_COUNTERS
	{
		public UInt64 ReadOperationCount;
		public UInt64 WriteOperationCount;
		public UInt64 OtherOperationCount;
		public UInt64 ReadTransferCount;
		public UInt64 WriteTransferCount;
		public UInt64 OtherTransferCount;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
	{
		public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
		public IO_COUNTERS IoInfo;
		public UInt32 ProcessMemoryLimit;
		public UInt32 JobMemoryLimit;
		public UInt32 PeakProcessMemoryUsed;
		public UInt32 PeakJobMemoryUsed;
	}

	public class Job : IDisposable
	{
		[DllImport ("kernel32.dll", CharSet = CharSet.Unicode)]
		static extern IntPtr CreateJobObject(object a, string lpName);

		[DllImport ("kernel32.dll")]
		static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

		[DllImport ("kernel32.dll", SetLastError = true)]
		static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

		[DllImport ("kernel32", SetLastError = true)]
		static extern bool CloseHandle(IntPtr hObject);

		private IntPtr m_handle;
		private bool m_disposed = false;

		public Job (IntPtr handle)
		{
			m_handle = CreateJobObject (null, null);

			JOBOBJECT_BASIC_LIMIT_INFORMATION info = new JOBOBJECT_BASIC_LIMIT_INFORMATION ();
			info.LimitFlags = 0x2000;

			JOBOBJECT_EXTENDED_LIMIT_INFORMATION extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION ();
			extendedInfo.BasicLimitInformation = info;

			int length = Marshal.SizeOf (typeof (JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
			IntPtr extendedInfoPtr = Marshal.AllocHGlobal (length);
			Marshal.StructureToPtr (extendedInfo, extendedInfoPtr, false);

			if (!SetInformationJobObject (m_handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length))
				throw new Exception (string.Format ("Unable to set information.  Error: {0}", Marshal.GetLastWin32Error ()));

			AssignProcessToJobObject (m_handle, handle);
		}

		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private void Dispose(bool disposing)
		{
			if (m_disposed)
				return;

			if (disposing) { }

			Close ();
			m_disposed = true;
		}

		public void Close()
		{
			CloseHandle (m_handle);
			m_handle = IntPtr.Zero;
		}
	}
}