using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MonoTouch.Dialog;
using TODOListPortableLibrary;

namespace TODOList.iOS
{
    public class TODOViewController : DialogViewController
    {

        List<Task> todoLists;
        Task currentTask;
        TaskDialog taskDialog;
        DialogViewController detailsScreen;
        BindingContext context;

        public TODOViewController()
            : base(UITableViewStyle.Plain, null)
        {
            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add), false);
            NavigationItem.RightBarButtonItem.Clicked += (sender, e) =>
            {
                taskDialog = new TaskDialog()
                {
                    Id = Guid.NewGuid().ToString()
                };
                context = new BindingContext(this, taskDialog, "New Task");
                detailsScreen = new DialogViewController(context.Root, true);
                ActivateController(detailsScreen);
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Refresh();
        }

        public override void Selected(NSIndexPath indexPath)
        {
            base.Selected(indexPath);
            var row = indexPath.Row;
            var task = todoLists[row];
            ShowDetails(task);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        private void ShowDetails(Task task)
        {
            currentTask = task;
            taskDialog = new TaskDialog(task);
            context = new BindingContext(this, taskDialog, taskDialog.Name);
            detailsScreen = new DialogViewController(context.Root, true);
            ActivateController(detailsScreen);
        }

        public void SaveTask()
        {
            context.Fetch();
            currentTask = new Task();
            currentTask.Id = taskDialog.Id;
            currentTask.Title = taskDialog.Name;
            currentTask.Description = taskDialog.Description;
            currentTask.Completed = taskDialog.Completed;
            NavigationController.PopViewController(true);
            CognitoSyncUtils utils = new CognitoSyncUtils();
            utils.SaveTask(currentTask);
            Refresh();
            currentTask = null;
        }

        public void DeleteTask()
        {
            if (currentTask != null)
            {
                context.Fetch();
                CognitoSyncUtils utils = new CognitoSyncUtils();
                utils.DeleteTask(taskDialog.Id);
            }
        }

        private void Refresh()
        {
            CognitoSyncUtils utils = new CognitoSyncUtils();
            utils.Synchronize((exception) =>
            {
                if (exception != null)
                {
                    Console.WriteLine("ERROR: " + exception.Message);
                    return;
                }
                todoLists = utils.GetTasks();
                if (todoLists != null && todoLists.Count > 0)
                {
                    InvokeOnMainThread(() =>
                    {
                        Root = new RootElement("Todo List") {
                            new Section() {
                                from t in todoLists
                                select (Element) new CheckboxElement((string.IsNullOrEmpty(t.Title) ? string.Empty : t.Title), t.Completed)
                            }
                        };
                    });
                }
            });
        }

    }

    public class TaskDialog
    {

        public TaskDialog()
        {
        }

        public TaskDialog(Task task)
        {
            Id = task.Id;
            Name = task.Title;
            Description = task.Description;
            Completed = task.Completed;
        }

        [Skip]
        public string Id { get; set; }

        [Entry("task title")]
        public string Name { get; set; }

        [Entry("description")]
        public string Description { get; set; }

        [Entry("Completed")]
        public bool Completed { get; set; }

        [Section("")]
        [OnTap("SaveTask")]
        [Alignment(UITextAlignment.Center)]
        public string Save;

        [OnTap("DeleteTask")]
        [Alignment(UITextAlignment.Center)]
        public string Delete;
    }
}