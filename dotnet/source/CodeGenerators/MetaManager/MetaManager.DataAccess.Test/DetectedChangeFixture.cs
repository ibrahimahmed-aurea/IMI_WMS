using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.DataAccess.Tests
{
    [TestClass]
    public class DetectedChangeFixture
    {
        [TestMethod]
        public void Vanilla()
        {
            /* setup */
            DetectedChange dc = Factory.getVanillaTestObject();

            /* method to test*/
            DataModelChangeType dmct = dc.ChangeType;
            List<string> list = dc.Changes;

            /* evaluation */
            Assert.IsTrue(dmct.Equals(DataModelChangeType.New));
            Assert.IsNull(list);

        }

        [TestMethod]
        public void ChangeChangeType()
        {
            /* setup */
            DetectedChange dc = Factory.getVanillaTestObject();
            DataModelChangeType dmctIn = DataModelChangeType.ModifiedHint;

            /* method to test*/
            dc.ChangeType = dmctIn;

            DataModelChangeType dmctOut = dc.ChangeType;

            /* evaluation */
            Assert.IsFalse(dmctOut == DataModelChangeType.New);
            Assert.IsTrue(dmctOut == dmctIn);
        }

        [TestMethod]
        public void AddEmptyText()
        {
            /* setup */
            DetectedChange dc = Factory.getVanillaTestObject();
            string txtEmpty = "";

            /* method to test*/
            dc.AddText(txtEmpty);
            List<string> list = dc.Changes;

            /* evaluation */
            Assert.IsNull(list);
        }

        [TestMethod]
        public void AddText()
        {
            /* setup */
            DetectedChange dc = Factory.getVanillaTestObject();
            string txt = "Some text to test";
            
            /* method to test*/
            dc.AddText(txt);
            List<string> list = dc.Changes;

            /* evaluation */
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);
            string test = dc.ToString();
            Assert.IsTrue(test.Equals( txt));
        }

        [TestMethod]
        public void AddTwoTexts()
        {
            /* setup */
            DetectedChange dc = Factory.getVanillaTestObject();
            string txt = "Some text to test";
            string txt2 = "Some more text";
            string expected = txt + "\r\n" + txt2;

            /* method to test*/
            dc.AddText(txt);
            dc.AddText(txt2);
            List<string> list = dc.Changes;

            /* evaluation */
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 2);
            string test = dc.ToString();
            Assert.IsTrue(test.Equals(expected));
        }

    }

        #region Factory Members
    public static class Factory
    {        
    public static DetectedChange getVanillaTestObject()
        {
            return new DetectedChange();
        }
        
            }
    #endregion

}
