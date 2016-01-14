// PluginHelpers.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "pluginhelpers.h"
#include "TDLTasklist.h"

#include "..\Interfaces\ITasklist.h"

////////////////////////////////////////////////////////////////////////////////////////////////

using namespace TDLPluginHelpers;

////////////////////////////////////////////////////////////////////////////////////////////////

TDLTaskList::TDLTaskList(ITaskList* pTaskList) 
	: 
	m_pTaskList(GetITLInterface<ITaskList14>(pTaskList, IID_TASKLIST14)), 
	m_pConstTaskList(nullptr) 
{
} 

TDLTaskList::TDLTaskList(const ITaskList* pTaskList) 
	: 
	m_pTaskList(nullptr), 
	m_pConstTaskList(GetITLInterface<ITaskList14>(pTaskList, IID_TASKLIST14)) 
{
} 

// private constructor
TDLTaskList::TDLTaskList() : m_pTaskList(nullptr), m_pConstTaskList(nullptr)
{

}

bool TDLTaskList::IsValid()
{
	return (m_pConstTaskList || m_pTaskList);
}

////////////////////////////////////////////////////////////////////////////////////////////////
// GETTERS

#define GETVAL(fn, errret) \
   (m_pConstTaskList ? m_pConstTaskList->fn() : (m_pTaskList ? m_pTaskList->fn() : errret))

#define GETSTR(fn) \
   gcnew String(m_pConstTaskList ? m_pConstTaskList->fn() : (m_pTaskList ? m_pTaskList->fn() : L""))

#define GETVAL_ARG(fn, arg, errret) \
   (m_pConstTaskList ? m_pConstTaskList->fn(arg) : (m_pTaskList ? m_pTaskList->fn(arg) : errret))

#define GETSTR_ARG(fn, arg) \
   gcnew String(m_pConstTaskList ? m_pConstTaskList->fn(arg) : (m_pTaskList ? m_pTaskList->fn(arg) : L""))

// -------------------------------------------------------------

String^ TDLTaskList::GetReportTitle()
{
   return GETSTR(GetReportTitle);
}

String^ TDLTaskList::GetReportDate()
{
   return GETSTR(GetReportDate);
}

String^ TDLTaskList::GetMetaData(String^ sKey)
{
   return GETSTR_ARG(GetMetaData, MS(sKey));
}

UInt32 TDLTaskList::GetCustomAttributeCount()
{
   return GETVAL(GetCustomAttributeCount, 0);
}

String^ TDLTaskList::GetCustomAttributeLabel(int nIndex)
{
   return GETSTR_ARG(GetCustomAttributeLabel, nIndex);
}

String^ TDLTaskList::GetCustomAttributeID(int nIndex)
{
   return GETSTR_ARG(GetCustomAttributeID, nIndex);
}

String^ TDLTaskList::GetCustomAttributeValue(int nIndex, String^ sItem)
{
    LPCWSTR szValue = (m_pConstTaskList ? m_pConstTaskList->GetCustomAttributeValue(nIndex, MS(sItem)) : 
                      (m_pTaskList ? m_pTaskList->GetCustomAttributeValue(nIndex, MS(sItem)) : L""));
   
    return gcnew String(szValue);
}

UInt32 TDLTaskList::GetTaskCount()
{
   return GETVAL(GetTaskCount, 0);
}

TDLTask^ TDLTaskList::FindTask(UInt32 dwTaskID)
{
   HTASKITEM hTask = GETVAL_ARG(FindTask, dwTaskID, NULL);

   return gcnew TDLTask((m_pConstTaskList ? m_pConstTaskList : m_pTaskList), hTask);
}

TDLTask^ TDLTaskList::GetFirstTask()
{
   if (m_pConstTaskList)
      return gcnew TDLTask(m_pConstTaskList, m_pConstTaskList->GetFirstTask(nullptr));

   // else
   return gcnew TDLTask(m_pTaskList, m_pTaskList->GetFirstTask(nullptr));
}

// ---------------------------------------------------------

#define GETTASKVAL(fn, errret) \
   (m_pConstTaskList ? m_pConstTaskList->fn(m_hTask) : (m_pTaskList ? m_pTaskList->fn(m_hTask) : errret))

#define GETTASKDATE(fn, errret) \
   (m_pConstTaskList ? m_pConstTaskList->fn(m_hTask, date) : (m_pTaskList ? m_pTaskList->fn(m_hTask, date) : errret))

