// ExporterBridge.cpp : Defines the exported functions for the DLL application.
//

#include <unknwn.h>
#include <tchar.h>

#include "stdafx.h"
#include "MarkdownImpExpBridge.h"

#include "..\..\..\Interfaces\ITasklist.h"
#include "..\..\..\Interfaces\ITransText.h"
#include "..\..\..\Interfaces\IPreferences.h"

////////////////////////////////////////////////////////////////////////////////////////////////

#using <..\Debug\MarkdownImpExpCore.dll>
#include <msclr\auto_gcroot.h>

#using <..\Debug\PluginHelpers.dll> as_friend

using namespace MarkdownImpExp;
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace TDLPluginHelpers;

////////////////////////////////////////////////////////////////////////////////////////////////

// This is the constructor of a class that has been exported.
// see ExporterBridge.h for the class definition
CMarkdownImpExpBridge::CMarkdownImpExpBridge()
{
	return;
}

void CMarkdownImpExpBridge::Release()
{
	delete this;
}

void CMarkdownImpExpBridge::SetLocalizer(ITransText* /*pTT*/)
{
	// TODO
}

LPCWSTR CMarkdownImpExpBridge::GetMenuText() const
{
	return L"Markdown";
}

LPCWSTR CMarkdownImpExpBridge::GetFileFilter() const
{
	return L"md";
}

LPCWSTR CMarkdownImpExpBridge::GetFileExtension() const
{
	return L"md";
}

////////////////////////////////////////////////////////////////////////////////////////////////

bool CMarkdownImpExpBridge::Export(const ITaskList* pSrcTaskFile, LPCWSTR szDestFilePath, BOOL bSilent, IPreferences* pPrefs, LPCWSTR szKey)
{
   const ITaskList14* pTasks14 = GetITLInterface<ITaskList14>(pSrcTaskFile, IID_TASKLIST14);

   if (pTasks14 == nullptr)
   {
      MessageBox(NULL, L"You need a minimum ToDoList version of 7.0 to use this plugin", L"Version Not Supported", MB_OK);
      return false;
   }

	// call into out sibling C# module to do the actual work
	msclr::auto_gcroot<MarkdownImpExpCore^> expCore = gcnew MarkdownImpExpCore();
	msclr::auto_gcroot<TDLPreferences^> prefs = gcnew TDLPreferences(pPrefs);
	msclr::auto_gcroot<TDLTaskList^> srcTasks = gcnew TDLTaskList(pSrcTaskFile);
	
	// do the export
	return expCore->Export(srcTasks.get(), gcnew String(szDestFilePath), (bSilent != FALSE), prefs.get(), gcnew String(szKey));
}

bool CMarkdownImpExpBridge::Export(const IMultiTaskList* pSrcTaskFile, LPCWSTR szDestFilePath, BOOL bSilent, IPreferences* pPrefs, LPCWSTR szKey)
{
	// TODO
	return false;
}
