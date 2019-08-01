public void alignWallToCurvedGrid()
// https://boostyourbim.wordpress.com/2018/08/09/biltna2018-wish-granted-align-a-straight-wall-with-a-curved-grid/
{
    UIDocument uidoc = this.ActiveUIDocument;
    Document doc = this.ActiveUIDocument.Document;
    Wall wall = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, "Select wall")) as Wall;
    Grid grid = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, "Select grid")) as Grid;
    Curve gridCurve = grid.Curve;
    Line wallLine = (((LocationCurve)wall.Location).Curve as Line);
    XYZ wallDirection = wallLine.Direction;
    XYZ wallPerpendicular = XYZ.BasisZ.CrossProduct(wallDirection);
    
    XYZ intersection0 = intersect(wallLine.GetEndPoint(0), wallPerpendicular, gridCurve);
    XYZ intersection1 = intersect(wallLine.GetEndPoint(1), wallPerpendicular, gridCurve);
    XYZ intersectionMid = intersect(wallLine.Evaluate(0.5, true), wallPerpendicular, gridCurve);
    
    Arc arc = Arc.Create(intersection0, intersection1, intersectionMid);
    
    List wallChildren = new FilteredElementCollector(doc)
             .OfClass(typeof(FamilyInstance))
             .Cast()
        .Where(q => q.Host.Id == wall.Id).ToList();
    
    using (Transaction t = new Transaction(doc, "Wall to Curve"))
    {
        t.Start();
        Wall newWall = Wall.Create(doc, arc, wall.LevelId, false);
        newWall.WallType = doc.GetElement(wall.GetTypeId()) as WallType;
        foreach (FamilyInstance fi in wallChildren)
        {
            XYZ fiPoint = (fi.Location as LocationPoint).Point;
            XYZ fiIntersection = intersect(fiPoint, wallPerpendicular, gridCurve);
            if (fiIntersection != null)
            {
                FamilyInstance fiNew = doc.Create.NewFamilyInstance(fiIntersection, 
                fi.Symbol,
                newWall,
                doc.GetElement(fi.LevelId) as Level,
                StructuralType.NonStructural);
                
                if (fi.FacingFlipped)
                    fiNew.flipFacing();
                if (fi.HandFlipped)
                    fiNew.flipHand();
            }
        }
        doc.Delete(wall.Id);
        t.Commit();
    }
}

private XYZ intersect(XYZ point, XYZ direction, Curve curve)
{
    Line unbound = Line.CreateUnbound(new XYZ(point.X, point.Y, curve.GetEndPoint(0).Z), direction);
    IntersectionResultArray ira = null;
    unbound.Intersect(curve, out ira);
    if (ira == null)
    {
        TaskDialog td = new TaskDialog("Error");
        td.MainInstruction = "no intersection";
        td.MainContent = point.ToString() + Environment.NewLine + direction.ToString();
        td.Show();
            
        return null;
    }
    IntersectionResult ir = ira.Cast().First();
    return ir.XYZPoint;
}
