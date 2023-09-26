using System;
using Newtonsoft.Json;

namespace Example
{
    public class TranslateText
    {
        public string key { get; }

        public string text { get; private set; }

        public TranslateText(string key)
        {
            this.key = key;
            text     = key;

            LocalizeComponent.on_language_change += Translate;

            Translate();
        }

        public void Translate()
        {
            if(LocalizeComponent.Instance is null)
            {
                return;
            }

            text = LocalizeComponent.Instance.GetText(this);
        }
    }
}