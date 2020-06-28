using System;

namespace Readinizer.Frontend.Messages
{
    public class ChangeView
    {
        public Type ViewModelType { get; }
        public int RefId { get; set; }
        public string Visibility { get; set; }

        public ChangeView(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public ChangeView(Type viewModelType, int refId)
        {
            ViewModelType = viewModelType;
            RefId = refId;
        }

        public ChangeView(Type viewModelType, string visibility)
        {
            ViewModelType = viewModelType;
            Visibility = visibility;
        }
    }
}