#define GETTASKSTR(fn) \
   gcnew String(GETTASKVAL(fn, L""))

#define GETTASKVAL_ARG(fn, arg, errret) \
   (m_pConstTaskList ? m_pConstTaskList->fn(m_hTask, arg) : (m_pTaskList ? m_pTaskList->fn(m_hTask, arg) : errret))

#define GETTASKSTR_ARG(fn, arg) \
   gcnew String(GETTASKVAL_ARG(fn, arg, L""))

#define GETTASKDATE_ARG(fn, arg, errret) \
   (m_pConstTaskList ? m_pConstTaskList->fn(m_hTask, arg, date) : (m_pTaskList ? m_pTaskList->fn(m_hTask, arg, date) : errret))

// ---------------------------------------------------------

TDLTask::TDLTask()
	: 
	m_pTaskList(nullptr), 
	m_pConstTaskList(nullptr), 
	m_hTask(nullptr)
{

}

TDLTask::TDLTask(ITaskList* pTaskList, HTASKITEM hTask) 
	: 
	m_pTaskList(GetITLInterface<ITaskList14>(pTaskList, IID_TASKLIST14)), 
	m_pConstTaskList(nullptr), 
	m_hTask(hTask)
{

}

TDLTask::TDLTask(const ITaskList* pTaskList, HTASKITEM hTask)
	: 
	m_pTaskList(nullptr), 
	m_pConstTaskList(GetITLInterface<ITaskList14>(pTaskList, IID_TASKLIST14)), 
	m_hTask(hTask)
{

}

TDLTask::TDLTask(const TDLTask^ task)
{
   m_pTaskList = task->m_pTaskList;
   m_pConstTaskList = task->m_pConstTaskList;
   m_hTask = task->m_hTask;
}

bool TDLTask::IsValid()
{
   return ((m_pConstTaskList || m_pTaskList) && (m_hTask != nullptr));
}

TDLTask^ TDLTask::GetFirstSubtask()
{
   if (m_pConstTaskList)
      return gcnew TDLTask(m_pConstTaskList, m_pConstTaskList->GetFirstTask(m_hTask));
   
   // else
   return gcnew TDLTask(m_pTaskList, m_pTaskList->GetFirstTask(m_hTask));
}

TDLTask^ TDLTask::GetNextTask()
{
   if (m_pConstTaskList)
      return gcnew TDLTask(m_pConstTaskList, m_pConstTaskList->GetNextTask(m_hTask));

   // else
   return gcnew TDLTask(m_pTaskList, m_pTaskList->GetNextTask(m_hTask));
}

TDLTask^ TDLTask::GetParentTask()
{
   if (m_pConstTaskList)
      return gcnew TDLTask(m_pConstTaskList, m_pConstTaskList->GetTaskParent(m_hTask));

   // else
   return gcnew TDLTask(m_pTaskList, m_pTaskList->GetTaskParent(m_hTask));
}

String^ TDLTask::GetTitle()
{
   return GETTASKSTR(GetTaskTitle);
}

String^ TDLTask::GetComments()
{
   return GETTASKSTR(GetTaskComments);
}

String^ TDLTask::GetAllocatedBy()
{
   return GETTASKSTR(GetTaskAllocatedBy);
}

String^ TDLTask::GetStatus()
{
   return GETTASKSTR(GetTaskStatus);
}

String^ TDLTask::GetWebColor()
{
   return GETTASKSTR(GetTaskWebColor);
}

String^ TDLTask::GetPriorityWebColor()
{
   return GETTASKSTR(GetTaskPriorityWebColor);
}

String^ TDLTask::GetVersion()
{
   return GETTASKSTR(GetTaskVersion);
}

String^ TDLTask::GetExternalID()
{
   return GETTASKSTR(GetTaskExternalID);
}

String^ TDLTask::GetCreatedBy()
{
   return GETTASKSTR(GetTaskCreatedBy);
}

String^ TDLTask::GetPositionString()
{
   return GETTASKSTR(GetTaskPositionString);
}

String^ TDLTask::GetIcon()
{
   return GETTASKSTR(GetTaskIcon);
}

UInt32 TDLTask::GetID()
{
   return GETTASKVAL(GetTaskID, 0);
}

UInt32 TDLTask::GetColor()
{
   return GETTASKVAL(GetTaskColor, 0);
}

