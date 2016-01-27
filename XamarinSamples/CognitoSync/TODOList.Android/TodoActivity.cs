using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TODOListPortableLibrary;
using Android.Util;

namespace TODOList.Android
{
    [Activity(Label = "Todo")]
    public class TodoActivity : Activity
    {

        string TAG = "TodoActivity";

        ListView todoListView;
        Button addTodoItenButton;
        List<Task> tasks;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.todo_layout);
            todoListView = FindViewById<ListView>(Resource.Id.lstTasks);
            addTodoItenButton = FindViewById<Button>(Resource.Id.btnAddTask);

            todoListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var todo = new Intent(this, typeof(TodoDetailActivity));
                todo.PutExtra("todoItem", tasks[e.Position].Id);
                StartActivity(todo);
            };

            addTodoItenButton.Click += (object sender, EventArgs e) =>
            {
                var todo = new Intent(this, typeof(TodoDetailActivity));
                todo.PutExtra("todoItem", Guid.NewGuid().ToString());
                StartActivity(todo);
            };

            Synchronize();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Synchronize();
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflator = this.MenuInflater;
            inflator.Inflate(Resource.Menu.home_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle item selection
            switch (item.ItemId)
            {
                case Resource.Id.synchronize:
                    Synchronize();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public void Synchronize()
        {
            CognitoSyncUtils utils = new CognitoSyncUtils();
            utils.Synchronize((exception) =>
            {
                if (exception != null)
                {
                    Log.Error(TAG, exception.Message);
                }
                else
                {
                    tasks = utils.GetTasks();
                    RunOnUiThread(() =>
                    {
                        todoListView.Adapter = new TodoListAdapter(this, tasks);
                    });
                }
            });
        }


    }

    class TodoListAdapter : BaseAdapter<Task>
    {

        private Activity context;
        private List<Task> tasks;

        public TodoListAdapter(Activity context, List<Task> tasks)
        {
            this.context = context;
            this.tasks = tasks;
        }

        public override Task this[int position]
        {
            get { return tasks[position]; }
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return tasks.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = tasks[position];

            var view = (convertView ??
                    context.LayoutInflater.Inflate(
                    global::Android.Resource.Layout.SimpleListItemChecked,
                    parent,
                    false)) as CheckedTextView;

            view.SetText(item.Title == "" ? "<new task>" : item.Title, TextView.BufferType.Normal);
            view.Checked = item.Completed;
            view.Tag = item.Id;

            //Finally return the view
            return view;
        }
    }

}