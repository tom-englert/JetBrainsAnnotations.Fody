using JetBrains.Annotations;
// ReSharper disable All
#pragma warning disable 169

[assembly: PublicAPI]
[module: PublicAPI]

namespace AssemblyToProcess
{
    using System;

    [UsedImplicitly]
    public class SimpleClass
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
        public SimpleClass() { }
        [PublicAPI]
        private SimpleClass([PublicAPI] int privateConstructorArgs) { }
        [PublicAPI]
        protected SimpleClass([PublicAPI] double protectedConstructorArgs) { }

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