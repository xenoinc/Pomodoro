﻿
using System;
using TDLPluginHelpers;

// PLS DON'T ADD 'USING' STATEMENTS WHILE I AM STILL LEARNING!

namespace SampleUIExtension
{
    public class SampleListItem
    {
        public String Type { get; set; }
        public String Attrib { get; set; }
        public String Value { get; set; }
        public String Tasks { get; set; }
    }

    public class SampleUIExtensionCore : System.Windows.Controls.Grid, ITDLUIExtension
    {
        public SampleUIExtensionCore()
        {
            InitializeComponent();
        }

        // ITDLUIExtension ------------------------------------------------------------------

        public bool SelectTask(UInt32 dwTaskID)
        {
            SampleListItem item = new SampleListItem();

            item.Value = dwTaskID.ToString();
            item.Type = "Selection";

            m_Items.Add(item);
            m_ListView.Items.Refresh();

            return true;
        }

	    public bool SelectTasks(UInt32[] pdwTaskIDs)
        {
            return false;
        }

	    public void UpdateTasks(TDLTaskList tasks, 
                                TDLUIExtension.UpdateType type, 
                                TDLUIExtension.TaskAttribute attrib)
        {
            TDLTask task = tasks.GetFirstTask();
            SampleListItem item = new SampleListItem();

            item.Tasks = task.GetID().ToString();
                    
            switch (attrib)
            {
                case TDLUIExtension.TaskAttribute.Title:            
                    item.Attrib = "Title";
                    item.Value = task.GetTitle();
                    break;

                case TDLUIExtension.TaskAttribute.DoneDate:         
                    item.Attrib = "Done Date";
                    item.Value = task.GetDoneDateString();
                    break;

                case TDLUIExtension.TaskAttribute.DueDate:          
                    item.Attrib = "Due Date";
                    item.Value = task.GetDueDateString();
                    break;
                
                case TDLUIExtension.TaskAttribute.StartDate:        
                    item.Attrib = "Start Date";
                    item.Value = task.GetStartDateString();
                    break;
                
                case TDLUIExtension.TaskAttribute.Priority:         
                    item.Attrib = "Priority";
                    item.Value = task.GetPriority().ToString();
                    break;
                
                case TDLUIExtension.TaskAttribute.Color:            
                    item.Attrib = "Color";
                    item.Value = task.GetColor().ToString();
                    break;
                
                case TDLUIExtension.TaskAttribute.AllocTo:          
                    item.Attrib = "Alloc To";
                    item.Value = task.GetAllocatedTo(0);
                    break;
                
                case TDLUIExtension.TaskAttribute.AllocBy:          
                    item.Attrib = "Alloc By";
                    item.Value = task.GetAllocatedBy();
                    break;
                
                case TDLUIExtension.TaskAttribute.Status:           
                    item.Attrib = "Status";
                    item.Value = task.GetStatus();
                    break;
                
                case TDLUIExtension.TaskAttribute.Category:         
                    item.Attrib = "Category";
                    item.Value = task.GetCategory(0);
                    break;
                
                case TDLUIExtension.TaskAttribute.Percent:          
                    item.Attrib = "Percent";
                    item.Value = task.GetPercentDone().ToString();
                    break;

                case TDLUIExtension.TaskAttribute.TimeEstimate:
                    {
                        item.Attrib = "Time Estimate";

                        Char units = 'H';
                        item.Value = (task.GetTimeEstimate(ref units).ToString() + units);
                    }
                    break;

                case TDLUIExtension.TaskAttribute.TimeSpent:
                    {
                        item.Attrib = "Time Spent";

                        Char units = 'H';
                        item.Value = (task.GetTimeSpent(ref units).ToString() + units);
                    }
                    break;
                
                case TDLUIExtension.TaskAttribute.FileReference:    
                    item.Attrib = "File Reference";
                    item.Value = task.GetFileReference(0);
                    break;
                
                case TDLUIExtension.TaskAttribute.Comments:         
                    item.Attrib = "Comments";
                    item.Value = task.GetComments();
                    break;
                
                case TDLUIExtension.TaskAttribute.Flag:             
                    item.Attrib = "Flag";
                    item.Value = task.IsFlagged().ToString();
                    break;
                
                case TDLUIExtension.TaskAttribute.CreationDate:     
                    item.Attrib = "Creation Date";
                    item.Value = task.GetCreationDateString();
                    break;
                
                case TDLUIExtension.TaskAttribute.CreatedBy:        
                    item.Attrib = "Created By";
                    item.Value = task.GetCreatedBy();
                    break;
                
                case TDLUIExtension.TaskAttribute.Risk:             
                    item.Attrib = "Risk";
                    item.Value = task.GetRisk().ToString();
                    break;
                
                case TDLUIExtension.TaskAttribute.ExternalId:       
                    item.Attrib = "External ID";
                    item.Value = task.GetExternalID();
                    break;
                
                case TDLUIExtension.TaskAttribute.Cost:             
                    item.Attrib = "Cost";
                    item.Value = task.GetCost().ToString();
                    break;
                
                case TDLUIExtension.TaskAttribute.Dependency:       
                    item.Attrib = "Dependency";
                    item.Value = task.GetDependency(0);
                    break;
                
                case TDLUIExtension.TaskAttribute.Recurrence:       
                    item.Attrib = "Recurrence";
                    //item.Value = task.GetRecurrence();
                    break;
                
                case TDLUIExtension.TaskAttribute.Version:          
                    item.Attrib = "Version";
                    item.Value = task.GetVersion();
                    break;
                
                case TDLUIExtension.TaskAttribute.Position:         
                    item.Attrib = "Position";
                    item.Value = task.GetPositionString();
                    break;
                
                case TDLUIExtension.TaskAttribute.Id:               
                    item.Attrib = "ID";
                    item.Value = task.GetID().ToString();
                    break;
                
                case TDLUIExtension.TaskAttribute.LastModified:     
                    item.Attrib = "Last Modified";
                    //item.Value = task.GetLastModifiedString();
                    break;
                
                case TDLUIExtension.TaskAttribute.Icon:             
                    item.Attrib = "Icon";
                    item.Value = task.GetIcon();
                    break;
                
                case TDLUIExtension.TaskAttribute.Tag:              
                    item.Attrib = "Tag";
                    item.Value = task.GetTag(0);
                    break;
                
                case TDLUIExtension.TaskAttribute.CustomAttribute:  
                    item.Attrib = "Custom Attribute";
                    //item.Value = task.GetCustomAttributeData();
                    break;
                
                case TDLUIExtension.TaskAttribute.All:              
                    item.Attrib = "All";
                    item.Value = "...";
                    break;
                
                case TDLUIExtension.TaskAttribute.Unknown:          
                    item.Attrib = "Unknown";            
                    break;
            }

            switch (type)
            {
                case TDLUIExtension.UpdateType.Edit:    item.Type = "Edit";             break;
                case TDLUIExtension.UpdateType.Add:     item.Type = "Add Task";         break;
                case TDLUIExtension.UpdateType.Delete:  item.Type = "Move Task(s)";     break;
                case TDLUIExtension.UpdateType.Move:    item.Type = "Delete Task(s)";   break;
                case TDLUIExtension.UpdateType.All:     item.Type = "All";              break;
                case TDLUIExtension.UpdateType.Unknown: item.Type = "Unknown";          break;
            }


            m_Items.Add(item);
            m_ListView.Items.Refresh();
        }

