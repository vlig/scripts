[AutoLISP: Make and Save Custom Hatch Pattern](https://autocadtips1.com/2011/10/11/autolisp-make-and-save-custom-hatch-pattern/)

This Routine has been featured on Cadalyst’s website and is very handy. If you have ever wondered how to make a custom hatch pattern and then looked at the coding that is required, you will really appreciate this routine. The only drawback that I have come across is that it will only accept line and point entities… This can be over come by drawing your curved objects and then using other LISP routines to convert arcs and circles to line entities. Or use the “SEGS” LISP routine to convert the curved objects to polylines and then explode the polyline. This will turn the polyline segments into line segments.
There are 2 commands:
1. DRAWHATCH <enter> creates a 1X1 square in which you draw your custom hatch pattern
After you have drawn your hatch pattern:
2. SAVEHATCH <enter> lets you name your hatch Pattern and save it as a .pat file.
Saving the .pat file (hatch pattern) allows you to copy the contents and save it in your support file or even add it to your acad.pat file where all of your default patterns are stored.
