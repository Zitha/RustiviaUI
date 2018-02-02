using System;
using System.IO;
using System.Web.Mvc;

namespace IntroductionMVC5.Web.Utils.Printing
{
    public class FakeView : IView
    {
        #region IView Members

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}