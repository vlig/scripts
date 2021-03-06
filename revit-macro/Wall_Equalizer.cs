/*
 * https://habr.com/ru/post/352948/
 * Created by SharpDevelop.
 * User: Alexandr Akunets
 * Date: 1/30/2018
 * Time: 9:50 AM
 * 
 * version 1.000
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Wall_Equalizer
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("31DDFC5C-59FF-416B-8482-49568A640EE5")]   

    public partial class ThisApplication
    {
        private void Module_Startup(object sender, EventArgs e)
        {

        }

        private void Module_Shutdown(object sender, EventArgs e)
        {

        }    

        public void WallEqualizer ()
        {

            var uiapp = this.ActiveUIDocument;

            var doc = uiapp.Document;

            //Dictionary<Element, Sherif> elms = new Dictionary<Element, Sherif>();

            //var wallist = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_Walls).ToElements();

            List<Element> wallist = new List<Element>();

            var rs = uiapp.Selection.PickObjects (ObjectType.Element, new PickFilter(BuiltInCategory.OST_Walls), "Select wrong walls");

            foreach(var w in rs)
            {
                var e = doc.GetElement(w.ElementId) as Wall;                
                    wallist.Add(e);
            }

            var r = uiapp.Selection.PickObject (ObjectType.Element, new PickFilter(BuiltInCategory.OST_Walls), "Select wall - sample");

            Wall selwall = doc.GetElement(r.ElementId) as Wall;

            Line line = GetLine(selwall);

               try
               {               
                foreach(Element el in wallist)
                {
                    using (Transaction t = new Transaction(doc, "r"))
                    {
                       t.Start();

                       var wLine = GetLine(el);        

                       if(wLine != null)
                       {
                            var wcos = (line.Direction.X * wLine.Direction.X + line.Direction.Y * wLine.Direction.Y);

                            var angleR = Math.Acos (wcos);

                            if (el.Id.IntegerValue != r.ElementId.IntegerValue)
                            {
                                BustEquiizer90(el, line, angleR );

                                BustEquiizer0(el, line, angleR );
                            }
                       }
                        t.Commit();
                    }
                }
               }  

               catch(Exception ex)
               {
                   if ( (ex is Autodesk.Revit.Exceptions.ArgumentsInconsistentException) == false)
                   {
                       TaskDialog.Show("Error", ex.Message + ex.StackTrace);
                   }
               }

        }

        private void BustEquiizer90(Element el, Line line, double angleR)
        {            
            var angle = RadiansToDegrees( angleR );

               if ( Math.Abs(angle - 90) > 0.0000001 && Math.Abs(angle - 90) < 1.2)
               {
                   Equalizer(el, -(DegreesToRadians(90) - angleR));

                   var wLine = GetLine(el);                    

                var wcos = (line.Direction.X * wLine.Direction.X + line.Direction.Y * wLine.Direction.Y);

                angleR = Math.Acos (wcos);

                angle = RadiansToDegrees( angleR );

                   if ( Math.Abs(angle - 90) > 0.0000001 && Math.Abs(angle - 90) < 2.4)
                   {
                       Equalizer(el, (DegreesToRadians(90) - angleR));
                   }
               }
        }

        private void BustEquiizer0(Element el, Line line, double angleR)
        {
            var angle = RadiansToDegrees( angleR );

            if ( (Math.Abs(angle) > 0.0000001 && Math.Abs(angle) < 1.2)  ||
                   Math.Abs(angle -180) > 0.0000001 && Math.Abs(angle -180) < 2.4)
               {
                   Equalizer(el, -angleR);

                   var wLine = GetLine(el);                    

                var wcos = (line.Direction.X * wLine.Direction.X + line.Direction.Y * wLine.Direction.Y);

                angleR = Math.Acos (wcos);

                angle = RadiansToDegrees( angleR );

                   if ( Math.Abs(angle) > 0.0000001 && Math.Abs(angle) < 2.4  ||
                   Math.Abs(angle -180) > 0.0000001 && Math.Abs(angle -180) < 2.4)
                   {
                       Equalizer(el, -angleR);                       

                       wLine = GetLine(el);                    

                    wcos = (line.Direction.X * wLine.Direction.X + line.Direction.Y * wLine.Direction.Y);

                    angleR = Math.Acos (wcos);

                    angle = RadiansToDegrees( angleR );

                       if ( Math.Abs(angle) > 0.0000001 && Math.Abs(angle) < 5  ||
                       Math.Abs(angle -180) > 0.0000001 && Math.Abs(angle -180) < 5)
                       {
                           Equalizer(el, angleR);
                       }
                   }

               }
        }

        private void Equalizer(Element el, double angle)
        {            
            LocationCurve wLine = el.Location as LocationCurve;

            var mid = Midpoint(wLine.Curve);

            Line axis = Line.CreateBound(mid, new XYZ( mid.X, mid.Y, mid.Z + 1));

            ElementTransformUtils.RotateElement( this.ActiveUIDocument.Document, el.Id, axis, angle);    
        }    

        private Line GetLine(Element el){

            var selCurve = (el.Location as LocationCurve).Curve;

            if( selCurve is Line)
            {
                XYZ p0 = new XYZ(selCurve.GetEndPoint(0).X, selCurve.GetEndPoint(0).Y, 0);

                XYZ p1 = new XYZ(selCurve.GetEndPoint(1).X, selCurve.GetEndPoint(1).Y, 0);            

                Line line = Line.CreateBound (p0, p1);

                return line;
            }
            else return null;
        }

        public static double RadiansToDegrees(double angle)
        {
            return (angle * 180 / Math.PI);
        }

        public static double DegreesToRadians(double angle)
        {
            return (angle * Math.PI / 180);
        }

        public static XYZ Midpoint(Curve curve)
        {

            return 0.5 * (curve.GetEndPoint(0) + curve.GetEndPoint(1));
        }

        #region Revit Macros generated code
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Module_Startup);
            this.Shutdown += new System.EventHandler(Module_Shutdown);
        }
        #endregion
    }

    public class PickFilter : ISelectionFilter
    {
        //BuiltInCategory SelectionCategory { get; set; }

        BuiltInCategory [] SelCats;

        /*
        public PickFilter(BuiltInCategory cat)
        {
            SelCats = new BuiltInCategory[] { cat };
        }
        */

        public PickFilter(params BuiltInCategory[] cat)
        {
            SelCats = cat;
        }

        public bool AllowElement (Element elem)
        {
            foreach(BuiltInCategory cat in SelCats)
            {
                if (elem.Category.Id.IntegerValue == (int)cat)
                    return true;
            }

            return false;
        }
        public bool AllowReference (Reference r, XYZ p)
        {
            return false;
        }
    }
}
