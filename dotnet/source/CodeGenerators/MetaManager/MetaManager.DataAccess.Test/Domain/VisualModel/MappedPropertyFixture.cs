using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel.Tests
{
    #region IsMapped Members
    [TestClass]
    public class MappedProperty_IsMapped
    {

        [TestMethod]
        public void Vanilla()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            /* method to test*/
            bool result = mp.IsMapped;

            /* evaluation */
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WhithTarget()
        {
            MappedProperty mp = Factory.getVanillaTestObject();
            IMappableProperty newTarget = Factory.getMappedPropertyObject();
            mp.Target = newTarget;

            Assert.IsTrue(mp.IsMapped);
        }

        [TestMethod]
        public void WhithDefaultValue()
        {
            MappedProperty mp = Factory.getVanillaTestObject();

            mp.DefaultValue = "Def Val";

            Assert.IsTrue(mp.IsMapped);
        }

        [TestMethod]
        public void WhithBlankDefaultValue()
        {
            MappedProperty mp = Factory.getVanillaTestObject();

            mp.DefaultValue = "";

            Assert.IsFalse(mp.IsMapped);
        }

        [TestMethod]
        public void WhithDefaultSessionProperty()
        {
            MappedProperty mp = Factory.getVanillaTestObject();

            UXSessionProperty sessionProperty = Factory.getDefaultSessionPropertyObject();
            mp.DefaultSessionProperty = sessionProperty;

            Assert.IsTrue(mp.IsMapped);
        }



    }
    #endregion

    #region Source Members
    [TestClass]
    public class MappedProperty_Source
    {

        [TestMethod]
        public void Vanilla()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            /* method to test*/
            IMappableProperty source = mp.Source;

            /* evaluation */
            Assert.IsNull(source);
        }

        [TestMethod]
        public void SetSourceByMappedProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();
            mp.Source = Factory.getMappedPropertyObject();

            /* method to test*/
            IMappableProperty source = mp.Source;

            /* evaluation */
            Assert.IsNotNull(source);
            Assert.IsTrue(source is MappedProperty);
        }

        [TestMethod]
        public void SetSourceByQueryProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();
            mp.Source = Factory.getQueryPropertyObject();

            /* method to test*/
            IMappableProperty source = mp.Source;

            /* evaluation */
            Assert.IsNotNull(source);
            Assert.IsTrue(source is QueryProperty);
        }

        [TestMethod]
        public void SetSourceByProcedureProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();
            mp.Source = Factory.getProcedurePropertyObject();

            /* method to test*/
            IMappableProperty source = mp.Source;

            /* evaluation */
            Assert.IsNotNull(source);
            Assert.IsTrue(source is ProcedureProperty);
        }

        [TestMethod]
        public void ChangeSourceFromMappedPropertyToProcedureProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            mp.Source = Factory.getMappedPropertyObject();
            mp.Source = Factory.getProcedurePropertyObject();

            /* method to test*/
            IMappableProperty source = mp.Source;

            /* evaluation */
            Assert.IsNotNull(source);
            Assert.IsTrue(source is ProcedureProperty);
        }

        [TestMethod]
        public void ChangeSourceFromMappedPropertyToNull()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            mp.Source = Factory.getMappedPropertyObject();
            mp.Source = null;

            /* method to test*/
            IMappableProperty source = mp.Source;

            /* evaluation */
            Assert.IsNull(source);
        }



    }
    #endregion

    #region Target Members
    [TestClass]
    public class MappedProperty_Target
    {

        [TestMethod]
        public void Vanilla()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            /* method to test*/
            IMappableProperty target = mp.Target;

            /* evaluation */
            Assert.IsNull(target);
        }

        [TestMethod]
        public void SetTargetToMappedProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();
            mp.Target = Factory.getMappedPropertyObject();

            /* method to test*/
            IMappableProperty target = mp.Target;

            /* evaluation */
            Assert.IsNotNull(target);
            Assert.IsTrue(target is MappedProperty);
        }

        [TestMethod]
        public void SetTargetToProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();
            mp.Target = Factory.getPropertyObject();

            /* method to test*/
            IMappableProperty target = mp.Target;

            /* evaluation */
            Assert.IsNotNull(target);
            Assert.IsTrue(target is Property);
        }

        [TestMethod]
        public void ChangeTargetFromMappedPropertyToProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            mp.Target = Factory.getMappedPropertyObject();
            mp.Target = Factory.getPropertyObject();

            /* method to test*/
            IMappableProperty target = mp.Target;

            /* evaluation */
            Assert.IsNotNull(target);
            Assert.IsTrue(target is Property);
        }

        [TestMethod]
        public void ChangeTargetFromPropertyToMappedProperty()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            mp.Target = Factory.getPropertyObject();
            mp.Target = Factory.getMappedPropertyObject();
           

            /* method to test*/
            IMappableProperty target = mp.Target;

            /* evaluation */
            Assert.IsNotNull(target);
            Assert.IsTrue(target is MappedProperty);
        }

        [TestMethod]
        public void ChangeTargetFromMappedPropertyToNull()
        {
            /* setup */
            MappedProperty mp = Factory.getVanillaTestObject();

            mp.Target = Factory.getMappedPropertyObject();
            mp.Target = null;

            /* method to test*/
            IMappableProperty target = mp.Target;

            /* evaluation */
            Assert.IsNull(target);
        }

    }
    #endregion


    #region Factory Members
    public static class Factory
    {
        public static MappedProperty getVanillaTestObject()
        {
            return new MappedProperty();
        }

        public static MappedProperty getMappedPropertyObject()
        {
            return new MappedProperty();
        }

        public static QueryProperty getQueryPropertyObject()
        {
            return new QueryProperty();
        }

        public static ProcedureProperty getProcedurePropertyObject()
        {
            return new ProcedureProperty();
        }

        public static Property getPropertyObject()
        {
            return new Property();
        }



        


        public static UXSessionProperty getDefaultSessionPropertyObject()
        {
            var toReturn = new UXSessionProperty();
            return toReturn;
        }
    }
    #endregion
}