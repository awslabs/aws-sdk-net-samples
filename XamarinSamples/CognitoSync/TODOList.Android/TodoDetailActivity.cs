using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using TODOListPortableLibrary;
using Android.Text;

namespace TODOList.Android
{
    [Activity(Label = "TodoDetail")]
    public class TodoDetailActivity : Activity
    {

        Button saveButton;
        Button deleteButton;
        EditText titleText;
        EditText descriptionText;
        CheckBox taskCompleted;
        string todoId;
        Task todoTask;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.todo_detail_layout);

            saveButton = FindViewById<Button>(Resource.Id.btnSave);
            deleteButton = FindViewById<Button>(Resource.Id.btnDelete);

            titleText = FindViewById<EditText>(Resource.Id.title);
            descriptionText = FindViewById<EditText>(Resource.Id.description);
            taskCompleted = FindViewById<CheckBox>(Resource.Id.chkDone);

            saveButton.Click += SaveButtonOnClick;
            deleteButton.Click += DeleteButtonOnClick;

            titleText.TextChanged += (object sender, TextChangedEventArgs e)=>
            {
                saveButton.Enabled = true;
                deleteButton.Enabled = true;
            };
        }

        private void DeleteButtonOnClick(object sender, EventArgs e)
        {
            CognitoSyncUtils utils = new CognitoSyncUtils();
            todoId = Intent.GetStringExtra("todoItem");
            utils.DeleteTask(todoId);
            Finish();
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            var task = new Task()
            {
                Id = todoId,
                Description = descriptionText.Text,
                Title = titleText.Text,
                Completed = taskCompleted.Checked
            };
            CognitoSyncUtils utils = new CognitoSyncUtils();
            utils.SaveTask(task);
            Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();

            todoId = Intent.GetStringExtra("todoItem");

            if (!string.IsNullOrEmpty(todoId))
            {
                CognitoSyncUtils utils = new CognitoSyncUtils();
                todoTask = utils.GetTask(todoId);
                if (todoTask != null)
                {
                    titleText.SetText(todoTask.Title, TextView.BufferType.Editable);
                    descriptionText.SetText(todoTask.Description, TextView.BufferType.Editable);
                    saveButton.Enabled = true;
                    deleteButton.Enabled = true;
                    taskCompleted.Checked = todoTask.Completed;
                }
            }
        }

    }
}