UInt32 TDLTask::GetTextColor()
{
   return GETTASKVAL(GetTaskTextColor, 0);
}

UInt32 TDLTask::GetPriorityColor()
{
   return GETTASKVAL(GetTaskPriorityColor, 0);
}

UInt32 TDLTask::GetPosition()
{
   return GETTASKVAL(GetTaskPosition, 0);
}

UInt32 TDLTask::GetPriority()
{
   return GETTASKVAL_ARG(GetTaskPriority, FALSE, 0);
}

UInt32 TDLTask::GetRisk()
{
   return GETTASKVAL_ARG(GetTaskRisk, FALSE, 0);
}

UInt32 TDLTask::GetCategoryCount()
{
   return GETTASKVAL(GetTaskCategoryCount, 0);
}

UInt32 TDLTask::GetAllocatedToCount()
{
   return GETTASKVAL(GetTaskAllocatedToCount, 0);
}

UInt32 TDLTask::GetTagCount()
{
   return GETTASKVAL(GetTaskTagCount, 0);
}

UInt32 TDLTask::GetDependencyCount()
{
   return GETTASKVAL(GetTaskDependencyCount, 0);
}

UInt32 TDLTask::GetFileReferenceCount()
{
   return GETTASKVAL(GetTaskFileReferenceCount, 0);
}

String^ TDLTask::GetAllocatedTo(int nIndex)
{
   return GETTASKSTR_ARG(GetTaskAllocatedTo, nIndex);
}

String^ TDLTask::GetCategory(int nIndex)
{
   return GETTASKSTR_ARG(GetTaskCategory, nIndex);
}

String^ TDLTask::GetTag(int nIndex)
{
   return GETTASKSTR_ARG(GetTaskTag, nIndex);
}

String^ TDLTask::GetDependency(int nIndex)
{
   return GETTASKSTR_ARG(GetTaskDependency, nIndex);
}

String^ TDLTask::GetFileReference(int nIndex)
{
   return GETTASKSTR_ARG(GetTaskFileReference, nIndex);
}

Byte TDLTask::GetPercentDone()
{
   return GETTASKVAL_ARG(GetTaskPercentDone, FALSE, 0);
}

double TDLTask::GetCost()
{
   return GETTASKVAL_ARG(GetTaskCost, FALSE, 0);
}

Int64 TDLTask::GetLastModified()
{
   __int64 date = 0;
   GETTASKDATE(GetTaskLastModified64, 0);

   return date;
}

Int64 TDLTask::GetDoneDate()
{
   __int64 date = 0;
   GETTASKDATE(GetTaskDoneDate64, 0);

   return date;
}

Int64 TDLTask::GetDueDate()
{
   __int64 date = 0;
   GETTASKDATE_ARG(GetTaskDueDate64, FALSE, 0);

   return date;
}

Int64 TDLTask::GetStartDate()
{
   __int64 date = 0;
   GETTASKDATE_ARG(GetTaskStartDate64, FALSE, 0);

   return date;
}

Int64 TDLTask::GetCreationDate()
{
   __int64 date = 0;
   GETTASKDATE(GetTaskCreationDate64, 0);

   return date;
}

String^ TDLTask::GetDoneDateString()
{
   return GETTASKSTR(GetTaskDoneDateString);
}

String^ TDLTask::GetDueDateString()
{
   return GETTASKSTR_ARG(GetTaskDueDateString, FALSE);
}

String^ TDLTask::GetStartDateString()
{
   return GETTASKSTR_ARG(GetTaskStartDateString, FALSE);
}

String^ TDLTask::GetCreationDateString()
{
   return GETTASKSTR(GetTaskCreationDateString);
}

Boolean TDLTask::IsDone()
{
   return GETTASKVAL(IsTaskDone, false);
}

Boolean TDLTask::IsDue()
{
   return GETTASKVAL(IsTaskDue, false);
}

Boolean TDLTask::IsGoodAsDone()
{
   return GETTASKVAL(IsTaskGoodAsDone, false);
}

Boolean TDLTask::IsFlagged()
{
   return GETTASKVAL(IsTaskFlagged, false);
}

// ---------------------------------------------------------

double TDLTask::GetTimeEstimate(Char% cUnits)
{
   WCHAR nUnits = 0;

   double dTime = (m_pConstTaskList ? m_pConstTaskList->GetTaskTimeEstimate(m_hTask, nUnits, FALSE) :
                  (m_pTaskList ? m_pTaskList->GetTaskTimeEstimate(m_hTask, nUnits, FALSE) : 0.0));

   cUnits = nUnits;
   return dTime;
}

