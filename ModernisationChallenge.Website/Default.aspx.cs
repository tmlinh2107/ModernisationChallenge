using ModernisationChallenge.DataAccess;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModernisationChallenge
{
    public partial class _Default : Page
    {
        private int? CurrentTaskId
        {
            get
            {
                return (int?)ViewState["CurrentTaskId"];
            }
            set
            {
                ViewState["CurrentTaskId"] = value;
            }
        }

        protected void CancelLinkButton_Click(object sender, EventArgs e)
        {
            DialoguePlaceHolder.Visible = false;

            ScriptManager.RegisterStartupScript(Page, GetType(), "fadeToWhite", "fadeToWhite();", true);
        }

        protected void CloseLinkButton_Click(object sender, EventArgs e)
        {
            DialoguePlaceHolder.Visible = false;

            ScriptManager.RegisterStartupScript(Page, GetType(), "fadeToWhite", "fadeToWhite();", true);
        }

        protected void CompletedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var completedCheckBox = (CheckBox)sender;

            var taskIdHiddenField = (HiddenField)completedCheckBox.NamingContainer.FindControl("TaskIdHiddenField");

            using (var dataContext = new DataContext())
            {
                var task = dataContext.Tasks.Single(x => x.Id == int.Parse(taskIdHiddenField.Value));

                task.DateModified = DateTime.Now;
                task.Completed = completedCheckBox.Checked;

                dataContext.SubmitChanges();
            }
        }

        protected void CreateLinkButton_Click(object sender, EventArgs e)
        {
            CurrentTaskId = null;

            DialoguePlaceHolder.Visible = true;

            TitleLiteral.Text = "Create task";

            DetailsTextBox.Text = "";
        }
        
        private void LoadTasks()
        {
            using (var dataContext = new DataContext())
            {
                TasksRepeater.DataSource = (
                    from task in dataContext.Tasks

                    where task.DateDeleted == null

                    orderby task.Id

                    select new
                    {
                        Id = task.Id,
                        Completed = task.Completed,
                        Details = task.Details
                    }
                );

                TasksRepeater.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            LoadTasks();
        }

        protected void SaveLinkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DetailsTextBox.Text))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "snackbar", "snackbar(\"error\", \"One or more required fields haven't been filled in.\");", true);

                return;
            }

            using (var dataContext = new DataContext())
            {
                Task task;

                if (CurrentTaskId.HasValue)
                {
                    task = dataContext.Tasks.Single(x => x.Id == CurrentTaskId);
                }
                else
                {
                    task = new Task
                    {
                        DateCreated = DateTime.Now,
                        Completed = false
                    };

                    dataContext.Tasks.InsertOnSubmit(task);
                }

                task.DateModified = DateTime.Now;                
                task.Details = DetailsTextBox.Text;

                dataContext.SubmitChanges();
            }

            CurrentTaskId = null;

            DialoguePlaceHolder.Visible = false;

            ScriptManager.RegisterStartupScript(Page, GetType(), "fadeToWhite", "fadeToWhite();", true);

            LoadTasks();
        }

        protected void TasksRepeater_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    {
                        using (var dataContext = new DataContext())
                        {
                            var task = dataContext.Tasks.Single(x => x.Id == int.Parse((string)e.CommandArgument));

                            task.DateDeleted = DateTime.Now;

                            dataContext.SubmitChanges();
                        }

                        LoadTasks();

                        break;
                    }

                case "Edit":
                    {
                        using (var dataContext = new DataContext())
                        {
                            var task = dataContext.Tasks.Single(x => x.Id == int.Parse((string)e.CommandArgument));

                            CurrentTaskId = task.Id;

                            DialoguePlaceHolder.Visible = true;

                            TitleLiteral.Text = "Edit task";

                            DetailsTextBox.Text = task.Details;
                        }

                        break;
                    }
            }
        }
    }
}
