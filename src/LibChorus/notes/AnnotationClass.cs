﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace Chorus.notes
{
    public class AnnotationClass
    {
        public AnnotationClass(string englishName)
        {
            NameInEnglish = englishName;
        }
    
        public string NameInEnglish{get;set;}

        virtual public Image GetImage(int pixelsOfSquare)
        {
            if (pixelsOfSquare <= 16)
                return Chorus.Properties.AnnotationImages.generic16x16;
            else
                return Chorus.Properties.AnnotationImages.generic32x32;
        }

        public virtual bool UserCanResolve { get { return false; } }

        public virtual string GetLongLabel(string labelOfThingAnnotated)
        {
            return String.Format("{0} on {1}", NameInEnglish, labelOfThingAnnotated);
        }
    }

    public class QuestionAnnotationClass:AnnotationClass
    {
        public QuestionAnnotationClass():base("Question")
        {           
        } 
 
        public override Image GetImage(int pixelsOfSquare)
        {
            if(pixelsOfSquare <= 16)
                return Chorus.Properties.AnnotationImages.question16x16;
            else
                return Chorus.Properties.AnnotationImages.question32x32;
        }

        public override string GetLongLabel(string labelOfThingAnnotated)
        {
            return String.Format("Question about {1}", NameInEnglish, labelOfThingAnnotated);
        }
        public override bool UserCanResolve { get { return true; } }
    }


    public class NoteAnnotationClass : AnnotationClass
    {
        public NoteAnnotationClass()
            : base("Note")
        {
        }

        public override Image GetImage(int pixelsOfSquare)
        {
            if (pixelsOfSquare <= 16)
                return Chorus.Properties.AnnotationImages.note16x16;
            else
                return Chorus.Properties.AnnotationImages.note32x32;
        }
    }


    public class ConflictAnnotationClass : AnnotationClass
    {
        public ConflictAnnotationClass()
            : base("Merge Conflict")
        {
        }

        public override Image GetImage(int pixelsOfSquare)
        {
            if (pixelsOfSquare <= 16)
                return Chorus.Properties.AnnotationImages.MergeConflict16x16;
            else
                return Chorus.Properties.AnnotationImages.MergeConflict32x32;
        }

        public override string GetLongLabel(string labelOfThingAnnotated)
        {
            return String.Format("Merge Conflict on {1}", NameInEnglish, labelOfThingAnnotated);
        }

        public override bool UserCanResolve { get { return true; } }
    }
}