using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace busniessLayer
{
    public class clsTasks
    {
        private enum enMode { AddMode = 0, UpdateMode = 1};
        enMode _Mode = enMode.AddMode;

        public int TaskID { get; set; }

        public string Title {  get; set; }

        public string Description { get; set; }


        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public int UserID { get; set; }

        public clsTasks()
        {
            this.TaskID = -1;
            this.Title = "";
            this.Description = "";
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now;
            this.IsActive = true;
            this.UserID = -1;

            _Mode = enMode.AddMode;
        }

        public static clsTasks Find(int TaskID)
        {
            //( TaskID,Title, Description,  StartDate, EndDate, IsActive ,UserID)
            int UserID = -1;
            string Title = "", Description = "", Email = "", ImagePath = "", Password = "", UserName = "";
            DateTime StartDate = DateTime.Now, EndDate = DateTime.Now;
            bool IsActive = false;

            if (clsTaskDataAccess.FindByTaskID(TaskID, ref Title, ref Description,  ref StartDate, ref EndDate,
                ref IsActive, ref UserID))
            {
                return new clsTasks(TaskID,  Title,  Description,   StartDate,  EndDate, IsActive, UserID);

            }
            else
                return null;
        }

        public clsTasks(int taskID, string title, string description,  DateTime startDate, DateTime endDate, bool isActive, int userID)
        {
            TaskID = taskID;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            UserID = userID;

            _Mode = enMode.UpdateMode;
        }

        public static DataTable GetAllTasks(int taskID)
        {
            return clsTaskDataAccess.GetAllTasks(taskID);
        }

        public static bool DeleteTask(int taskID)
        {
            return clsTaskDataAccess.DeleteTask(taskID);
        }

        private bool AddNewTask()
        {
            this.TaskID = clsTaskDataAccess.AddNewTask(this.Title, this.Description,  this.StartDate,
                this.EndDate, this.IsActive, this.UserID);

            return (this.TaskID != -1);
        }

        private bool UpdateTask()
        {
            return clsTaskDataAccess.UpdateTask(this.TaskID, this.Title, this.Description, this.StartDate,
                this.EndDate, this.IsActive, this.UserID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    {
                        if (AddNewTask())
                        {
                            _Mode = enMode.UpdateMode;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.UpdateMode:
                    return UpdateTask();
            }
            return false;

        }


    }
}
