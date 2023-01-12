using System;
using System.Collections;
using DevExpress.Blazor.RichEdit;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office.Blazor.Components.Models;
using DevExpress.ExpressApp.Office.Blazor.Editors;
using DevExpress.ExpressApp.Office.Blazor.Editors.Adapters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

//...
public class BlazorRichEditController : ViewController<DetailView>
{
     
    public BlazorRichEditController()
    {
        SimpleAction _action = new SimpleAction(this,"New Id", DevExpress.Persistent.Base.PredefinedCategory.View);
        _action.Caption = "Process Mail Merge";
        _action.Execute+= ActionOnExecute;
    }

    private void ActionOnExecute(object sender, SimpleActionExecuteEventArgs e)
    {
        this.CurrentControl.ComponentModel.MailMergeSettings = builder => {
            builder.OpenComponent<DxMailMergeSettings>(1);
            builder.AddAttribute(2, "Data", ProvideMailMergeData());
            builder.CloseComponent();
        };
    }

    public IEnumerable ProvideMailMergeData()
    {
        //TODO here you have to return the IEnumerable

       return  Enumerable.Empty<object>();
    }
    protected override void OnActivated()
    {
        base.OnActivated();
        foreach (RichTextPropertyEditor editor in View.GetItems<RichTextPropertyEditor>())
        {
            if (editor.Control != null)
            {
                CustomizeRichEditComponentModel(((RichTextEditorComponentAdapter)editor.Control).ComponentModel);

            }
            else
            {
                editor.ControlCreated += Editor_ControlCreated;
            }
        }
    }
    RichTextEditorComponentAdapter CurrentControl;
    private void Editor_ControlCreated(object sender, EventArgs e)
    {
        
        CurrentControl = ((RichTextEditorComponentAdapter)((RichTextPropertyEditor)sender).Control);
        //Mail Merge
        //https://supportcenter.devexpress.com/ticket/details/t1112682/how-to-setup-mail-merge-datasource-in-blazor-rich-text-editor
        CustomizeRichEditComponentModel(CurrentControl.ComponentModel);
    }
    private void CustomizeRichEditComponentModel(DxRichEditModel richEditModel)
    {
     
        
        richEditModel.ViewType = DevExpress.Blazor.RichEdit.ViewType.Simple;
    }
    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        foreach (RichTextPropertyEditor editor in View.GetItems<RichTextPropertyEditor>())
        {
            editor.ControlCreated -= Editor_ControlCreated;
        }
    }
}