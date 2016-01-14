// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the EXPORTERBRIDGE_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// EXPORTERBRIDGE_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.

#include "..\..\..\Interfaces\IImportExport.h"

// This class is exported from ExporterBridge.dll
class CMarkdownImpExpBridge : public IExportTasklist
{
public:
	CMarkdownImpExpBridge();

   void Release(); // releases the interface

   void SetLocalizer(ITransText* pTT);

   LPCWSTR GetMenuText() const;
   LPCWSTR GetFileFilter() const;
   LPCWSTR GetFileExtension() const;

   bool Export(const ITaskList* pSrcTaskFile, LPCWSTR szDestFilePath, BOOL bSilent, IPreferences* pPrefs, LPCWSTR szKey);
   bool Export(const IMultiTaskList* pSrcTaskFile, LPCWSTR szDestFilePath, BOOL bSilent, IPreferences* pPrefs, LPCWSTR szKey);
};

DLL_DECLSPEC int GetInterfaceVersion()
{
   return IIMPORTEXPORT_VERSION;
}

DLL_DECLSPEC IExportTasklist* CreateExportInterface()
{
   return new CMarkdownImpExpBridge();
}

DLL_DECLSPEC IImportTasklist* CreateImportInterface()
{
   return NULL;
}
