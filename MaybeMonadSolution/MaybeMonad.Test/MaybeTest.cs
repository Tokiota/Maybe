using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaybeMonad.Test
{

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MaybeTest
    {
        Cliente cliente;

        private const string DefaultNombre = "Sin Nombre";

        [TestInitialize]
        public void Antes_de_cada_prueba()
        {
        }

        [TestMethod]
        public void Cuando_usa_tipo_por_referencia()
        {
            var result = cliente.ToMaybe();

            Assert.IsFalse(result.HasValue);
            
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void Cuando_usa_tipo_por_valor()
        {
            var result = 5.ToMaybe();

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(result.Value,5);
                
        }
        [TestMethod]
        public void Cuando_la_raiz_es_nula_y_se_pide_por_una_propiedad()
        {
            var result = cliente.ToMaybe()
                .Select(c => c.Direccion);

            Assert.IsTrue(result.GetType() == typeof(Maybe<Direccion>));
            Assert.IsFalse(result.HasValue);
            Assert.IsNull(result.Value);

        }

        [TestMethod]
        public void Cuando_la_raiz_es_nula_y_se_pide_por_una_propiedad_con_valor_predeterminado()
        {
            
            var result = cliente.ToMaybe()
                .SelectOrDefault(c => c.Nombre, () => DefaultNombre);

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(DefaultNombre,result.Value);
        }
        
        [TestMethod]
        public void Cuando_la_raiz_no_es_nula_y_se_pide_por_un_valor_existente()
        {
            cliente = new Cliente
                {
                    Nombre = "Daniel"
                };

            var result = cliente.ToMaybe()
                .Select(c => c.Nombre);

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(result.Value,"Daniel");
        }

        [TestMethod]
        public void Cuando_la_raiz_no_es_nula_y_se_pide_por_un_valor_existente_O_default()
        {
            cliente = new Cliente
            {
                Nombre = "Daniel"
            };

            var result = cliente.ToMaybe()
                .SelectOrDefault(c => c.Nombre,()=>DefaultNombre);

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(result.Value, "Daniel");
        }

        [TestMethod]
        public void Igualando_Dos_Maybe()
        {
            var first = Maybe<Cliente>.Nothing;
            var second = Maybe < Cliente >.Nothing;

            Assert.IsTrue(first == second);
        }
        [TestMethod]
        public void Cuando_la_raiz_no_es_nula_y_se_pide_un_valor_de_salida()
        {
            cliente = new Cliente
            {
                Nombre = "Daniel"
            };

            var result = cliente.ToMaybe()
                .Return(c => c.Nombre, () => "DefaultNombre");
                

            Assert.AreEqual(result, "Daniel");
        }

        [TestMethod]
        public void Cuando_la_raiz_no_es_nula_y_hay_una_condicion_que_sale_por_si()
        {
            cliente = new Cliente
                {
                    Nombre = "Daniel",
                    Direccion = new Direccion
                        {
                            CodigoPostal = "08401",
                            CodigoPais = "ES"
                        }
                };

            bool seEjecuto = false;
            var result = cliente.ToMaybe()
                .Select(c => c.Direccion)
                .If(d =>
                {
                    seEjecuto = true;
                    return EsCodigoPaisEspaña(d);
                })
                .SelectOrDefault(d => d.CodigoPostal,()=> "None");

            Assert.AreEqual("08401",result.Value);
            Assert.IsTrue(seEjecuto);

        }

        [TestMethod]
        public void Cuando_la_raiz_no_es_nula_y_hay_una_condicion_que_sale_por_no()
        {
            cliente = new Cliente
            {
                Nombre = "Daniel",
                Direccion = new Direccion
                {
                    CodigoPostal = "08401",
                    CodigoPais = "PT"
                }
            };

            bool seEjecuto = false;
            var result = cliente.ToMaybe()
                .Select(c => c.Direccion)
                .If(d =>
                {
                    seEjecuto = true;
                    return EsCodigoPaisEspaña(d);
                })
                .SelectOrDefault(d => d.CodigoPostal, () => "None");

            Assert.AreEqual("None", result.Value);
            Assert.IsTrue(seEjecuto);
        }

        [TestMethod]
        public void Cuando_la_raiz_es_nula_y_hay_una_condicion_sale_por_no()
        {
            cliente = new Cliente
            {
                Nombre = "Daniel",
            };
            bool seEjecuto = false;
            var result = cliente.ToMaybe()
                .Select(c => c.Direccion)
                .If(d=>
                    {
                        seEjecuto = true;
                        return EsCodigoPaisEspaña(d);
                    })
                .SelectOrDefault(d => d.CodigoPostal, () => "None");

            Assert.AreEqual("None", result.Value);
            Assert.IsFalse(seEjecuto);

        }
        
        [TestMethod]
        public void Cuando_la_raiz_no_es_nula_y_executa_una_accion()
        {
            cliente = new Cliente
            {
                Nombre = "Daniel",
                Direccion = new Direccion
                {
                    CodigoPostal = "08401",
                    CodigoPais = "PT"
                }
            };

            bool seEjecuto = false;


            cliente.ToMaybe()
                .Execute(c =>
                    {
                        seEjecuto = true;
                    });

            Assert.IsTrue(seEjecuto);
        }

        [TestMethod]
        public void Cuando_la_raiz_es_nula_y_executa_una_accion()
        {

            bool seEjecuto = false;


            cliente.ToMaybe()
                .Execute(c =>
                {
                    seEjecuto = true;
                });

            Assert.IsFalse(seEjecuto);
        }

        
        private bool EsCodigoPaisEspaña(Direccion direccion)
        {
            return direccion.CodigoPais == "ES";
        }
    }
}