        public bool WantUpdate(TDLUIExtension.TaskAttribute attrib)
	    {
			return true; // all updates
	    }
	   
        public bool PrepareNewTask(TDLTaskList task)
	    {
		    return false;
    	}

        public bool ProcessMessage(IntPtr hwnd, UInt32 message, UInt32 wParam, UInt32 lParam, UInt32 time, Int32 xPos, Int32 yPos)
	    {
		    return false;
	    }
	   
		public void DoAppCommand(TDLUIExtension.AppCommand cmd, UInt32 dwExtra)
		{
		}

	    public bool CanDoAppCommand(TDLUIExtension.AppCommand cmd, UInt32 dwExtra)
	    {
		    return false;
	    }

	    public bool GetLabelEditRect(ref Int32 left, ref Int32 top, ref Int32 right, ref Int32 bottom)
	    {
			return false;
	    }

        public TDLUIExtension.HitResult HitTest(Int32 xPos, Int32 yPos)
	    {
            return TDLUIExtension.HitResult.Nowhere;
	    }

        public void SetUITheme(TDLTheme theme)
        {
            System.Windows.Media.Color bkColor = theme.GetAppColor(TDLTheme.AppColor.AppBackDark);

            this.Background = new System.Windows.Media.SolidColorBrush(bkColor);
        }

        public void SetReadOnly(bool bReadOnly)
	    {
	    }

	    public void SavePreferences(TDLPreferences prefs, String key)
	    {
	    }

	    public void LoadPreferences(TDLPreferences prefs, String key, bool appOnly)
	    {
	    }
        
        // PRIVATE ------------------------------------------------------------------------------

        private void InitializeComponent()
        {
            this.Background = System.Windows.Media.Brushes.White;

            CreateListView();
            PopulateListView();
        }

        private void CreateListView()
        {
            m_ListView = new System.Windows.Controls.ListView();
            m_GridView = new System.Windows.Controls.GridView();

            m_TypeCol = new System.Windows.Controls.GridViewColumn();
            m_TypeCol.DisplayMemberBinding = new System.Windows.Data.Binding("Type");
            m_TypeCol.Header = "Change Type";
            m_TypeCol.Width = 200;
            m_GridView.Columns.Add(m_TypeCol);

            m_AttribCol = new System.Windows.Controls.GridViewColumn();
            m_AttribCol.DisplayMemberBinding = new System.Windows.Data.Binding("Attrib");
            m_AttribCol.Header = "Attribute Changed";
            m_AttribCol.Width = 200;
            m_GridView.Columns.Add(m_AttribCol);

            m_ValueCol = new System.Windows.Controls.GridViewColumn();
            m_ValueCol.DisplayMemberBinding = new System.Windows.Data.Binding("Value");
            m_ValueCol.Header = "New Value";
            m_ValueCol.Width = 200;
            m_GridView.Columns.Add(m_ValueCol);

            m_TasksCol = new System.Windows.Controls.GridViewColumn();
            m_TasksCol.DisplayMemberBinding = new System.Windows.Data.Binding("Tasks");
            m_TasksCol.Header = "Tasks Changed";
            m_TasksCol.Width = 200;
            m_GridView.Columns.Add(m_TasksCol);

            m_ListView.View = m_GridView;

            this.Children.Add(m_ListView);
        }

        private void PopulateListView()
        {
            m_Items = new System.Collections.Generic.List<SampleListItem>();
            m_ListView.ItemsSource = m_Items;
        }

        // --------------------------------------------------------------------------------------
        private System.Collections.Generic.List<SampleListItem> m_Items;
        private System.Windows.Controls.ListView m_ListView;
        private System.Windows.Controls.GridView m_GridView;
        private System.Windows.Controls.GridViewColumn m_TypeCol;
        private System.Windows.Controls.GridViewColumn m_AttribCol;
        private System.Windows.Controls.GridViewColumn m_ValueCol;
        private System.Windows.Controls.GridViewColumn m_TasksCol;
    }
}
