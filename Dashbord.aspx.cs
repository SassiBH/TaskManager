﻿using busniessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static TasksManager.LoginPage;

namespace TasksManager
{
    public partial class Dashbord : System.Web.UI.Page
    {
        
        private enum enMode { AddTask=0, UpdateTask=1};
         enMode _Mode;

       static clsTasks Tasks;
       static clsAchievement Achievement;
       static clsUsers _User;
       int TaskID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Config();
            }



        }


        private void Config()
        {
            LoadTasks();
            BindAchievements();
            LoadSideBarInfo();
            BindAchievements();

        }



        private void LoadSideBarInfo()
        {
            string imagePath = Server.MapPath("~/imgs/man400px.png");
            if (!System.IO.File.Exists(imagePath))
            {
                throw new Exception($"File not found: {imagePath}");
            }

            //set Side Bar Settings
            if (string.IsNullOrEmpty(Globle._GUser.ImagePath))
            {
                imgUserProfile.ImageUrl = "~/imgs/man400px.png";
            }
            else
            {
                // Convert absolute path to relative
                string relativePath = Globle._GUser.ImagePath.Replace(Server.MapPath("~/"), "~/").Replace("\\", "/");
                imgUserProfile.ImageUrl = relativePath;
            }


            lblUserName.Text = Globle._GUser.UserName;
        }

        private void LoadTasks()
        {

            gvTasks.DataSource = clsTasks.GetAllTasks(Globle._GUser.UserID);
            
            gvTasks.DataBind();
        }

        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "MarkComplete")
            {
                
                clsTasks _Task = clsTasks.Find(int.Parse(e.CommandArgument.ToString()));

                if(_Task.IsActive)
                    _Task.IsActive = false;

                else
                    _Task.IsActive = true;

                if (_Task.Save())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Error",
                        "alert('Suceessfully')");
                }
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Error",
                        "alert('Failed')");

            }

            if (e.CommandName == "DeleteTask")
            {                
                clsTasks.DeleteTask(Convert.ToInt32(e.CommandArgument));
            }

            // Reload tasks
            LoadTasks();
        }

        [WebMethod]
        public static int TaskServerHandler(int TaskID)
        {
            if (TaskID == -1)
            {
                Tasks = new clsTasks(); 
            }
            else
                Tasks = clsTasks.Find(TaskID);


            return TaskID;
        }

        [WebMethod]
        public static int AhcievmentServerHandler(int AchievmentID)
        {
            if (AchievmentID == -1)
            {
                Achievement = new clsAchievement();
            }
            else
                Achievement = clsAchievement.Find(AchievmentID);


            return AchievmentID;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            Tasks.Title = txtTitle.Text;
            Tasks.Description = txtDescription.Text;
            Tasks.StartDate = DateTime.Parse(txtStartDate.Text);
            Tasks.EndDate = DateTime.Parse(txtEndDate.Text);
            Tasks.IsActive = chkIsActive.Checked;
            Tasks.UserID = Globle._GUser.UserID;
                        

            if (Tasks.Save())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    "alert('Suceessfully')");
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    "alert('Failed')");

            // Reload data in 
            LoadTasks();
        }


        private void BindAchievements()
        {

            gvAchievements.DataSource = clsAchievement.GetAllAchievement(Globle._GUser.UserID);
            gvAchievements.DataBind();
        }

        protected void btnAddAchievement_Click(object sender, EventArgs e)
        {
            //string achievement = txtAchievement.Text.Trim();
            //if (!string.IsNullOrEmpty(achievement))
            //{
            //    string query = "INSERT INTO Achievements (Achievement) VALUES (@Achievement)";
            //    using (SqlConnection conn = new SqlConnection("your_connection_string"))
            //    {
            //        using (SqlCommand cmd = new SqlCommand(query, conn))
            //        {
            //            cmd.Parameters.AddWithValue("@Achievement", achievement);
            //            conn.Open();
            //            cmd.ExecuteNonQuery();
            //        }
            //    }

            //    txtAchievement.Text = string.Empty;
            //    BindAchievements();
            //}
        }

        protected void btnUpdateAchievement_Click(object sender, EventArgs e)
        {
            //int noteId = int.Parse(hfNoteId.Value);
            //string updatedAchievement = txtAchievement.Text.Trim();

            //if (!string.IsNullOrEmpty(updatedAchievement))
            //{
            //    string query = "UPDATE Achievements SET Achievement = @Achievement WHERE NoteId = @NoteId";
            //    using (SqlConnection conn = new SqlConnection("your_connection_string"))
            //    {
            //        using (SqlCommand cmd = new SqlCommand(query, conn))
            //        {
            //            cmd.Parameters.AddWithValue("@Achievement", updatedAchievement);
            //            cmd.Parameters.AddWithValue("@NoteId", noteId);
            //            conn.Open();
            //            cmd.ExecuteNonQuery();
            //        }
            //    }

            //    txtAchievement.Text = string.Empty;
            //    hfNoteId.Value = string.Empty;
            //    btnAddAchievement.Visible = true;
            //    btnUpdateAchievement.Visible = false;
            //    btnCancelEdit.Visible = false;
            //    BindAchievements();
            //}
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            //txtAchievement.Text = string.Empty;
            //hfNoteId.Value = string.Empty;
            //btnAddAchievement.Visible = true;
            //btnUpdateAchievement.Visible = false;
            //btnCancelEdit.Visible = false;
        }

        protected void gvAchievements_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "DeleteAchievement")
            {

                clsAchievement.DeleteAchievement(Convert.ToInt32(e.CommandArgument));
                
            }
            BindAchievements();
        }


        protected void AddTask_Command(object sender, CommandEventArgs e)
        {
            lModalTitle.Text = "Add New new Task";
        }

        protected void AbtnSave_Click(object sender, EventArgs e)
        {
            Achievement.AchievementName = txtAchievement.Text;
            Achievement.AchievementDescription = txtAchievmentDescription.Text;
            Achievement.AchievementDate = DateTime.Parse(txtAchievementDate.Text);
            Achievement.UserID =Globle._GUser.UserID;
            
           

            if (Achievement.Save())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    "alert('Suceessfully')");
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    "alert('Failed')");

            BindAchievements();
        }

        [WebMethod]
        public static clsUsers GetUserDetails()
        {
            _User = clsUsers.Find(Globle._GUser.UserID);

            DateTime n = _User.DateOfBirth;
           
            return _User;
        }


        private void ReloadSideBarInfo()
        {
            imgUserProfile.ImageUrl = _User.ImagePath.Replace(Server.MapPath("~/"), "~/").Replace("\\", "/");                   
            lblUserName.Text = _User.UserName;
        }
        protected void saveButton_Click(object sender, EventArgs e)
        {
            string selectedGender = Request.Form["gender"];

            int d = _User.UserID;
            _User.FirstName = firstName.Text;
            _User.LastName = lastName.Text;
            _User.Email = email.Text;
            _User.DateOfBirth = DateTime.Parse(dob.Text);
            _User.Gender = selectedGender == "Male" ? (byte)1 : (byte)0;
            _User.ImagePath = @"F:\My_github_apps\TaskManegment\TheTasksManager\imgs\" + photo.FileName;
            photo.SaveAs(_User.ImagePath);
            _User.UserName = username.Text;
            _User.Password = password.Text;

            if(_User.Save()) 
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    "alert('Suceessfully')");
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    "alert('Failed')");

            ReloadSideBarInfo();
        }

    }
}
