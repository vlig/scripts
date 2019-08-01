public void selectAllLineStyle()
{
    Document doc = this.ActiveUIDocument.Document;
    UIDocument uidoc = this.ActiveUIDocument;
    DetailLine line = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, "Select detail line")) as DetailLine;
    
    // if you want only the lines in the current view, add "doc.ActiveView.Id" to the FilteredElementCollector
    List lines = new FilteredElementCollector(doc)
        .OfClass(typeof(CurveElement))
        .OfCategory(BuiltInCategory.OST_Lines)
        .Where(q => q is DetailLine)
        .Cast()
        .Where(q => q.LineStyle.Id == line.LineStyle.Id)
        .ToList();
    uidoc.Selection.SetElementIds(lines.Select(q => q.Id).ToList());
}
