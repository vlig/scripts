public void lineLength()
{
    double length = 0;
    Document doc = this.ActiveUIDocument.Document;
    UIDocument uidoc = this.ActiveUIDocument;
    ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
    foreach (ElementId id in ids)
    {
        Element e = doc.GetElement(id);
        Parameter lengthParam = e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
        if (lengthParam == null)
            continue;
        length += lengthParam.AsDouble();
    }
    string lengthWithUnits = UnitFormatUtils.Format(doc.GetUnits(), UnitType.UT_Length, length, false, false);
    TaskDialog.Show("Length", ids.Count + " elements = " + lengthWithUnits);
}