double TDLTask::GetTimeSpent(Char% cUnits)
{
   WCHAR nUnits = 0;

   double dTime = (m_pConstTaskList ? m_pConstTaskList->GetTaskTimeSpent(m_hTask, nUnits, FALSE) :
                  (m_pTaskList ? m_pTaskList->GetTaskTimeSpent(m_hTask, nUnits, FALSE) : 0.0));

   cUnits = nUnits;
   return dTime;
}

Boolean TDLTask::GetRecurrence()
{
	// TODO
	return false;
}

Boolean TDLTask::HasAttribute(String^ sAttrib)
{
   return (m_pConstTaskList ? m_pConstTaskList->TaskHasAttribute(m_hTask, MS(sAttrib)) : 
          (m_pTaskList ? m_pTaskList->TaskHasAttribute(m_hTask, MS(sAttrib)) : false));
}

String^ TDLTask::GetAttribute(String^ sAttrib)
{
   LPCWSTR szValue = (m_pConstTaskList ? m_pConstTaskList->GetTaskAttribute(m_hTask, MS(sAttrib)) : 
                     (m_pTaskList ? m_pTaskList->GetTaskAttribute(m_hTask, MS(sAttrib)) : L""));

   return gcnew String(szValue);
}

String^ TDLTask::GetCustomAttributeData(String^ sID)
{
   LPCWSTR szValue = (m_pConstTaskList ? m_pConstTaskList->GetTaskCustomAttributeData(m_hTask, MS(sID)) : 
                     (m_pTaskList ? m_pTaskList->GetTaskCustomAttributeData(m_hTask, MS(sID)) : L""));

   return gcnew String(szValue);
}

String^ TDLTask::GetMetaData(String^ sKey)
{
   LPCWSTR szValue = (m_pConstTaskList ? m_pConstTaskList->GetTaskMetaData(m_hTask, MS(sKey)) : 
                     (m_pTaskList ? m_pTaskList->GetTaskMetaData(m_hTask, MS(sKey)) : L""));

   return gcnew String(szValue);
}

// TODO

////////////////////////////////////////////////////////////////////////////////////////////////
// SETTERS

TDLTask^ TDLTaskList::NewTask(String^ sTitle)
{
   HTASKITEM hTask = (m_pTaskList ? m_pTaskList->NewTask(MS(sTitle), nullptr, 0) : nullptr);

   return gcnew TDLTask(m_pTaskList, hTask);
}

Boolean TDLTaskList::AddCustomAttribute(String^ sID, String^ sLabel)
{
   return (m_pTaskList ? m_pTaskList->AddCustomAttribute(MS(sID), MS(sLabel)) : false);
}

Boolean TDLTaskList::SetMetaData(String^ sKey, String^ sValue)
{
   return (m_pTaskList ? m_pTaskList->SetMetaData(MS(sKey), MS(sValue)) : false);
}

Boolean TDLTaskList::ClearMetaData(String^ sKey)
{
   return (m_pTaskList ? m_pTaskList->ClearMetaData(MS(sKey)) : false);
}

// ---------------------------------------------------------

#define SETTASKVAL(fn, val) \
   (m_pTaskList ? m_pTaskList->fn(m_hTask, val) : false)

#define SETTASKSTR(fn, str) \
   (m_pTaskList ? m_pTaskList->fn(m_hTask, MS(str)) : false)

// ---------------------------------------------------------

TDLTask^ TDLTask::NewSubtask(String^ sTitle)
{
   HTASKITEM hTask = (m_pTaskList ? m_pTaskList->NewTask(MS(sTitle), nullptr, 0) : nullptr);

   return gcnew TDLTask(m_pTaskList, hTask);
}

Boolean TDLTask::SetTitle(String^ sTitle)
{
   return SETTASKSTR(SetTaskTitle, sTitle);
}

Boolean TDLTask::SetComments(String^ sComments)
{
   return SETTASKSTR(SetTaskComments, sComments);
}

Boolean TDLTask::SetAllocatedBy(String^ sAllocBy)
{
   return SETTASKSTR(SetTaskAllocatedBy, sAllocBy);
}

Boolean TDLTask::SetStatus(String^ sStatus)
{
   return SETTASKSTR(SetTaskStatus, sStatus);
}

