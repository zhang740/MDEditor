using MDEditor.Properties;
using Microsoft.ClearScript.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MDEditor
{
    class MarkdownProcessor
    {
        private JScriptEngine Engine;
        private static string TemplatePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\";

        public MarkdownProcessor(bool autoInit = true)
        {
            if (!autoInit) return;
            var markedjs = "";
            var config = "";
            if (Directory.Exists(TemplatePath))
            {
                if (File.Exists(TemplatePath + "Marked.js"))
                {
                    markedjs = File.ReadAllText(TemplatePath + "Marked.js");
                }
                if (File.Exists(TemplatePath + "Config.json"))
                {
                    config = File.ReadAllText(TemplatePath + "Config.json");
                }
            }
            Init(markedjs, config);
        }

        public static void ExportTemplate()
        {
            Directory.CreateDirectory(TemplatePath);
            File.WriteAllText(TemplatePath + "Marked.js", Resources.marked);
            File.WriteAllText(TemplatePath + "Config.json", Encoding.Default.GetString(Resources.config));
            File.WriteAllText(TemplatePath + "Template.html", Resources.template);
        }

        public void Init(string markedjs, string config)
        {
            Engine = new JScriptEngine();
            if (string.IsNullOrWhiteSpace(markedjs))
            {
                markedjs = Resources.marked;
            }
            Engine.Execute(markedjs);

            if (string.IsNullOrWhiteSpace(config))
            {
                config = Encoding.Default.GetString(Resources.config);
            }
            Engine.Execute("marked.setOptions(" + config + ");");
        }

        public string ConvertMarkdownToHTML(string markdown, bool useTemplate = true)
        {
            if (Engine == null) return "处理器尚未初始化";
            try
            {
                var tmp = (string)((dynamic)Engine.Script).marked(markdown);
                return useTemplate ? UseTemplate(tmp) : tmp;
            }
            catch (Exception ex)
            {
                return "Markdown生成出错。" + ex.Message;
            }
        }

        public string UseTemplate(string html)
        {
            var template = File.Exists(TemplatePath + "Template.html") ? File.ReadAllText(TemplatePath + "Template.html") : Resources.template;
            return template.Replace("@HTMLPlaceHolder", html);
        }
    }
}
