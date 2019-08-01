public void registerWarningSuppresion()
// https://boostyourbim.wordpress.com/2018/08/08/biltna2018-wish-granted-remove-warning-when-modifying-type-parameter-values-in-schedule/
{
    Document doc = this.ActiveUIDocument.Document;   
    Application app = doc.Application;
    UIApplication uiapp = new UIApplication(app);
    uiapp.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(dismissTaskDialog);
}

private void dismissTaskDialog(object sender, DialogBoxShowingEventArgs args)
{
    TaskDialogShowingEventArgs e = args as TaskDialogShowingEventArgs;
    if (e == null)
        return;
    if (e.Message.StartsWith("This change will be applied to all elements of type"))
    // if (e.Message.StartsWith("Данное изменение будет применено ко всем элементам следующего типа"))
    {
        e.OverrideResult((int)TaskDialogCommonButtons.Ok);
    }
}
