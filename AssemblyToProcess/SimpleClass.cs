using JetBrains.Annotations;
// ReSharper disable All
#pragma warning disable 67, 169, 649, 1591

[assembly: PublicAPI]
[module: PublicAPI]

namespace AssemblyToProcess
{
    using System;

    /// <summary>
    /// This is a simple class
    /// </summary>
    [UsedImplicitly]
    public class SimpleClass
    {
        /// <summary>
        /// The private field
        /// </summary>
        [PublicAPI]
        int privateField;
        /// <summary>
        /// The public field
        /// </summary>
        [NotNull]
        public object publicField;
        /// <summary>
        /// The protected field
        /// </summary>
        [NotNull]
        protected object protectedField;

        /// <summary>
        /// Gets or sets the public property.
        /// </summary>
        /// <value>
        /// The public property.
        /// </value>
        [PublicAPI]
        public int PublicProperty { get; set; }
        /// <summary>
        /// Gets or sets the private property.
        /// </summary>
        /// <value>
        /// The private property.
        /// </value>
        [PublicAPI]
        private int PrivateProperty { get; set; }
        /// <summary>
        /// Gets or sets the protected property.
        /// </summary>
        /// <value>
        /// The protected property.
        /// </value>
        [PublicAPI]
        protected int ProtectedProperty { get; set; }
        /// <summary>
        /// Gets or sets the internal property.
        /// </summary>
        /// <value>
        /// The internal property.
        /// </value>
        [PublicAPI]
        internal int InternalProperty { get; set; }


        /// <summary>
        /// Generics the method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [PublicAPI]
        [return: PublicAPI]
        public void GenericMethod<[PublicAPI]T>()
        {
        }

        /// <summary>
        /// Occurs when [public event].
        /// </summary>
        [CanBeNull]
        public event EventHandler PublicEvent;
        /// <summary>
        /// Occurs when [private event].
        /// </summary>
        [CanBeNull]
        private event EventHandler PrivateEvent;
        /// <summary>
        /// Occurs when [protected event].
        /// </summary>
        [CanBeNull]
        protected event EventHandler ProtectedEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleClass"/> class.
        /// </summary>
        [PublicAPI]
        public SimpleClass() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleClass"/> class.
        /// </summary>
        /// <param name="privateConstructorArgs">The private constructor arguments.</param>
        [PublicAPI]
        private SimpleClass([PublicAPI] int privateConstructorArgs) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleClass"/> class.
        /// </summary>
        /// <param name="protectedConstructorArgs">The protected constructor arguments.</param>
        [PublicAPI]
        protected SimpleClass([PublicAPI] double protectedConstructorArgs) { }

