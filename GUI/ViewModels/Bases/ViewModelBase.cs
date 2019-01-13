using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace ProjectManager.ViewModels.Bases
{
    public abstract class ViewModelBase : BindableBase
    {
        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        protected ViewModelBase()
        {
            NotificationRequest = new InteractionRequest<INotification>();

            RegisterCommands();
        }

        protected virtual void RegisterCommands()
        {

        }

        protected void ShowError(string title, string mesage)
        {
            var notification = new Notification
            {
                Content = mesage,
                Title = title
            };

            Action<INotification> lambda = _ =>
            {
            };

            NotificationRequest.Raise(notification, lambda);

        }
    }
}
