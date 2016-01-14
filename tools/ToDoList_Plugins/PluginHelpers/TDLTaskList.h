#pragma once

////////////////////////////////////////////////////////////////////////////////////////////////

class IPreferences;
class ITaskList;
class ITaskList14;
class ITransText;

struct UITHEME;

typedef void* HTASKITEM;

////////////////////////////////////////////////////////////////////////////////////////////////

using namespace System;

////////////////////////////////////////////////////////////////////////////////////////////////

#pragma warning( push )
#undef GetProfileInt
#undef GetProfileString

////////////////////////////////////////////////////////////////////////////////////////////////

namespace TDLPluginHelpers
{

   public ref class TDLTask
   {
   public:
      TDLTask(ITaskList* pTaskList, HTASKITEM hTask);        // GET & SET
      TDLTask(const ITaskList* pTaskList, HTASKITEM hTask);  // GET ONLY
      TDLTask(const TDLTask^ task);

      bool IsValid();

      TDLTask^ GetFirstSubtask();
      TDLTask^ GetNextTask();
      TDLTask^ GetParentTask();

      String^ GetTitle();
      String^ GetComments();
      String^ GetAllocatedBy();
      String^ GetStatus();
      String^ GetWebColor();
      String^ GetPriorityWebColor();
      String^ GetVersion();
      String^ GetExternalID();
      String^ GetCreatedBy();
      String^ GetPositionString();
      String^ GetIcon();

      UInt32 GetID();
      UInt32 GetColor();
      UInt32 GetTextColor();
      UInt32 GetPriorityColor();
      UInt32 GetPosition();
      UInt32 GetPriority();
      UInt32 GetRisk();

      UInt32 GetCategoryCount();
      UInt32 GetAllocatedToCount();
      UInt32 GetTagCount();
      UInt32 GetDependencyCount();
      UInt32 GetFileReferenceCount();

      String^ GetAllocatedTo(int nIndex);
      String^ GetCategory(int nIndex);
      String^ GetTag(int nIndex);
      String^ GetDependency(int nIndex);
      String^ GetFileReference(int nIndex);

      Byte GetPercentDone();
      double GetCost();

      Int64 GetLastModified();
      Int64 GetDoneDate();
      Int64 GetDueDate();
      Int64 GetStartDate();
      Int64 GetCreationDate();

      String^ GetDoneDateString();
      String^ GetDueDateString();
      String^ GetStartDateString();
      String^ GetCreationDateString();

      Boolean IsDone();
      Boolean IsDue();
      Boolean IsGoodAsDone();
      Boolean IsFlagged();
      
      double GetTimeEstimate(Char% cUnits);
      double GetTimeSpent(Char% cUnits);

      Boolean GetRecurrence(); // TODO
      Boolean HasAttribute(String^ sAttrib);

      String^ GetAttribute(String^ sAttrib);
      String^ GetCustomAttributeData(String^ sID);
      String^ GetMetaData(String^ sKey);

      // SETTERS -----------------------------------------------------
      
      TDLTask^ NewSubtask(String^ sTitle);

      Boolean SetTitle(String^ sTitle);
      Boolean SetComments(String^ sComments);
      Boolean SetAllocatedBy(String^ sAllocBy);
      Boolean SetStatus(String^ sStatus);
      Boolean SetVersion(String^ sVersion);
      Boolean SetExternalID(String^ sExternalID);
      Boolean SetCreatedBy(String^ sCreatedBy);
      Boolean SetPosition(String^ sPosition);
      Boolean SetIcon(String^ sIcon);

      Boolean AddAllocatedTo(String^ sAllocTo);
      Boolean AddCategory(String^ sCategory);
      Boolean AddTag(String^ sTag);
      Boolean AddDependency(String^ sDependency);
      Boolean AddFileReference(String^ sFileLink);

      Boolean SetColor(UInt32 color);
      Boolean SetPriority(Byte nPriority);
      Boolean SetRisk(Byte Risk);

      Boolean SetPercentDone(Byte nPercent);
      Boolean SetCost(double dCost);
      Boolean SetFlag(Boolean bFlag);

      Boolean SetLastModified(Int64 dtLastMod);
      Boolean SetDoneDate(Int64 dtCompletion);
      Boolean SetDueDate(Int64 dtDue);
      Boolean SetStartDate(Int64 dtStart);
      Boolean SetCreationDate(Int64 dtCreation);

      Boolean SetTimeEstimate(double dTime, Char cUnits);
      Boolean SetTimeSpent(double dTime, Char cUnits);
      Boolean SetCustomAttributeData(String^ sID, String^ sValue);
      Boolean ClearCustomAttributeData(String^ sID);
      Boolean SetMetaData(String^ sKey, String^ sValue);
      Boolean ClearMetaData(String^ sKey);
      
   private: // -------------------------------------------------------
      ITaskList14* m_pTaskList;
      const ITaskList14* m_pConstTaskList;
      HTASKITEM m_hTask;

   private: // -------------------------------------------------------
      TDLTask();
   };

////////////////////////////////////////////////////////////////////////////////////////////////

   public ref class TDLTaskList
   {
   public:
      TDLTaskList(ITaskList* pTaskList);        // GET & SET
      TDLTaskList(const ITaskList* pTaskList);  // GET ONLY
 
	  bool IsValid();

      String^ GetReportTitle();
      String^ GetReportDate();
      String^ GetMetaData(String^ sKey);
      
      UInt32  GetCustomAttributeCount();
      String^ GetCustomAttributeLabel(int nIndex);
      String^ GetCustomAttributeID(int nIndex);
      String^ GetCustomAttributeValue(int nIndex, String^ sItem);

      UInt32 GetTaskCount();
      TDLTask^ FindTask(UInt32 dwTaskID);

      TDLTask^ GetFirstTask();
      TDLTask^ NewTask(String^ sTitle);
      
      Boolean AddCustomAttribute(String^ sID, String^ sLabel);
      Boolean SetMetaData(String^ sKey, String^ sValue);
      Boolean ClearMetaData(String^ sKey);

   private: // -------------------------------------------------------
      ITaskList14* m_pTaskList;
      const ITaskList14* m_pConstTaskList;

   private: // -------------------------------------------------------
      TDLTaskList();
   };

}

#pragma warning( pop )