        /// <summary>
        /// Nots the null method.
        /// </summary>
        /// <param name="notNull">The not null.</param>
        /// <param name="canBeNull">The can be null.</param>
        /// <param name="notNullAndItemNotNull">The not null and item not null.</param>
        /// <returns></returns>
        [NotNull]
        public string NotNullMethod([NotNull][System.Diagnostics.CodeAnalysis.NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
        {
            return "";
        }

        /// <summary>
        /// Strings the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        [NotNull]
        [StringFormatMethod("format")]
        public string StringFormat([NotNull] string format, [NotNull] params object[] args)
        {
            return "";
        }

        /// <summary>
        /// Contracts the method.
        /// </summary>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <returns></returns>
        [ContractAnnotation("param1:null => null")]
        public string ContractMethod(string param1, string param2)
        {
            return "";
        }

        /// <summary>
        /// Methods the without any attribute.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        public string MethodWithoutAnyAttribute(string arg)
        {
            return "";
        }

        /// <summary>
        /// Internals the method.
        /// </summary>
        /// <param name="notNull">The not null.</param>
        /// <param name="canBeNull">The can be null.</param>
        /// <param name="notNullAndItemNotNull">The not null and item not null.</param>
        /// <returns></returns>
        [NotNull]
        internal string InternalMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
        {
            return "";
        }

        /// <summary>
        /// Privates the method.
        /// </summary>
        /// <param name="notNull">The not null.</param>
        /// <param name="canBeNull">The can be null.</param>
        /// <param name="notNullAndItemNotNull">The not null and item not null.</param>
        /// <returns></returns>
        [NotNull]
        private string PrivateMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
        {
            return "";
        }

        /// <summary>
        /// A nested class
        /// </summary>
        [UsedImplicitly]
        public class NestedClass
        {
            [PublicAPI]
            int privateField;
            [NotNull]
            public object publicField;
            [NotNull]
            protected object protectedField;

            /// <summary>
            /// Gets or sets the public property.
            /// </summary>
            /// <value>
            /// The public property.
            /// </value>
            [PublicAPI]
            public int PublicProperty { get; set; }
            [PublicAPI]
            private int PrivateProperty { get; set; }
            [PublicAPI]
            protected int ProtectedProperty { get; set; }
            [PublicAPI]
            internal int InternalProperty { get; set; }


            [PublicAPI]
            [return: PublicAPI]
            public void GenericMethod<[PublicAPI]T>()
            {
            }

            [CanBeNull]
            public event EventHandler PublicEvent;
            [CanBeNull]
            private event EventHandler PrivateEvent;
            [CanBeNull]
            protected event EventHandler ProtectedEvent;

            [PublicAPI]
            public NestedClass() { }
            [PublicAPI]
            private NestedClass(int privateConstructorArgs) { }
            [PublicAPI]
            protected NestedClass(double protectedConstructorArgs) { }

            [NotNull]
            public string NotNullMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
            {
                return "";
            }

            [NotNull]
            [StringFormatMethod("format")]
            public string StringFormat([NotNull] string format, [NotNull] params object[] args)
            {
                return "";
            }

            [ContractAnnotation("param1:null => null")]
            public string ContractMethod(string param1, string param2)
            {
                return "";
            }

            public string MethodWithoutAnyAttribute(string arg)
            {
                return "";
            }

            [NotNull]
            internal string InternalMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
            {
                return "";
            }

            [NotNull]
            private string PrivateMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
            {
                return "";
            }
        }
    }

    [UsedImplicitly]
    internal class InternalClass
    {
        [PublicAPI]
        int privateField;
        [NotNull]
        public object publicField;
        [NotNull]
        protected object protectedField;

        [PublicAPI]
        public int PublicProperty { get; set; }
        [PublicAPI]
        private int PrivateProperty { get; set; }
        [PublicAPI]
        protected int ProtectedProperty { get; set; }
        [PublicAPI]
        internal int InternalProperty { get; set; }


        [PublicAPI]
        [return: PublicAPI]
        public void GenericMethod<[PublicAPI]T>()
        {
        }

        [CanBeNull]
        public event EventHandler PublicEvent;
        [CanBeNull]
        private event EventHandler PrivateEvent;
        [CanBeNull]
        protected event EventHandler ProtectedEvent;

        [PublicAPI]
        public InternalClass() { }
        [PublicAPI]
        private InternalClass(int privateConstructorArgs) { }
        [PublicAPI]
        protected InternalClass(double protectedConstructorArgs) { }

        [NotNull]
        public string NotNullMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
        {
            return "";
        }

        [NotNull]
        [StringFormatMethod("format")]
        public string StringFormat([NotNull] string format, [NotNull] params object[] args)
        {
            return "";
        }

        [ContractAnnotation("param1:null => null")]
        public string ContractMethod(string param1, string param2)
        {
            return "";
        }

        public string MethodWithoutAnyAttribute(string arg)
        {
            return "";
        }

        [NotNull]
        internal string InternalMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
        {
            return "";
        }

        [NotNull]
        private string PrivateMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
        {
            return "";
        }

        [UsedImplicitly]
        public class NestedClass
        {
            [PublicAPI]
            int privateField;
            [NotNull]
            public object publicField;
            [NotNull]
            protected object protectedField;

            [PublicAPI]
            public int PublicProperty { get; set; }
            [PublicAPI]
            private int PrivateProperty { get; set; }
            [PublicAPI]
            protected int ProtectedProperty { get; set; }
            [PublicAPI]
            internal int InternalProperty { get; set; }


            [PublicAPI]
            [return: PublicAPI]
            public void GenericMethod<[PublicAPI]T>()
            {
            }

            [CanBeNull]
            public event EventHandler PublicEvent;
            [CanBeNull]
            private event EventHandler PrivateEvent;
            [CanBeNull]
            protected event EventHandler ProtectedEvent;

            [PublicAPI]
            public NestedClass() { }
            [PublicAPI]
            private NestedClass(int privateConstructorArgs) { }
            [PublicAPI]
            protected NestedClass(double protectedConstructorArgs) { }

            [NotNull]
            public string NotNullMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
            {
                return "";
            }

            [NotNull]
            [StringFormatMethod("format")]
            public string StringFormat([NotNull] string format, [NotNull] params object[] args)
            {
                return "";
            }

            [ContractAnnotation("param1:null => null")]
            public string ContractMethod(string param1, string param2)
            {
                return "";
            }

            public string MethodWithoutAnyAttribute(string arg)
            {
                return "";
            }

            [NotNull]
            internal string InternalMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
            {
                return "";
            }

            [NotNull]
            private string PrivateMethod([NotNull] string notNull, [CanBeNull] string canBeNull, [NotNull, ItemNotNull] string[] notNullAndItemNotNull)
            {
                return "";
            }
        }
    }
}