Boolean TDLTask::SetVersion(String^ sVersion)
{
   return SETTASKSTR(SetTaskVersion, sVersion);
}

Boolean TDLTask::SetExternalID(String^ sExternalID)
{
   return SETTASKSTR(SetTaskExternalID, sExternalID);
}

Boolean TDLTask::SetCreatedBy(String^ sCreatedBy)
{
   return SETTASKSTR(SetTaskCreatedBy, sCreatedBy);
}

Boolean TDLTask::SetPosition(String^ sPosition)
{
   return SETTASKSTR(SetTaskPosition, sPosition);
}

Boolean TDLTask::SetIcon(String^ sIcon)
{
   return SETTASKSTR(SetTaskIcon, sIcon);
}

Boolean TDLTask::AddAllocatedTo(String^ sAllocTo)
{
   return SETTASKSTR(AddTaskAllocatedTo, sAllocTo);
}

Boolean TDLTask::AddCategory(String^ sCategory)
{
   return SETTASKSTR(AddTaskCategory, sCategory);
}

Boolean TDLTask::AddTag(String^ sTag)
{
   return SETTASKSTR(AddTaskTag, sTag);
}

Boolean TDLTask::AddDependency(String^ sDependency)
{
   return SETTASKSTR(AddTaskDependency, sDependency);
}

Boolean TDLTask::AddFileReference(String^ sFileLink)
{
   return SETTASKSTR(AddTaskFileReference, sFileLink);
}

Boolean TDLTask::SetColor(UINT32 color)
{
   return SETTASKVAL(SetTaskColor, color);
}

Boolean TDLTask::SetPriority(Byte nPriority)
{
   return SETTASKVAL(SetTaskPriority, nPriority);
}

Boolean TDLTask::SetRisk(Byte Risk)
{
   return SETTASKVAL(SetTaskRisk, Risk);
}

Boolean TDLTask::SetPercentDone(Byte nPercent)
{
   return SETTASKVAL(SetTaskPercentDone, nPercent);
}

Boolean TDLTask::SetCost(double dCost)
{
   return SETTASKVAL(SetTaskCost, dCost);
}

Boolean TDLTask::SetFlag(Boolean bFlag)
{
   return SETTASKVAL(SetTaskFlag, bFlag);
}

Boolean TDLTask::SetLastModified(Int64 dtLastMod)
{
   return SETTASKVAL(SetTaskLastModified, dtLastMod);
}

Boolean TDLTask::SetDoneDate(Int64 dtCompletion)
{
   return SETTASKVAL(SetTaskDoneDate, dtCompletion);
}

Boolean TDLTask::SetDueDate(Int64 dtDue)
{
   return SETTASKVAL(SetTaskDueDate, dtDue);
}

Boolean TDLTask::SetStartDate(Int64 dtStart)
{
   return SETTASKVAL(SetTaskStartDate, dtStart);
}

Boolean TDLTask::SetCreationDate(Int64 dtCreation)
{
   return SETTASKVAL(SetTaskCreationDate, dtCreation);
}

Boolean TDLTask::SetTimeEstimate(double dTime, Char cUnits)
{
   return (m_pTaskList ? m_pTaskList->SetTaskTimeEstimate(m_hTask, dTime, cUnits) : false);
}

Boolean TDLTask::SetTimeSpent(double dTime, Char cUnits)
{
   return (m_pTaskList ? m_pTaskList->SetTaskTimeSpent(m_hTask, dTime, cUnits) : false);
}

Boolean TDLTask::SetCustomAttributeData(String^ sID, String^ sValue)
{
   return (m_pTaskList ? m_pTaskList->SetTaskCustomAttributeData(m_hTask, MS(sID), MS(sValue)) : false);
}

Boolean TDLTask::ClearCustomAttributeData(String^ sID)
{
   return (m_pTaskList ? m_pTaskList->ClearTaskCustomAttributeData(m_hTask, MS(sID)) : false);
}

Boolean TDLTask::SetMetaData(String^ sKey, String^ sValue)
{
   return (m_pTaskList ? m_pTaskList->SetTaskMetaData(m_hTask, MS(sKey), MS(sValue)) : false);
}

Boolean TDLTask::ClearMetaData(String^ sKey)
{
   return (m_pTaskList ? m_pTaskList->ClearTaskMetaData(m_hTask, MS(sKey)) : false);
}

////////////////////////////////////////////////////////////////////////////////////////////////
